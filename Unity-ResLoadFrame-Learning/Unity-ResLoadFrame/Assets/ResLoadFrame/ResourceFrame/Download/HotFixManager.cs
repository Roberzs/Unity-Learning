using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class HotFixManager : Singleton<HotFixManager>
{
    private string m_CurVersion;
    public string CurVersion { get => m_CurVersion; }
    private string m_CurPackName;

    private MonoBehaviour m_Mono;

    private string m_ServerXmlPath = Application.persistentDataPath + "/ServerInfo.xml";
    private string m_LocalServerXmlPath = Application.persistentDataPath + "/LocalServerInfo.xml";
#if UNITY_EDITOR
    public string m_DownloadPath = Application.dataPath + "/../TempDownload/Download";
#elif UNITY_STANDALONE_WIN
    private string m_DownloadPath = Application.streamingAssetsPath + "/Download";
#else
    private string m_DownloadPath = Application.persistentDataPath + "/Download";
#endif

    public const string ServerPath = "http://192.168.50.121/";

    private ServerInfo m_ServerInfo;
    private ServerInfo m_LocalServerInfo;
    private VersionInfo m_GameVersion;
    private Patches m_CurrentPatches;
    public Patches CurrentPatches
    {
        get { return m_CurrentPatches; }
    }

    private Dictionary<string, Patch> m_HotFixDic = new Dictionary<string, Patch>();
    private List<Patch> m_DownloadList = new List<Patch>();
    private Dictionary<string, Patch> m_DownloadDic = new Dictionary<string, Patch>();
    private Dictionary<string, string> m_DownloadMd5Dic = new Dictionary<string, string>();
    // 服务器列表获取失败回调
    public Action ServerInfoError;
    /// <summary>
    /// 资源下载失败
    /// </summary>
    public Action<string> ItemError;
    public Action LoadOver;
    // 下载完成的资源列表
    public List<Patch> m_AlreadDownList = new List<Patch>();

    private DownloadAssetBundle m_CurDownload;

    // 是否开始下载
    private bool m_IsStartDownLoad = false;
    public bool IsStartDownLoad
    {
        get => m_IsStartDownLoad;
    }

    /// <summary>
    /// 尝试下载的次数
    /// </summary>
    private int m_TryDownCount = 0;

    private const int DOWNLOADCOUNT = 4;

    public int LoadFileCount { get; set; } = 0;
    /// <summary>
    /// 需要下载资源的总大小(kb)
    /// </summary>
    public float LoadSumSize { get; set; } = 0;

    /// <summary>
    /// 需要解压的文件
    /// </summary>
    private List<string> m_UnpackList = new List<string>();

    /// <summary>
    /// 原文件的MD5
    /// </summary>
    private Dictionary<string, ABMD5Base> m_PackedMd5 = new Dictionary<string, ABMD5Base>();

    /// <summary>
    /// 是否开始解压
    /// </summary>
    public bool IsStartUnPack = false;

    /// <summary>
    /// 解压文件总大小
    /// </summary>
    public float UnPackSumSize { get; set; } = 0;

    /// <summary>
    /// 已解压文件大小
    /// </summary>
    public float AlreadyUnPackSize { get; set; } = 0;

    public void Init(MonoBehaviour mono)
    {
        m_Mono = mono;
        // 只有在Android环境下才有可能复制文件
#if UNITY_ANDROID
        ReadLocalFileMd5();
#endif
    }

    /// <summary>
    /// 检查热更版本
    /// </summary>
    /// <param name="hotCallBack"></param>
    public void CheckVersion(Action<bool> hotCallBack = null)
    {
        m_TryDownCount = 0;
        m_HotFixDic.Clear();

        ReadVersion();
        m_Mono.StartCoroutine(ReadXml(() => 
        {
            if (m_ServerInfo == null)
            {
                if (ServerInfoError != null)
                {
                    ServerInfoError();
                }
                return;
            }

            foreach (var item in m_ServerInfo.GameVersion)
            {
                if (item.Version == m_CurVersion)
                {
                    m_GameVersion = item;
                    break;
                }
            }

            GetHotAB();
            if (CheckLocalAndServerPatch())
            {
                ComputeDownload();
                /// 后续要更改到下载完资源后处理以下逻辑
                if (File.Exists(m_ServerXmlPath))
                {
                    if (File.Exists(m_LocalServerXmlPath))
                    {
                        File.Delete(m_LocalServerXmlPath);
                    }
                    File.Move(m_ServerXmlPath, m_LocalServerXmlPath);
                }
            }

            // 计算资源大小
            LoadFileCount = m_DownloadList.Count;
            LoadSumSize = m_DownloadList.Sum(x => x.Size);

            if (hotCallBack != null)
            {
                hotCallBack(m_DownloadList.Count > 0);
            }
        }));

    }

    /// <summary>
    /// 检查本地热更信息与服务器信息是否一致
    /// </summary>
    /// <returns></returns>
    private bool CheckLocalAndServerPatch()
    {
        if (!File.Exists(m_LocalServerXmlPath))
        {
            return true;
        }

        m_LocalServerInfo = BinarySerializeOption.XmlDeserializeInFile(m_LocalServerXmlPath, typeof(ServerInfo)) as ServerInfo;

        VersionInfo localGameVesion = null;
        if(m_LocalServerInfo != null)
        {
            foreach (var item in m_LocalServerInfo.GameVersion)
            {
                if (item.Version == m_CurVersion)
                {
                    localGameVesion = item;
                    break;
                }
            }
        }

        if (localGameVesion != null && m_GameVersion.Patches != null && m_GameVersion.Patches.Length > 0 && localGameVesion.Patches.Length > 0 && m_GameVersion.Patches[m_GameVersion.Patches.Length - 1] != localGameVesion.Patches[localGameVesion.Patches.Length - 1])
        {
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// 计算需要解压的文件
    /// </summary>
    /// <returns></returns>
    public bool ComputeUnPackPath()
    {
#if !UNITY_ANDROID
        return false;

#else
        // 判断是否需要解压
        if (!ResourceManager.Instance.ABCopyToPersistent)
        {
            return false;
        }
        if (!Directory.Exists(AssetBundleManager.Instance.m_ABLoadPath))
        {
            Directory.CreateDirectory(AssetBundleManager.Instance.m_ABLoadPath);
        }
        m_UnpackList.Clear();
        foreach (var item in m_PackedMd5.Keys)
        {
            string filePath = m_UnpackList + "/" + item;
            if (File.Exists(filePath))
            {
                string md5 = MD5Manager.Instance.BuildFileMd5(filePath);
                if (md5 != m_PackedMd5[item].Md5)
                {
                    m_UnpackList.Add(item);
                }
            }
            else
            {
                m_UnpackList.Add(item);
            }
        }

        UnPackSumSize = 0;
        foreach (var item in m_UnpackList)
        {
            if (m_PackedMd5.ContainsKey(item))
            {
                UnPackSumSize += m_PackedMd5[item].Size;
            }
        }
        return m_UnpackList.Count > 0;
#endif
    }

    /// <summary>
    /// 获取解压进度
    /// </summary>
    /// <returns></returns>
    public float GetUnpackProgress()
    {
        return AlreadyUnPackSize / UnPackSumSize;
    }

    /// <summary>
    /// 开始解压
    /// </summary>
    /// <param name="cb"></param>
    public void StartUnpackFile(Action cb)
    {
        IsStartUnPack = true;
        m_Mono.StartCoroutine(UnPackToPersistentDataPath(cb));
    }

    IEnumerator UnPackToPersistentDataPath(Action cb)
    {
        foreach (var item in m_UnpackList)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(AssetBundleManager.Instance.m_ABRootPath + "/" + item);
            unityWebRequest.timeout = 30;
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("解压出错:" + unityWebRequest.error);
            }
            else
            {
                byte[] bytes = unityWebRequest.downloadHandler.data;
                FileTool.CreateFile(AssetBundleManager.Instance.m_ABLoadPath + "/" + item, bytes);
            }

            if (m_PackedMd5.ContainsKey(item))
            {
                AlreadyUnPackSize += m_PackedMd5[item].Size;
            }
            unityWebRequest.Dispose();
        }
        if (cb !=null)
        {
            cb();
        }
        IsStartUnPack = false;
    }

    /// <summary>
    /// 读取本地文件的MD5
    /// </summary>
    private void ReadLocalFileMd5()
    {
        m_PackedMd5.Clear();

        TextAsset md5 = Resources.Load<TextAsset>("ABMD5");
        if (md5 == null)
        {
            Debug.LogError("未读取到本地MD5数据");
            return;
        }
        using(MemoryStream stream = new MemoryStream(md5.bytes))
        {
            BinaryFormatter bf = new BinaryFormatter();
            ABMD5 aBMD5 = bf.Deserialize(stream) as ABMD5;
            foreach (var item in aBMD5.ABMD5List)
            {
                m_PackedMd5.Add(item.Name, item);
            }
        }
    }

    /// <summary>
    /// 读取当前打包版本
    /// </summary>
    private void ReadVersion()
    {
        var versionText = Resources.Load<TextAsset>("Version");
        if (versionText == null)
        {
            Debug.LogError("当前不存在本地版本文件!");
        }
        var allStr = versionText.text.Split('\n');
        if (allStr.Length > 0)
        {
            string[] infos = allStr[0].Split(';');
            if (infos.Length > 2)
            {
                m_CurVersion = infos[0].Split('|')[1];
                m_CurPackName = infos[1].Split('|')[1];
            }
        }
    }

    IEnumerator ReadXml(Action callBack)
    {
        string xmlUrl = ServerPath + "ServerInfo.xml";
        UnityWebRequest webRequest = UnityWebRequest.Get(xmlUrl);
        webRequest.timeout = 30;
        yield return webRequest.SendWebRequest();
        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Download Error:" + webRequest.error);
        }
        else
        {
            FileTool.CreateFile(m_ServerXmlPath, webRequest.downloadHandler.data);
            if (File.Exists(m_ServerXmlPath))
            {
                m_ServerInfo = BinarySerializeOption.XmlDeserializeInFile(m_ServerXmlPath, typeof(ServerInfo)) as ServerInfo;
            }
            else
            {
                Debug.Log("Download Error");
            }
        }

        callBack?.Invoke();
    }

    /// <summary>
    /// 获取下载的总进度
    /// </summary>
    /// <returns></returns>
    public float GetProgress()
    {
        return GetLoadSize() / LoadSumSize;
    }

    /// <summary>
    /// 获取已下载总大小
    /// </summary>
    /// <returns></returns>
    public float GetLoadSize()
    {
        float alreadySize = m_AlreadDownList.Sum(x => x.Size);
        float curAlreadySize = 0;
        if (m_CurDownload != null)
        {
            Patch patch = FindPatchByGamePath(m_CurDownload.FileName);
            if (patch != null && !m_AlreadDownList.Contains(patch))
            {
                curAlreadySize = m_CurDownload.GetProcess() * patch.Size;
            }
        }
        return alreadySize + curAlreadySize;
    }

    public string ComputeABPath(string name)
    {
        Patch patch = null;
        m_HotFixDic.TryGetValue(name, out patch);
        if (patch != null)
        {
            return m_DownloadPath + "/" + name;
        }
        return "";
    }

    private void GetHotAB()
    {
        if (m_GameVersion!=null && m_GameVersion.Patches != null)
        {
            Patches lastPatches = m_GameVersion.Patches[m_GameVersion.Patches.Length - 1];
            if (lastPatches != null && lastPatches.Files != null)
            {
                foreach (var item in lastPatches.Files)
                {
                    m_HotFixDic.Add(item.Name, item);
                }
            }
        }
    }


    /// <summary>
    /// 计算要下载的资源
    /// </summary>
    private void ComputeDownload()
    {
        m_DownloadDic.Clear();
        m_DownloadList.Clear();
        m_DownloadMd5Dic.Clear();

        if (m_GameVersion != null && m_GameVersion.Patches !=null && m_GameVersion.Patches.Length > 0)
        {
            m_CurrentPatches = m_GameVersion.Patches[m_GameVersion.Patches.Length - 1];
            if (m_CurrentPatches.Files != null && m_CurrentPatches.Files.Count > 0)
            {
                foreach (var item in m_CurrentPatches.Files)
                {
                    if (item.Platform.Contains("StandaloneWindows64") && (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor))
                    {
                        AddDownloadList(item);
                    }
                    else if (item.Platform.Contains("Android") && (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.Android))
                    {
                        AddDownloadList(item);
                    }
                    else if (item.Platform.Contains("IOS") && (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.IPhonePlayer))
                    {
                        AddDownloadList(item);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 将需要更新的数据添加进列表
    /// </summary>
    /// <param name="item"></param>
    private void AddDownloadList(Patch item)
    {
        string filePath = m_DownloadPath + "/" + item.Name;
        if (File.Exists(filePath))
        {
            string md5 = MD5Manager.Instance.BuildFileMd5(filePath);
            if (item.Md5 != md5)
            {
                m_DownloadList.Add(item);
                m_DownloadDic.Add(item.Name, item);
                m_DownloadMd5Dic.Add(item.Name, item.Md5);
            }
        }
        else
        {
            m_DownloadList.Add(item);
            m_DownloadDic.Add(item.Name, item);
            m_DownloadMd5Dic.Add(item.Name, item.Md5);
        }
    }

    public IEnumerator StartDownloadAB(Action cb = null, List<Patch> allPatch = null)
    {
        m_AlreadDownList.Clear();
        m_IsStartDownLoad = true;

        if (allPatch == null)
        {
            allPatch = m_DownloadList;
        }

        if (!Directory.Exists(m_DownloadPath))
        {
            Directory.CreateDirectory(m_DownloadPath);
        }
        List<DownloadAssetBundle> downloadAssetBundles = new List<DownloadAssetBundle>();
        foreach (var item in allPatch)
        {
            downloadAssetBundles.Add(new DownloadAssetBundle(ServerPath + item.Url, m_DownloadPath));
        }

        // 开始下载
        foreach (var item in downloadAssetBundles)
        {
            m_CurDownload = item;
            yield return m_Mono.StartCoroutine(item.Download());
            Patch patch = FindPatchByGamePath(item.FileName);
            if (patch != null)
            {
                m_AlreadDownList.Add(patch);
            }
            item.Destroy();
        }

        // MD5校验
        VerifyMD5(downloadAssetBundles, cb);
    }

    private void VerifyMD5(List<DownloadAssetBundle> downloadAssetBundles, Action cb)
    {
        List<Patch> downloadList = new List<Patch>();
        foreach (var item in downloadAssetBundles)
        {
            string md5 = "";
            if (m_DownloadMd5Dic.TryGetValue(item.FileName, out md5))
            {
                if (MD5Manager.Instance.BuildFileMd5(item.SaveFilePath) != md5)
                {
                    Debug.Log("文件下载失败");
                    Patch patch = FindPatchByGamePath(item.FileName);
                    if (patch != null)
                    {
                        downloadList.Add(patch);
                    }
                }
            }
            
        }
        if (downloadList.Count <= 0)
        {
            m_DownloadMd5Dic.Clear();
            if (cb != null)
            {
                cb();
            }
            if (LoadOver != null)
            {
                LoadOver();
            }
            m_IsStartDownLoad = false;
        }
        else
        {
            if (m_TryDownCount > DOWNLOADCOUNT)
            {
                m_IsStartDownLoad = false;

                string allName = "";

                foreach (var item in downloadList)
                {
                    allName += item.Name + ";";
                }
                if (ItemError != null)
                {
                    ItemError(allName);
                }
                Debug.Log("资源重复下载失败,请检查资源");
            }
            else
            {
                m_TryDownCount++;
                m_DownloadMd5Dic.Clear();
                foreach (var item in downloadList)
                {
                    m_DownloadMd5Dic.Add(item.Name, item.Md5);
                }
                // 重新下载
                m_Mono.StartCoroutine(StartDownloadAB(cb, downloadList));
            }
            
        }
    }

    /// <summary>
    /// 根据名字查找对应Patch
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private Patch FindPatchByGamePath(string name)
    {
        Patch patch = null;
        m_DownloadDic.TryGetValue(name, out patch);
        return patch;
    }
}

public class FileTool
{
    public static void CreateFile(string filePath, byte[] bytes)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        FileInfo file = new FileInfo(filePath);
        Stream stream = file.Create();
        stream.Write(bytes, 0, bytes.Length);
        stream.Close();
        stream.Dispose();
    }
}