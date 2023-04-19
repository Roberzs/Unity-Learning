using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class HotFixManager : Singleton<HotFixManager>
{
    private string m_CurVersion;
    private string m_CurPackName;

    private MonoBehaviour m_Mono;

    private string m_ServerXmlPath = Application.persistentDataPath + "/ServerInfo.xml";
    private string m_LocalServerXmlPath = Application.persistentDataPath + "/LocalServerInfo.xml";
    private string m_DownloadPath = Application.persistentDataPath + "/Download";

    private ServerInfo m_ServerInfo;
    private ServerInfo m_LocalServerInfo;
    private VersionInfo m_GameVersion;
    private Patches m_CurrentPatches;

    private Dictionary<string, Patch> m_HotFixDic = new Dictionary<string, Patch>();
    private List<Patch> m_DownloadList = new List<Patch>();
    private Dictionary<string, Patch> m_DownloadDic = new Dictionary<string, Patch>();

    public int LoadFileCount { get; set; } = 0;
    /// <summary>
    /// 需要下载资源的总大小(kb)
    /// </summary>
    public float LoadSumSize { get; set; } = 0;

    public void Init(MonoBehaviour mono)
    {
        m_Mono = mono;
    }

    // 检查热更版本
    public void CheckVersion(Action<bool> hotCallBack = null)
    {
        m_HotFixDic.Clear();

        ReadVersion();
        m_Mono.StartCoroutine(ReadXml(() => 
        {
            if (m_ServerInfo == null)
            {
                if (hotCallBack != null)
                {
                    hotCallBack(false);
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
    /// 检查本地资源是否与服务器下载列表信息一致
    /// </summary>
    private void CheckLocalResource()
    {
        m_DownloadList.Clear();
        m_DownloadDic.Clear();


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
        string xmlUrl = "http://127.0.0.1/ServerInfo.xml";
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
        if(m_GameVersion != null && m_GameVersion.Patches !=null && m_GameVersion.Patches.Length > 0)
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
            }
        }
        else
        {
            m_DownloadList.Add(item);
            m_DownloadDic.Add(item.Name, item);
        }
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