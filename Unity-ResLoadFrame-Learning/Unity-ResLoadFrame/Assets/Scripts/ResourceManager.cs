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
    // 正在异步加载的Dic
    protected Dictionary<uint, AsyncLoadResParam> m_LoadingAssetDic = new Dictionary<uint, AsyncLoadResParam>();

    public void Init(MonoBehaviour mono)
    {
        for (int i = 0; i < (int)LoadResPriority.RES_NUM; i++)
        {
            m_LoadingAssetList[i] = new List<AsyncLoadResParam>();
        }
        m_Startmono = mono;
        m_Startmono.StartCoroutine(AsyncLoadCor());
    }

    /// <summary>
    /// 同步资源加载， 用于加载不需要实例化的资源文件
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
            if (item.m_Obj != null)
            {
                obj = item.m_Obj as T;
            }
            else
            {
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

    protected void WashOut()
    {
        // 当前内存使用大于80% 清空没用的资源
        //{
        //    if (m_NoRefrenceAssetMapList.Size() <= 0)
        //        break;

        //    ResourceItem item = m_NoRefrenceAssetMapList.Back();
        //    DestoryResourceItem(item, true);
        //    m_NoRefrenceAssetMapList.Pop();
        //}
    }

    protected void DestoryResourceItem(ResourceItem item, bool destroyCache = false)
    {
        if (item == null || item.RefCount > 0)
        {
            return;
        }

        if (!AssetDic.Remove(item.m_Crc))
        {
            return;
        }

        if (!destroyCache)
        {
            m_NoRefrenceAssetMapList.InsertToHead(item);
            return;
        }

        AssetBundleManager.Instance.ReleaseAsset(item);

        if (item.m_Obj != null)
        {
            item.m_Obj = null;
        }
    }

#if UNITY_EDITOR
    protected T LoadAssetByEditor<T>(string path) where T : UnityEngine.Object
    {
        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
    }
#endif

    ResourceItem GetCacheResouceItem(uint crc, int addRefcount = 1)
    {
        ResourceItem item = null;
        if (AssetDic.TryGetValue(crc, out item)){
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

    /// <summary>
    /// 异步加载协程
    /// </summary>
    /// <returns></returns>
    IEnumerator AsyncLoadCor()
    {
        
        while (true)
        {
            long lastYieldTime = System.DateTime.Now.Ticks;
            for (int i = 0; i < (int)LoadResPriority.RES_NUM; i++)
            {
                List<AsyncLoadResParam> loadingList = m_LoadingAssetList[i];
                if (loadingList.Count <= 0)
                    continue;
                AsyncLoadResParam loadingItem = loadingList[0];
            }

            yield return null;
        }
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    public void AsyncLoadResource(string path, OnAsyncObjFinish dealFinish, LoadResPriority priority, object param1 = null, object param2 = null, object param3 = null, uint crc = 0)
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
    public LoadResPriority m_Priority = LoadResPriority.RES_SLOW;

    public void Reset()
    {
        m_CallBackList.Clear();
        m_Crc = 0;
        m_Path = "";
        m_Priority = LoadResPriority.RES_SLOW;
    }
}

public class AsyncCallBack
{
    // 事件
    public OnAsyncObjFinish m_DealFinish = null;
    // 参数
    public object m_Param1 = null, m_Param2 = null, m_Param3 = null;

    public void Reset()
    {
        m_DealFinish = null;
        m_Param1 = null;
        m_Param2 = null;
        m_Param3 = null;
    }
}

public delegate void OnAsyncObjFinish(string path, Object obj, object param1 = null, object param2 = null, object param3 = null);