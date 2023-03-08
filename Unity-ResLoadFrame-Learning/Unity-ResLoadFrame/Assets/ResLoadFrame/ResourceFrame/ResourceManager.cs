/****************************************************
    文件：ResourceManager.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager :Singleton<ResourceManager>
{
    public bool m_LoadFormAssetBundle = false;

    protected long m_Guid = 0;

    // 缓存使用的资源列表
    public Dictionary<uint, ResourceItem> AssetDic { get; set; } = new Dictionary<uint, ResourceItem>();

    // 缓存引用计数为零的资源列表， 达到缓存最大计数的时候清除列表最没用的资源
    protected CMapList<ResourceItem> m_NoRefrenceAssetMapList = new CMapList<ResourceItem>();

    // 异步加载中间类、回调类对象池
    protected ClassObjectPool<AsyncLoadResParam> m_AsyncLoadResParamPool = new ClassObjectPool<AsyncLoadResParam>(50);
    protected ClassObjectPool<AsyncCallBack> m_AsyncCallBackPool = new ClassObjectPool<AsyncCallBack>(100);

    // Mono
    protected MonoBehaviour m_Startmono;
    // 正在加载中的异步加载队列
    protected List<AsyncLoadResParam>[] m_LoadingAssetList = new List<AsyncLoadResParam>[(int)LoadResPriority.RES_NUM];
    /// <summary>
    /// 正在异步加载的资源字典
    /// </summary>
    protected Dictionary<uint, AsyncLoadResParam> m_LoadingAssetDic = new Dictionary<uint, AsyncLoadResParam>();

    // 异步加载最长时间 单位微秒
    private const long MAXLOADRESTIME = (long)(2 * 1e5);
    /// <summary>
    /// 最大缓存个数
    /// </summary>
    private const int MAXCACHECOUNT = 500;

    public void Init(MonoBehaviour mono)
    {
        for (int i = 0; i < (int)LoadResPriority.RES_NUM; i++)
        {
            m_LoadingAssetList[i] = new List<AsyncLoadResParam>();
        }
        m_Startmono = mono;
        m_Startmono.StartCoroutine(AsyncLoadCor()); 
    }

    
    public long CreateGuid()
    {
        return m_Guid++;
    }

    #region 同步资源加载

    /// <summary>
    /// 同步资源加载 加载需要实例化的游戏对象
    /// </summary>
    public ResourceObj LoadResource(string path, ResourceObj resObj)
    {
        if (resObj == null)
            return null;
        uint crc = resObj.m_Crc == 0 ? CRC32.GetCRC32(path) : resObj.m_Crc;

        ResourceItem item = GetCacheResouceItem(crc);
        if (item != null)
        {
            resObj.m_ResItem = item;
            return resObj;
        }
        Object obj = null;
#if UNITY_EDITOR
        if (!m_LoadFormAssetBundle)
        {
            item = AssetBundleManager.Instance.FindResourcesItem(crc);
            if (item != null && item.m_AssetBundle != null)
            {
                if (item.m_Obj != null)
                {
                    obj = item.m_Obj as Object;
                }
                else
                {
                    obj = item.m_AssetBundle.LoadAsset<Object>(item.m_AssetName);
                }
            }
            else
            {
                if (item == null)
                {
                    item = new ResourceItem();
                    item.m_Crc = crc;
                }
                obj = LoadAssetByEditor<Object>(path);
            }
        }
#endif
        if (obj == null)
        {
            item = AssetBundleManager.Instance.LoadResourceAssetBundle(crc);
            if (item != null && item.m_AssetBundle != null)
            {
                if (item.m_Obj != null)
                {
                    obj = item.m_Obj as Object;
                }
                else
                {
                    obj = item.m_AssetBundle.LoadAsset<Object>(item.m_AssetName);
                }

            }
        }
        CacheResource(path, ref item, crc, obj);
        resObj.m_ResItem = item;
        item.m_Clear = resObj.m_bClear;
        return resObj;
    }

    /// <summary>
    /// 同步资源加载 用于加载不需要实例化的资源文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T LoadResource<T>(string path) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }
        uint crc = CRC32.GetCRC32(path);
        ResourceItem item = GetCacheResouceItem(crc);
        if (item != null)
        {
            return item.m_Obj as T;
        }

        T obj = null;
#if UNITY_EDITOR
        if (!m_LoadFormAssetBundle)
        {
            item = AssetBundleManager.Instance.FindResourcesItem(crc);
            if (item != null && item.m_AssetBundle != null)
            {
                if (item.m_Obj != null)
                {
                    obj = item.m_Obj as T;
                }
                else
                {
                    obj = item.m_AssetBundle.LoadAsset<T>(item.m_AssetName);
                }
            }
            else
            {
                if (item == null)
                {
                    item = new ResourceItem();
                    item.m_Crc = crc;
                }
                obj = LoadAssetByEditor<T>(path);
            }
        }
        
        
#endif
        if (obj == null)
        {
            item = AssetBundleManager.Instance.LoadResourceAssetBundle(crc);
            if (item != null && item.m_AssetBundle != null)
            {
                if (item.m_Obj != null)
                {
                    obj = item.m_Obj as T;
                }
                else
                {
                    obj = item.m_AssetBundle.LoadAsset<T>(item.m_AssetName);
                }
                
            }
        }
        
        CacheResource(path, ref item, crc, obj);
        return obj;
    }
    #endregion

    #region 异步加载资源

    /// <summary>
    /// 异步加载协程
    /// </summary>
    /// <returns></returns>
    IEnumerator AsyncLoadCor()
    {
        List<AsyncCallBack> callBackList = new List<AsyncCallBack>();
        long lastYieldTime = 0;
        while (true)
        {
            bool haveYield = false;
            for (int i = 0; i < (int)LoadResPriority.RES_NUM; i++)
            {
                if (m_LoadingAssetList[(int)LoadResPriority.RES_HIGHT].Count > 0)
                {
                    i = (int)LoadResPriority.RES_HIGHT;
                }
                else if (m_LoadingAssetList[(int)LoadResPriority.RES_MIDDLE].Count > 0)
                {
                    i = (int)LoadResPriority.RES_MIDDLE;
                }

                List <AsyncLoadResParam> loadingList = m_LoadingAssetList[i];
                if (loadingList.Count <= 0)
                    continue;
                AsyncLoadResParam loadingItem = loadingList[0];
                loadingList.RemoveAt(0);
                callBackList = loadingItem.m_CallBackList;

                Object obj = null;
                ResourceItem item = null;
#if UNITY_EDITOR
                if (!m_LoadFormAssetBundle)
                {
                    if (loadingItem.m_IsSprite)
                    {
                        obj = LoadAssetByEditor<Sprite>(loadingItem.m_Path);
                    }
                    else
                    {
                        obj = LoadAssetByEditor<Object>(loadingItem.m_Path);
                    }
                    
                    // 延时 模拟异步加载
                    yield return new WaitForSeconds(0.5f);

                    item = AssetBundleManager.Instance.FindResourcesItem(loadingItem.m_Crc);
                    if (item == null)
                    {
                        item = new ResourceItem();
                        item.m_Crc = loadingItem.m_Crc;
                    }
                }
#endif
                if (obj == null)
                {
                    item = AssetBundleManager.Instance.LoadResourceAssetBundle(loadingItem.m_Crc);
                    if (item != null && item.m_AssetBundle != null)
                    {
                        AssetBundleRequest abRequest = null;
                        if (loadingItem.m_IsSprite)
                        {
                            abRequest = item.m_AssetBundle.LoadAssetAsync<Sprite>(item.m_AssetName);
                        }
                        else
                        {
                            abRequest = item.m_AssetBundle.LoadAssetAsync(item.m_AssetName);
                        }

                        yield return abRequest;
                        if (abRequest.isDone)
                        {
                            obj = abRequest.asset;
                        }
                        lastYieldTime = System.DateTime.Now.Ticks;
                    }
                }
                CacheResource(loadingItem.m_Path, ref item, loadingItem.m_Crc, obj, callBackList.Count);

                for (int j = 0; j < callBackList.Count; j++)
                {
                    AsyncCallBack callBack = callBackList[j];
                    if (callBack != null && callBack.m_DealFinish != null)
                    {
                        callBack.m_DealFinish(loadingItem.m_Path, obj, callBack.m_Param1, callBack.m_Param2, callBack.m_Param3);
                        callBack.m_DealFinish = null;
                    }
                    if (callBack != null && callBack.m_DealResObjFinish != null && callBack.m_ResObj != null)
                    {
                        ResourceObj tmpResObj = callBack.m_ResObj;
                        tmpResObj.m_ResItem = item;
                        callBack.m_DealResObjFinish(loadingItem.m_Path, tmpResObj, tmpResObj.m_Param1, tmpResObj.m_Param2, tmpResObj.m_Param3);
                        callBack.m_DealResObjFinish = null;
                    }
                    callBack.Reset();
                    m_AsyncCallBackPool.Recycle(callBack);
                }
                obj = null;
                callBackList.Clear();
                m_LoadingAssetDic.Remove(loadingItem.m_Crc);

                loadingItem.Reset();
                m_AsyncLoadResParamPool.Recycle(loadingItem);

                if (System.DateTime.Now.Ticks - lastYieldTime > MAXLOADRESTIME)
                {
                    yield return null;
                    haveYield = true;
                    lastYieldTime = System.DateTime.Now.Ticks;
                }
            }

            if (!haveYield && System.DateTime.Now.Ticks - lastYieldTime > MAXLOADRESTIME)
            {
                yield return null;
                haveYield = false;
                lastYieldTime = System.DateTime.Now.Ticks;
            }

            yield return 0;
        }
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    public void AsyncLoadResource(string path, OnAsyncObjFinish dealFinish, LoadResPriority priority, bool isSprite = false, object param1 = null, object param2 = null, object param3 = null, uint crc = 0)
    {
        if (crc == 0)
        {
            crc = CRC32.GetCRC32(path);
        }
        ResourceItem item = GetCacheResouceItem(crc);
        if (item != null)
        {
            if (dealFinish != null)
            {
                dealFinish(path, item.m_Obj, param1, param2, param3);
            }
            return;
        }
        // 判断是否正在加载中
        AsyncLoadResParam param = null;
        if (!m_LoadingAssetDic.TryGetValue(crc, out param) || param == null)
        {
            param = m_AsyncLoadResParamPool.Spawn(true);
            param.m_Crc = crc;
            param.m_Path = path;
            param.m_Priority = priority;
            param.m_IsSprite = isSprite;
            m_LoadingAssetDic.Add(crc, param);
            m_LoadingAssetList[(int)priority].Add(param);
        }

        // 添加回调列表
        AsyncCallBack callBack = m_AsyncCallBackPool.Spawn(true);
        callBack.m_DealFinish = dealFinish;
        callBack.m_Param1 = param1;
        callBack.m_Param2 = param2;
        callBack.m_Param3 = param3;
        param.m_CallBackList.Add(callBack);

    }

    public void AsyncLoadResource(string path, ResourceObj resObj, OnAsyncResObjFinish dealFinish, LoadResPriority priority)
    {
        ResourceItem item = GetCacheResouceItem(resObj.m_Crc);
        if (item != null)
        {
            resObj.m_ResItem = item;
            if (dealFinish != null)
            {
                dealFinish(path, resObj);
            }
            return;
        }

        // 判断是否正在加载中
        AsyncLoadResParam param = null;
        if (!m_LoadingAssetDic.TryGetValue(resObj.m_Crc, out param) || param == null)
        {
            param = m_AsyncLoadResParamPool.Spawn(true);
            param.m_Crc = resObj.m_Crc;
            param.m_Path = path;
            param.m_Priority = priority;
            m_LoadingAssetDic.Add(resObj.m_Crc, param);
            m_LoadingAssetList[(int)priority].Add(param);
        }

        // 添加回调列表
        AsyncCallBack callBack = m_AsyncCallBackPool.Spawn(true);
        callBack.m_DealResObjFinish = dealFinish;
        callBack.m_ResObj = resObj;
        param.m_CallBackList.Add(callBack);
    }

    #endregion

    #region 卸载资源

    public bool ReleaseResource(ResourceObj resObj, bool destoryObj = false)
    {
        if (resObj == null)
            return false;

        uint crc = resObj.m_Crc;
        ResourceItem item = null;

        if (!AssetDic.TryGetValue(crc, out item) || item == null)
        {
            Debug.LogError($"AssetDic不存在该资源: {resObj.m_CloneObj.name}");
            return false;
        }
        GameObject.Destroy(resObj.m_CloneObj);
        item.RefCount--;
        DestoryResourceItem(item, destoryObj);

        return true;
    }

    public bool ReleaseResource(Object obj, bool destoryObj = false)
    {
        if (obj == null)
            return false;

        ResourceItem item = null;
        foreach (ResourceItem res in AssetDic.Values)
        {
            if (res.m_Guid == obj.GetInstanceID())
            {
                item = res;
            }
        }
        if (item == null)
        {
            Debug.LogError($"AssetDic不存在该资源: {obj.name}");
            return false;
        }

        item.RefCount--;
        DestoryResourceItem(item, destoryObj);

        return true;
    }

    public bool ReleaseResource(string path, bool destoryObj = false)
    {
        if (string.IsNullOrEmpty(path))
            return false;
        uint crc = CRC32.GetCRC32(path);
        ResourceItem item = null;
        if (!AssetDic.TryGetValue(crc, out item) || item == null)
        {
            Debug.LogError($"AssetDic不存在该资源: {path}");
            return false;
        }

        item.RefCount--;
        DestoryResourceItem(item, destoryObj);

        return true;
    }
    #endregion

    public bool CancleAsyncLoad(ResourceObj resObj)
    {
        AsyncLoadResParam param = null;
        if (m_LoadingAssetDic.TryGetValue(resObj.m_Crc, out param) && m_LoadingAssetList[(int)param.m_Priority].Contains(param))
        {
            for (int i = param.m_CallBackList.Count - 1; i >= 0; i--)
            {
                AsyncCallBack tmpCb = param.m_CallBackList[i];
                if (tmpCb != null && resObj == tmpCb.m_ResObj)
                {
                    tmpCb.Reset();
                    m_AsyncCallBackPool.Recycle(tmpCb);
                }
            }
            if (param.m_CallBackList.Count <= 0)
            {
                param.Reset();
                m_LoadingAssetList[(int)param.m_Priority].Remove(param);
                m_AsyncLoadResParamPool.Recycle(param);
                m_LoadingAssetDic.Remove(param.m_Crc);
                return true;
            }
        }
        return false;
    }

    private void CacheResource(string path, ref ResourceItem item, uint crc, Object obj, int addRefcount = 1)
    {
        // 清除最早没有使用的资源
        WashOut();

        if (item == null)
        {
            Debug.LogError($"ResourceItem is null, path: {path}");
        }

        if (obj == null)
        {
            Debug.LogError($"ResourceLoad Fail: {path}");
        }

        item.m_Obj = obj;
        item.m_Guid = obj.GetInstanceID();
        item.m_LastUserTime = Time.realtimeSinceStartup;
        item.RefCount += addRefcount;
        ResourceItem oldItem = null;
        if (AssetDic.TryGetValue(item.m_Crc, out oldItem))
        {
            AssetDic[item.m_Crc] = item;
        }
        else
        {
            AssetDic.Add(item.m_Crc, item);
        }
    }

    /// <summary>
    /// 从缓存中获取资源
    /// </summary>
    /// <param name="crc"></param>
    /// <param name="addRefcount"></param>
    /// <returns></returns>
    ResourceItem GetCacheResouceItem(uint crc, int addRefcount = 1)
    {
        ResourceItem item = null;
        if (AssetDic.TryGetValue(crc, out item))
        {
            if (item != null)
            {
                item.RefCount += addRefcount;
                item.m_LastUserTime = Time.realtimeSinceStartup;

                if (item.RefCount <= 1)
                {
                    m_NoRefrenceAssetMapList.Remove(item);
                }
            }

        }

        return item;
    }

    protected void WashOut()
    {
        // 大于缓存个数时，释放一半资源
        while(m_NoRefrenceAssetMapList.Size() >= MAXCACHECOUNT)
        {
            for (int i = 0; i < MAXCACHECOUNT / 2; i++)
            {
                ResourceItem item = m_NoRefrenceAssetMapList.Back();
                DestoryResourceItem(item, true);
            }
            
        }
        //{
        //    if (m_NoRefrenceAssetMapList.Size() <= 0)
        //        break;

        //    ResourceItem item = m_NoRefrenceAssetMapList.Back();
        //    DestoryResourceItem(item, true);
        //    m_NoRefrenceAssetMapList.Pop();
        //}
    }

    /// <summary>
    /// 用于跳转场景时清空不需要缓存的资源
    /// </summary>
    public void ClearCache()
    {
        List<ResourceItem> tempList = new List<ResourceItem>();
        foreach (ResourceItem item in AssetDic.Values)
        {
            if (!item.m_Clear)
                tempList.Add(item);
        }
        foreach (ResourceItem item in tempList)
        {
            DestoryResourceItem(item, true);
        }
        tempList.Clear();
    }

    /// <summary>
    /// 预加载
    /// </summary>
    /// <param name="path"></param>
    public void PreloadRes(string path)
    {
        if (string.IsNullOrEmpty(path))
            return;
        uint crc = CRC32.GetCRC32(path);
        ResourceItem item = GetCacheResouceItem(crc, 0);
        if (item != null)
        {
            return;
        }

        Object obj = null;
#if UNITY_EDITOR
        if (!m_LoadFormAssetBundle)
        {
            item = AssetBundleManager.Instance.FindResourcesItem(crc);
            if (item != null && item.m_Obj != null)
            {
                obj = item.m_Obj;
            }
            else
            {
                if (item == null)
                {
                    item = new ResourceItem();
                    item.m_Crc = crc;
                }
                obj = LoadAssetByEditor<Object>(path);
            }
        }
#endif
        if (obj == null)
        {
            item = AssetBundleManager.Instance.LoadResourceAssetBundle(crc);
            if (item != null && item.m_AssetBundle != null)
            {
                if (item.m_Obj != null)
                {
                    obj = item.m_Obj;
                }
                else
                {
                    obj = item.m_AssetBundle.LoadAsset<Object>(item.m_AssetName);
                }

            }
        }

        CacheResource(path, ref item, crc, obj);
        // 跳转场景不清空缓存
        item.m_Clear = false;
        ReleaseResource(obj, false);
    }

    

    protected void DestoryResourceItem(ResourceItem item, bool destroyCache = false)
    {
        if (item == null || item.RefCount > 0)
        {
            return;
        }

        if (!destroyCache)
        {
            m_NoRefrenceAssetMapList.InsertToHead(item);
            return;
        }

        if (!AssetDic.Remove(item.m_Crc))
        {
            return;
        }

        m_NoRefrenceAssetMapList.Remove(item);

        AssetBundleManager.Instance.ReleaseAsset(item);
        // 清空GameObject资源对应的对象池
        ObjectManager.Instance.ClearPoolObject(item.m_Crc);

        if (item.m_Obj != null)
        {
#if UNITY_EDITOR
            if (!m_LoadFormAssetBundle)
            {
                if (item.m_Obj.GetType() != typeof(GameObject))
                    Resources.UnloadAsset(item.m_Obj);
                else
                    // 感觉有点问题 由于只在编辑器下运行 先保留
                    Resources.UnloadUnusedAssets();
            }
#endif
            item.m_Obj = null;
        }
    }

#if UNITY_EDITOR
    protected T LoadAssetByEditor<T>(string path) where T : UnityEngine.Object
    {
        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
    }
#endif

    public int IncreaseResourceRef(ResourceObj resObj, int count = 1)
    {
        return resObj != null ? IncreaseResourceRef(resObj.m_Crc, count) : 0;
    }

    public int IncreaseResourceRef(uint crc, int count = 1)
    {
        ResourceItem item = null;
        if (!AssetDic.TryGetValue(crc, out item) || item == null)
            return 0;

        item.RefCount += count;
        item.m_LastUserTime = Time.realtimeSinceStartup;

        return item.RefCount;
    }

    public int DecreaseResourceRef(ResourceObj resObj, int count = 1)
    {
        return resObj != null ? DecreaseResourceRef(resObj.m_Crc, count) : 0;
    }

    public int DecreaseResourceRef(uint crc, int count = 1)
    {
        ResourceItem item = null;
        if (!AssetDic.TryGetValue(crc, out item) || item == null)
            return 0;

        item.RefCount -= count;
        item.m_LastUserTime = Time.realtimeSinceStartup;

        return item.RefCount;
    }
}

#region 链表相关

/// <summary>
/// 双向链表结构节点
/// </summary>
/// <typeparam name="T"></typeparam>
public class DoubleLinkedListNode<T> where T : class, new() 
{
    // 前一个节点
    public DoubleLinkedListNode<T> prev = null;
    // 后一个节点
    public DoubleLinkedListNode<T> next = null;
    // 当前节点
    public T t = null;
}

/// <summary>
/// 双向链表结构类
/// </summary>
/// <typeparam name="T"></typeparam>
public class DoubleLinkedList<T> where T : class, new()
{
    // 表头
    public DoubleLinkedListNode<T> Head = null;
    // 表尾
    public DoubleLinkedListNode<T> Tail = null;
    // 结构类对象池
    protected ClassObjectPool<DoubleLinkedListNode<T>> m_DoubleLinkNodePool = new ClassObjectPool<DoubleLinkedListNode<T>>(500);
    // 个数
    protected int m_Count = 0;
    public int Count => m_Count;

    public DoubleLinkedListNode<T> AddToHeader(T t)
    {
        DoubleLinkedListNode<T> pList = m_DoubleLinkNodePool.Spawn(true);
        pList.next = null;
        pList.prev = null;
        pList.t = t;
        return AddToHeader(pList);
        
    }

    public DoubleLinkedListNode<T> AddToHeader(DoubleLinkedListNode<T> pNode)
    {
        if (pNode == null)
            return null;
        pNode.prev = null;
        if (Head == null)
        {
            // 如果头节点为空 则pNode即是头节点也是尾节点
            Head = Tail = pNode;
        }
        else
        {
            pNode.next = Head;
            Head.prev = pNode;
            Head = pNode;
        }
        m_Count++;
        return Head;
    }

    public DoubleLinkedListNode<T> AddToTail(T t)
    {
        DoubleLinkedListNode<T> pList = m_DoubleLinkNodePool.Spawn(true);
        pList.next = null;
        pList.prev = null;
        pList.t = t;
        return AddToTail(pList);

    }

    public DoubleLinkedListNode<T> AddToTail(DoubleLinkedListNode<T> pNode)
    {
        if (pNode == null)
            return null;
        pNode.next = null;
        if (Tail == null)
        {
            // 如果头节点为空 则pNode即是头节点也是尾节点
            Head = Tail = pNode;
        }
        else
        {
            pNode.prev = Tail;
            Tail.next = pNode;
            Tail = pNode;
        }
        m_Count++;
        return Tail;
    }

    /// <summary>
    /// 移除某个节点
    /// </summary>
    /// <param name="pNode"></param>
    public void RemoveNode(DoubleLinkedListNode<T> pNode)
    {
        if (pNode == null)
            return;
        if (pNode == Head)
            Head = pNode.next;
        if (pNode == Tail)
            Tail = pNode.prev;
        if (pNode.prev != null)
            pNode.prev.next = pNode.next;
        if (pNode.next != null)
            pNode.next.prev = pNode.prev;

        pNode.next = pNode.prev = null;
        pNode.t = null;
        m_DoubleLinkNodePool.Recycle(pNode);
        m_Count--;
    }

    /// <summary>
    /// 移动某个节点到头部
    /// </summary>
    /// <param name="pNode"></param>
    public void MoveToHead(DoubleLinkedListNode<T> pNode)
    {
        if (pNode == null || pNode == Head)
            return;
        if (pNode.prev == null && pNode.next == null)
            return;

        if (pNode == Tail)
            Tail = pNode.prev;
        if (pNode.prev != null)
            pNode.prev.next = pNode.next;
        if (pNode.next != null)
            pNode.next.prev = pNode.prev;
        pNode.prev = null;
        pNode.next = Head;
        Head.prev = pNode;
        Head = pNode;
        if (Tail == null)
        {
            Tail = Head;
        }
    }
}

public class CMapList<T> where T : class, new()
{
    DoubleLinkedList<T> m_DLink = new DoubleLinkedList<T>();
    Dictionary<T, DoubleLinkedListNode<T>> m_FindMap = new Dictionary<T, DoubleLinkedListNode<T>>();

    // 注: 析构函数 在对象被销毁时调用
    ~CMapList()
    {
        Clear();
    }

    public void InsertToHead(T t)
    {
        DoubleLinkedListNode<T> node = null;
        if (m_FindMap.TryGetValue(t, out node) && node != null)
        {
            m_DLink.AddToHeader(node);
            return;
        }
        m_DLink.AddToHeader(t);
        m_FindMap.Add(t, m_DLink.Head);
    }

    /// <summary>
    /// 从表尾弹出一个节点
    /// </summary>
    public void Pop()
    {
        if (m_DLink.Tail != null)
        {
            Remove(m_DLink.Tail.t);
        }
    }

    /// <summary>
    /// 删除某个节点
    /// </summary>
    /// <param name="t"></param>
    public void Remove(T t)
    {
        DoubleLinkedListNode<T> node = null;
        if (!m_FindMap.TryGetValue(t, out node) || node == null)
            return;
        m_DLink.RemoveNode(node);
        m_FindMap.Remove(t);
    }

    /// <summary>
    /// 获取尾部节点
    /// </summary>
    /// <returns></returns>
    public T Back()
    {
        return m_DLink.Tail == null ? null : m_DLink.Tail.t;
    }

    /// <summary>
    /// 返回节点个数
    /// </summary>
    /// <returns></returns>
    public int Size()
    {
        return m_FindMap.Count;
    }

    /// <summary>
    /// 查询是否存在该节点
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public bool Find(T t)
    {
        DoubleLinkedListNode<T> node = null;
        if (!m_FindMap.TryGetValue(t, out node) || node == null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 刷新某个节点 将该节点移动至头部
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public bool Reflesh(T t)
    {
        DoubleLinkedListNode<T> node = null;
        if (!m_FindMap.TryGetValue(t, out node) || node == null)
        {
            return false;
        }

        m_DLink.MoveToHead(node);
        return true;
    }

    /// <summary>
    /// 清空链表
    /// </summary>
    public void Clear()
    {
        while(m_DLink.Tail != null)
        {
            Remove(m_DLink.Tail.t);
        }
    }
}

#endregion

/// <summary>
/// 资源加载优先级
/// </summary>
public enum LoadResPriority
{
    RES_HIGHT = 0,  // 最高优先级
    RES_MIDDLE,
    RES_SLOW,
    RES_NUM,
}

public class AsyncLoadResParam
{
    public List<AsyncCallBack> m_CallBackList = new List<AsyncCallBack>();
    public uint m_Crc;
    public string m_Path;
    public bool m_IsSprite = false;
    public LoadResPriority m_Priority = LoadResPriority.RES_SLOW;

    public void Reset()
    {
        m_CallBackList.Clear();
        m_Crc = 0;
        m_Path = "";
        m_IsSprite = false;
        m_Priority = LoadResPriority.RES_SLOW;
    }
}

public class AsyncCallBack
{
    // 事件
    public OnAsyncObjFinish m_DealFinish = null;

    public OnAsyncResObjFinish m_DealResObjFinish = null;
    public ResourceObj m_ResObj = null;
    // 参数
    public object m_Param1 = null, m_Param2 = null, m_Param3 = null;

    public void Reset()
    {
        m_DealFinish = null;
        m_DealResObjFinish = null;
        m_ResObj = null;
        m_Param1 = null;
        m_Param2 = null;
        m_Param3 = null;
    }
}
// 资源加载完成后的回调
public delegate void OnAsyncObjFinish(string path, Object obj, object param1 = null, object param2 = null, object param3 = null);
// 实例化对象完成后的回调
public delegate void OnAsyncResObjFinish(string path, ResourceObj resObj, object param1 = null, object param2 = null, object param3 = null);

public class ResourceObj
{
    public uint m_Crc = 0;
    public ResourceItem m_ResItem;
    public GameObject m_CloneObj = null;
    // 是否场景跳转时清除该资源
    public bool m_bClear = true;
    public long m_Guid = 0;
    // 是否已放回对象池
    public bool m_Already = false;

    public bool m_SetSceneParent = false;
    public OnAsyncObjFinish m_DealFinish = null;
    public object m_Param1 = null, m_Param2 = null, m_Param3 = null;
    public OfflineData m_OfflineData = null;
    public void Reset()
    {
        m_Crc = 0;
        m_ResItem = null;
        m_CloneObj = null;
        m_bClear = true;
        m_Guid = 0;
        m_Already = false;

        m_SetSceneParent = false;
        m_DealFinish = null;
        m_Param1 = m_Param2 = m_Param3 = null;

        m_OfflineData = null;
    }
}