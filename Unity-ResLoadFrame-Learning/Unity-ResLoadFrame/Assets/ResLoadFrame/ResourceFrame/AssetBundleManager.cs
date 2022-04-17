/****************************************************
    文件：AssetBundleManager.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class AssetBundleManager :Singleton<AssetBundleManager>
{
#if UNITY_EDITOR
    protected string m_ABRootPath = Application.dataPath + "/../AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString();
#else
    protected string m_ABRootPath = Application.streamingAssetsPath;
#endif

    protected string m_ABConfigABName = "assetbundleconfig";

    // 资源关系依赖配置表, 通过Crc查找
    protected Dictionary<uint, ResourceItem> m_ResourceItemDic = new Dictionary<uint, ResourceItem>();
    // 已加载的AB包
    protected Dictionary<uint, AssetBundleItem> m_AssetBundleItemDic = new Dictionary<uint, AssetBundleItem>();

    // AssetBundleItem对象池
    protected ClassObjectPool<AssetBundleItem> m_AssetBundleItemPool = ObjectManager.Instance.GetOrCreateClassPool<AssetBundleItem>(1024);

    /// <summary>
    /// 加载依赖配置表
    /// </summary>
    /// <returns>是否加载成功</returns>
    public bool LoadAssetBundleConfig()
    {
#if UNITY_EDITOR
        if (!ResourceManager.Instance.m_LoadFormAssetBundle)
        {
            return false;
        }
#endif
        string configPath = m_ABRootPath + "/" + m_ABConfigABName;
        AssetBundle configAB = AssetBundle.LoadFromFile(configPath);
        TextAsset textAsset = configAB.LoadAsset<TextAsset>(m_ABConfigABName);
        if (textAsset == null)
        {
            Debug.LogError("AssetBundleConfig is no exist!");
            return false;
        }

        MemoryStream stream = new MemoryStream(textAsset.bytes);
        BinaryFormatter bf = new BinaryFormatter();
        AssetBundleConfig config = bf.Deserialize(stream) as AssetBundleConfig;
        stream.Close();

        for (int i = 0; i < config.ABList.Count; i++)
        {
            ABBase abBase = config.ABList[i];
            ResourceItem item = new ResourceItem();
            item.m_Crc = abBase.Crc;
            item.m_AssetName = abBase.AssetName;
            item.m_ABName = abBase.ABName;
            item.m_DependAssetBundle = abBase.ABDependance;
            if (m_ResourceItemDic.ContainsKey(item.m_Crc)){
                Debug.LogError("重复的Crc: 资源名" + item.m_AssetName + " 包名:" + item.m_ABName);
                return false;
            }
            else
            {
                m_ResourceItemDic.Add(item.m_Crc, item);
            }
        }

        return true;
    }

    public ResourceItem LoadResourceAssetBundle(uint crc)
    {
        ResourceItem item = null;
        if (m_ResourceItemDic.TryGetValue(crc, out item))
        {
            if (item.m_AssetBundle == null)
            {
                item.m_AssetBundle = LoadAssetBundle(item.m_ABName);
            }
            if (item.m_DependAssetBundle != null)
            {
                foreach (var dependName in item.m_DependAssetBundle)
                {
                    LoadAssetBundle(dependName);
                }
            }

        }
        else
        {
            Debug.LogError($"LoadResourceAssetBundle Error: can not find crc: {crc} in AssetBundleConfig");
        }
        return item;
    }

    private AssetBundle LoadAssetBundle(string name)
    {
        AssetBundleItem item = null;
        uint crc = CRC32.GetCRC32(name);
        if (!m_AssetBundleItemDic.TryGetValue(crc, out item))
        {
            AssetBundle assetBundle = null;
            string fullPath = m_ABRootPath + "/" + name;
            // Android 与 IOS 不允许访问路径
            //if (File.Exists(fullPath))
            //{
            //    assetBundle = AssetBundle.LoadFromFile(fullPath);
            //}
            assetBundle = AssetBundle.LoadFromFile(fullPath);
            if (assetBundle == null)
            {
                Debug.LogError($"Load AssetBundle Error, path:{fullPath}");
            }
            item = m_AssetBundleItemPool.Spawn(true);
            item.assetBundle = assetBundle;
            item.RefCount++;
            m_AssetBundleItemDic.Add(crc, item);
        }
        else
        {
            item.RefCount++;
        }
        
        return item.assetBundle;

    }

    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="item"></param>
    public void ReleaseAsset(ResourceItem item)
    {
        if (item == null)
        {
            return;
        }
        if (item.m_DependAssetBundle != null && item.m_DependAssetBundle.Count > 0)
        {
            for (int i = 0; i < item.m_DependAssetBundle.Count; i++)
            {
                UnLoadAssetBundle(item.m_DependAssetBundle[i]);
            }
        }
        UnLoadAssetBundle(item.m_ABName);
    }

    private void UnLoadAssetBundle(string name)
    {
        uint crc = CRC32.GetCRC32(name);
        AssetBundleItem item = null;
        if (m_AssetBundleItemDic.TryGetValue(crc, out item) && item != null)
        {
            item.RefCount--;
            if (item.RefCount <= 0 && item.assetBundle != null)
            {
                item.assetBundle.Unload(true);
                item.Reset();
                m_AssetBundleItemPool.Recycle(item);
                m_AssetBundleItemDic.Remove(crc);
            }
        }
    }

    public ResourceItem FindResourcesItem(uint crc)
    {
        ResourceItem res = null;
        if (m_ResourceItemDic.TryGetValue(crc, out res))
        {
            return res;
        }
        return null;
    }
}

public class AssetBundleItem
{
    public AssetBundle assetBundle = null;
    public int RefCount;

    public void Reset()
    {
        assetBundle = null;
        RefCount = 0;
    }
}

public class ResourceItem
{
    public uint m_Crc = 0;
    public string m_AssetName = string.Empty;
    public string m_ABName = string.Empty;
    public List<string> m_DependAssetBundle = null;
    public AssetBundle m_AssetBundle = null;

    // 资源对象
    public Object m_Obj = null;
    // 资源唯一标识
    public int m_Guid = 0;
    // 资源最后使用的时间
    public float m_LastUserTime = 0.0f;
    // 引用计数
    protected int m_RefCount = 0;
    // 是否在场景跳转时清掉该资源
    public bool m_Clear = true;

    public int RefCount
    {
        get => m_RefCount;
        set
        {
            m_RefCount = value;
            if (m_RefCount < 0)
            {
                Debug.LogError($"refCount < 0, {((m_Obj != null) ? m_Obj.name : "name is null")}");
            }
        }
    }

}
