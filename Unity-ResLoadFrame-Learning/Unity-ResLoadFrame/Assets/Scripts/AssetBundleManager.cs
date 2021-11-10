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
using UnityEngine;

public class AssetBundleManager :Singleton<AssetBundleManager>
{
    // 资源关系依赖配置表, 通过Crc查找
    protected Dictionary<uint, ResourceItem> m_ResourceItemDic = new Dictionary<uint, ResourceItem>();

    /// <summary>
    /// 加载依赖配置表
    /// </summary>
    /// <returns>是否加载成功</returns>
    public bool LoadAssetBundleConfig()
    {
        string configPath = Application.streamingAssetsPath + "/assetbundleconfig";
        AssetBundle configAB = AssetBundle.LoadFromFile(configPath);
        TextAsset textAsset = configAB.LoadAsset<TextAsset>("assetbundleconfig");
        if (textAsset = null)
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
}

public class ResourceItem
{
    public uint m_Crc = 0;
    public string m_AssetName = string.Empty;
    public string m_ABName = string.Empty;
    public List<string> m_DependAssetBundle = null;
    public AssetBundle m_AssetBundle = null;
}
