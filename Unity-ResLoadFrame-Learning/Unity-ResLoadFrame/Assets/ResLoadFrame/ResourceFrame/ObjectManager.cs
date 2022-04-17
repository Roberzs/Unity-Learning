/****************************************************
    文件：ObjectManager.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    // 对象池默认回收节点
    public Transform RecyclePoolTrs;
    // 场景节点
    public Transform SceneTrs;
    protected Dictionary<uint, List<ResourceObj>> m_ObjectPoolDic = new Dictionary<uint, List<ResourceObj>>();
    protected ClassObjectPool<ResourceObj> m_ResourceObjClassPool;
    // 暂存ResourceObj的字典
    protected Dictionary<int, ResourceObj> m_ResourceObjDic = new Dictionary<int, ResourceObj>();
    // 根据异步加载的Guid存储ResourceObj
    protected Dictionary<long, ResourceObj> m_AsyncResObjDic = new Dictionary<long, ResourceObj>();
    public void Init(Transform recyclePoolTrs, Transform sceneTrs)
    {
        m_ResourceObjClassPool = ObjectManager.Instance.GetOrCreateClassPool<ResourceObj>(1000);
        RecyclePoolTrs = recyclePoolTrs;
        SceneTrs = sceneTrs;
    }

    /// <summary>
    /// 从对象池取出对象
    /// </summary>
    /// <param name="crc"></param>
    /// <returns></returns>
    protected ResourceObj GetObjectFromPool(uint crc)
    {
        List<ResourceObj> st = null;
        if (m_ObjectPoolDic.TryGetValue(crc, out st) && st != null && st.Count > 0)
        {
            ResourceManager.Instance.IncreaseResourceRef(crc);
            ResourceObj resObj = st[0];
            st.RemoveAt(0);
            GameObject obj = resObj.m_CloneObj;
            if (!System.Object.ReferenceEquals(obj, null))
            {
                if (!System.Object.ReferenceEquals(resObj.m_OfflineData, null))
                {
                    resObj.m_OfflineData.ResetProp();
                }
                resObj.m_Already = false;
#if UNITY_EDITOR
                if (obj.name.EndsWith("(Recycle)"))
                {
                    obj.name = obj.name.Replace("(Recycle)", "");
                }
#endif
                return resObj;
            }
        }
        return null;
    }

    public GameObject InstantiateObject(string path, bool setSceneObj = false, bool bClear = true)
    {
        uint crc = CRC32.GetCRC32(path);
        ResourceObj resourceObj = GetObjectFromPool(crc);
        if (resourceObj == null)
        {
            resourceObj = m_ResourceObjClassPool.Spawn(true);
            resourceObj.m_Crc = crc;
            resourceObj.m_bClear = bClear;
            // 资源加载
            resourceObj = ResourceManager.Instance.LoadResource(path, resourceObj);
            if (resourceObj.m_ResItem.m_Obj != null)
            {
                resourceObj.m_CloneObj = GameObject.Instantiate(resourceObj.m_ResItem.m_Obj) as GameObject;
                resourceObj.m_OfflineData = resourceObj.m_CloneObj.GetComponent<OfflineData>();
            }
        }
        if (setSceneObj)
        {
            resourceObj.m_CloneObj.transform.SetParent(SceneTrs, false);
        }

        int tmpID = resourceObj.m_CloneObj.GetInstanceID();
        if (!m_ResourceObjDic.ContainsKey(tmpID))
        {
            m_ResourceObjDic.Add(tmpID, resourceObj);
        }
        return resourceObj.m_CloneObj;
    }

    public void ReleaseResource(GameObject obj, int maxCacheCount = -1, bool destoryCache = false, bool recycleParent = true)
    {
        if (obj == null)
            return;

        ResourceObj resObj = null;
        int tmpID = obj.GetInstanceID();
        if (!m_ResourceObjDic.TryGetValue(tmpID, out resObj))
        {
            Debug.Log($"{obj.name}不属于ObjectManager!");
            return;
        }
        if (resObj == null)
        {
            Debug.LogError($"缓存的ResourceObj为空");
            return;
        }
        if (resObj.m_Already)
        {
            Debug.LogError($"{obj.name}已被放回对象池");
            return;
        }
#if UNITY_EDITOR
        obj.name += "(Recycle)";
#endif
        List<ResourceObj> st = null;

        if (maxCacheCount == 0)
        {
            m_ResourceObjDic.Remove(tmpID);
            ResourceManager.Instance.ReleaseResource(resObj, destoryCache);
            resObj.Reset();
            m_ResourceObjClassPool.Recycle(resObj);
        }
        else
        {
            if (!m_ObjectPoolDic.TryGetValue(resObj.m_Crc, out st) || st == null)
            {
                st = new List<ResourceObj>();
                m_ObjectPoolDic.Add(resObj.m_Crc, st);
            }
            if (resObj.m_CloneObj)
            {
                if (recycleParent)
                {
                    resObj.m_CloneObj.transform.SetParent(RecyclePoolTrs);
                }
                else
                {
                    resObj.m_CloneObj.SetActive(false);
                }
            }
            if (maxCacheCount < 0 || st.Count < maxCacheCount)
            {
                st.Add(resObj);
                resObj.m_Already = true;

                ResourceManager.Instance.DecreaseResourceRef(resObj);
            }
            else
            {
                m_ResourceObjDic.Remove(tmpID);
                ResourceManager.Instance.ReleaseResource(resObj, destoryCache);
                resObj.Reset();
                m_ResourceObjClassPool.Recycle(resObj);
            }
        }
    }

    public long InstantiateObjectAsync(string path, OnAsyncObjFinish dealFinish, LoadResPriority priority, bool setSceneObj = false,
        object param1 = null, object param2 = null, object param3 = null, bool bClear = true)
    {
        if (string.IsNullOrEmpty(path))
            return -1;

        uint crc = CRC32.GetCRC32(path);
        ResourceObj resObj = GetObjectFromPool(crc);
        if (resObj != null)
        {
            if (setSceneObj)
            {
                resObj.m_CloneObj.transform.SetParent(SceneTrs, false);
            }

            if (dealFinish != null)
            {
                dealFinish(path, resObj.m_CloneObj, param1, param2, param3);
            }
            return resObj.m_Guid;
        }

        long guid = ResourceManager.Instance.CreateGuid();

        resObj = m_ResourceObjClassPool.Spawn(true);
        resObj.m_Crc = crc;
        resObj.m_Guid = guid;
        resObj.m_SetSceneParent = setSceneObj;
        resObj.m_bClear = bClear;
        resObj.m_DealFinish = dealFinish;
        resObj.m_Param1 = param1;
        resObj.m_Param2 = param2;
        resObj.m_Param3 = param3;

        m_AsyncResObjDic.Add(guid, resObj);

        ResourceManager.Instance.AsyncLoadResource(path, resObj, OnLoadResObjFinish, priority);

        return guid;
    }

    /// <summary>
    /// 根据实例化对象获取离线数据
    /// </summary>
    /// <param name="obj">GameObject</param>
    /// <returns></returns>
    public OfflineData FindOfflineData(GameObject obj)
    {
        OfflineData data = null;
        ResourceObj resObj = null;
        if ( m_ResourceObjDic.TryGetValue(obj.GetInstanceID(), out resObj) && !System.Object.ReferenceEquals(resObj, null))
        {
            data = resObj.m_OfflineData;
        }
        return data;
    }

    public void ClearCache()
    {
        List<uint> tmpList = new List<uint>();
        foreach (var key in m_ObjectPoolDic.Keys)
        {
            List<ResourceObj> st = m_ObjectPoolDic[key];
            for (int i = st.Count - 1; i >= 0; i--)
            {
                ResourceObj resObj = st[i];
                if (!System.Object.ReferenceEquals(resObj, null) && resObj.m_bClear)
                {
                    GameObject.Destroy(resObj.m_CloneObj);
                    m_ResourceObjDic.Remove(resObj.m_CloneObj.GetInstanceID());
                    resObj.Reset();
                    m_ResourceObjClassPool.Recycle(resObj);
                    st.Remove(resObj);
                }
            }
            if (st.Count <= 0)
            {
                tmpList.Add(key);
            }
        }
        for (int i = 0; i < tmpList.Count; i++)
        {
            uint tmp = tmpList[i];
            if (m_ObjectPoolDic.ContainsKey(tmp))
            {
                m_ObjectPoolDic.Remove(tmp);
            }
        }
        tmpList.Clear();
    }

    public void ClearPoolObject(uint crc)
    {
        List<uint> tmpList = new List<uint>();
        List<ResourceObj> st = null;
        if (!m_ObjectPoolDic.TryGetValue(crc, out st) || st == null)
            return;

        for (int i = st.Count - 1; i >= 0; i--)
        {
            ResourceObj resObj = st[i];
            if (resObj.m_bClear)
            {
                m_ResourceObjDic.Remove(resObj.m_CloneObj.GetInstanceID());
                GameObject.Destroy(resObj.m_CloneObj);
                resObj.Reset();
                m_ResourceObjClassPool.Recycle(resObj);
            }
            if (st.Count <= 0)
            {
                m_ObjectPoolDic.Remove(crc);
            }
        }
        
    }

    public bool IsAsyncLoading(long guid)
    {
        return m_AsyncResObjDic[guid] != null;
    }

    public bool IsObjectManagerCreate(GameObject obj)
    {
        return m_ResourceObjDic[obj.GetInstanceID()] != null;
    }

    public void CancleAsyncLoad(long guid)
    {
        ResourceObj resObj = null;
        if (m_AsyncResObjDic.TryGetValue(guid, out resObj) && ResourceManager.Instance.CancleAsyncLoad(resObj))
        {
            m_AsyncResObjDic.Remove(guid);
            resObj.Reset();
            m_ResourceObjClassPool.Recycle(resObj);
        }
    }

    private void OnLoadResObjFinish(string path, ResourceObj resObj, object param1 = null, object param2 = null, object param3 = null)
    {
        if (resObj == null)
            return;
        if (resObj.m_ResItem.m_Obj == null)
        {
#if UNITY_EDITOR
            Debug.LogError($"异步资源加载为空:{path}");
#endif
        }
        else
        {
            resObj.m_CloneObj = GameObject.Instantiate(resObj.m_ResItem.m_Obj) as GameObject;
            resObj.m_OfflineData = resObj.m_CloneObj.GetComponent<OfflineData>();
        }

        // 加载完后移除GUID
        if (m_AsyncResObjDic.ContainsKey(resObj.m_Guid))
        {
            m_AsyncResObjDic.Remove(resObj.m_Guid);
        }

        if (resObj.m_CloneObj != null && resObj.m_SetSceneParent)
        {
            resObj.m_CloneObj.transform.SetParent(SceneTrs, false);
        }
        if (resObj.m_DealFinish != null)
        {
            int tmpId = resObj.m_CloneObj.GetInstanceID();
            if (!m_ResourceObjDic.ContainsKey(tmpId))
            {
                m_ResourceObjDic.Add(tmpId, resObj);
            }

            resObj.m_DealFinish(path, resObj.m_CloneObj, param1, param2, param3);
        }
    }

    public void PreloadGameObject(string path, int count = 1, bool clear = false)
    {
        Debug.Log(count);
        List<GameObject> tmpGameObjectList = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject tmpGameObject = InstantiateObject(path, false, clear);
            tmpGameObjectList.Add(tmpGameObject);
        }
        for (int i = 0; i< count; i++)
        {
            GameObject tmpGameObject = tmpGameObjectList[i];
            ReleaseResource(tmpGameObject);
        }
        tmpGameObjectList.Clear();
    }

    #region 类对象池管理
    protected Dictionary<Type, object> m_ClassPoolDic = new Dictionary<Type, object>();

    /// <summary>
    /// 创建或获取类对象池 保存ClassObjectPool<T>, 通过Spawn与Recycle获取与回收类对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="maxCount"></param>
    /// <returns></returns>
    public ClassObjectPool<T> GetOrCreateClassPool<T>(int maxCount) where T : class, new()
    {
        Type type = typeof(T);
        object outObj = null;
        if (!m_ClassPoolDic.TryGetValue(type, out outObj) || outObj == null)
        {
            ClassObjectPool<T> newPool = new ClassObjectPool<T>(maxCount);
            m_ClassPoolDic.Add(type, newPool);
            return newPool;
        }
        return outObj as ClassObjectPool<T>;
        
    }
    #endregion

}
