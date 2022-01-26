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

public class ObjectManager :Singleton<ObjectManager>
{
    // 对象池默认回收节点
    public Transform RecyclePoolTrs;
    // 场景节点
    public Transform SceneTrs;
    protected Dictionary<uint, List<ResourceObj>> m_ObjectPoolDic = new Dictionary<uint, List<ResourceObj>>();
    protected ClassObjectPool<ResourceObj> m_ResourceObjClassPool = ObjectManager.Instance.GetOrCreateClassPool<ResourceObj>(1000);
    // 暂存ResourceObj的字典
    protected Dictionary<int, ResourceObj> m_ResourceObjDic = new Dictionary<int, ResourceObj>();
    public void Init(Transform recyclePoolTrs, Transform sceneTrs)
    {
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
            ResourceObj resObj = st[0];
            st.RemoveAt(0);
            GameObject obj = resObj.m_CloneObj;
            if (!System.Object.ReferenceEquals(obj, null))
            {
                resObj.m_Already = false;
#if UNITY_EDITOR
                if (obj.name.EndsWith("(Recycle)"))
                {
                    obj.name.Replace("(Recycle)", "");
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

            }

        }
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
