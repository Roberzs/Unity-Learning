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
    // �������б��ȡʧ�ܻص�
    public Action ServerInfoError;
    /// <summary>
    /// ��Դ����ʧ��
    /// </summary>
    public Action<string> ItemError;
    public Action LoadOver;
    // ������ɵ���Դ�б�
    public List<Patch> m_AlreadDownList = new List<Patch>();

    private DownloadAssetBundle m_CurDownload;

    // �Ƿ�ʼ����
    private bool m_IsStartDownLoad = false;
    public bool IsStartDownLoad
    {
        get => m_IsStartDownLoad;
    }

    /// <summary>
    /// �������صĴ���
    /// </summary>
    private int m_TryDownCount = 0;

    private const int DOWNLOADCOUNT = 4;

    public int LoadFileCount { get; set; } = 0;
    /// <summary>
    /// ��Ҫ������Դ���ܴ�С(kb)
    /// </summary>
    public float LoadSumSize { get; set; } = 0;

    /// <summary>
    /// ��Ҫ��ѹ���ļ�
    /// </summary>
    private List<string> m_UnpackList = new List<string>();

    /// <summary>
    /// ԭ�ļ���MD5
    /// </summary>
    private Dictionary<string, ABMD5Base> m_PackedMd5 = new Dictionary<string, ABMD5Base>();

    /// <summary>
    /// �Ƿ�ʼ��ѹ
    /// </summary>
    public bool IsStartUnPack = false;

    /// <summary>
    /// ��ѹ�ļ��ܴ�С
    /// </summary>
    public float UnPackSumSize { get; set; } = 0;

    /// <summary>
    /// �ѽ�ѹ�ļ���С
    /// </summary>
    public float AlreadyUnPackSize { get; set; } = 0;

    public void Init(MonoBehaviour mono)
    {
        m_Mono = mono;
        // ֻ����Android�����²��п��ܸ����ļ�
#if UNITY_ANDROID
        ReadLocalFileMd5();
#endif
    }

    /// <summary>
    /// ����ȸ��汾
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
                /// ����Ҫ���ĵ���������Դ���������߼�
                if (File.Exists(m_ServerXmlPath))
                {
                    if (File.Exists(m_LocalServerXmlPath))
                    {
                        File.Delete(m_LocalServerXmlPath);
                    }
                    File.Move(m_ServerXmlPath, m_LocalServerXmlPath);
                }
            }

            // ������Դ��С
            LoadFileCount = m_DownloadList.Count;
            LoadSumSize = m_DownloadList.Sum(x => x.Size);

            if (hotCallBack != null)
            {
                hotCallBack(m_DownloadList.Count > 0);
            }
        }));

    }

    /// <summary>
    /// ��鱾���ȸ���Ϣ���������Ϣ�Ƿ�һ��
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
    /// ������Ҫ��ѹ���ļ�
    /// </summary>
    /// <returns></returns>
    public bool ComputeUnPackPath()
    {
#if !UNITY_ANDROID
        return false;

#else
        // �ж��Ƿ���Ҫ��ѹ
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
    /// ��ȡ��ѹ����
    /// </summary>
    /// <returns></returns>
    public float GetUnpackProgress()
    {
        return AlreadyUnPackSize / UnPackSumSize;
    }

    /// <summary>
    /// ��ʼ��ѹ
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
                Debug.LogError("��ѹ����:" + unityWebRequest.error);
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
    /// ��ȡ�����ļ���MD5
    /// </summary>
    private void ReadLocalFileMd5()
    {
        m_PackedMd5.Clear();

        TextAsset md5 = Resources.Load<TextAsset>("ABMD5");
        if (md5 == null)
        {
            Debug.LogError("δ��ȡ������MD5����");
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
    /// ��ȡ��ǰ����汾
    /// </summary>
    private void ReadVersion()
    {
        var versionText = Resources.Load<TextAsset>("Version");
        if (versionText == null)
        {
            Debug.LogError("��ǰ�����ڱ��ذ汾�ļ�!");
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
    /// ��ȡ���ص��ܽ���
    /// </summary>
    /// <returns></returns>
    public float GetProgress()
    {
        return GetLoadSize() / LoadSumSize;
    }

    /// <summary>
    /// ��ȡ�������ܴ�С
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
    /// ����Ҫ���ص���Դ
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
    /// ����Ҫ���µ�������ӽ��б�
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

        // ��ʼ����
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

        // MD5У��
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
                    Debug.Log("�ļ�����ʧ��");
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
                Debug.Log("��Դ�ظ�����ʧ��,������Դ");
            }
            else
            {
                m_TryDownCount++;
                m_DownloadMd5Dic.Clear();
                foreach (var item in downloadList)
                {
                    m_DownloadMd5Dic.Add(item.Name, item.Md5);
                }
                // ��������
                m_Mono.StartCoroutine(StartDownloadAB(cb, downloadList));
            }
            
        }
    }

    /// <summary>
    /// �������ֲ��Ҷ�ӦPatch
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