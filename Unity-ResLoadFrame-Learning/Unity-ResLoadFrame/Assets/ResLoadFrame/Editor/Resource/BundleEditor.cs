/****************************************************
    文件：BundleEditor.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

public class BundleEditor
{
    private static string M_ABCONFIG_PATH = GetScriptInDirectory(MethodBase.GetCurrentMethod().DeclaringType.Name) + "/ABConfig.asset";
    private static string M_BUNDLETARGET_PATH = Application.dataPath + "/../AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString();
    private static string M_ABBYTEPATH = GetScriptInDirectory(MethodBase.GetCurrentMethod().DeclaringType.Name).Replace("ResLoadFrame/Editor/Resource", "ResLoadFrame/Temp/AssetBundleConfig.bytes");
    private static string M_VERSIONMD5PATH = Application.dataPath + "/../Version/" + EditorUserBuildSettings.activeBuildTarget.ToString();
    private static string M_HOTPATH = Application.dataPath + "/../Hot/" + EditorUserBuildSettings.activeBuildTarget.ToString();

    /// <summary>
    /// 存储所有资源类AB包的路径 key:包名 value:路径
    /// </summary>
    private static Dictionary<string, string> m_AllFileDir = new Dictionary<string, string>();
    private static Dictionary<string, List<string>> m_AllPrefabDir = new Dictionary<string, List<string>>();
    private static List<string> m_AllFileAB = new List<string>();
    private static List<string> m_ConfigFile = new List<string>();

    private static Dictionary<string, ABMD5Base> m_PackedMd5 = new Dictionary<string, ABMD5Base>();

    [MenuItem("ResLoadFrame/打包AssetBundle")]
    public static void NormalBuild()
    {
        Build();
    }

    public static void Build(bool bHotFix = false, string filePath = "", string hotfixCount = "1")
    {

        /**
        // BuildPipeline.BuildAssetBundles(路径, 压缩格式, 打包平台);
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        // 刷新编辑器, 使刚刚打包的AB包资源立即导入
        AssetDatabase.Refresh();
        */

        DataEditor.AllXmlToBinary();

        m_AllFileDir.Clear();
        m_AllPrefabDir.Clear();
        m_AllFileAB.Clear();
        m_ConfigFile.Clear();
        // 添加依赖关系配置表
        m_AllFileDir.Add("assetbundleconfig", M_ABCONFIG_PATH);
        m_AllFileAB.Add(M_ABCONFIG_PATH);
        m_ConfigFile.Add(M_ABCONFIG_PATH);

        ABConfig abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(M_ABCONFIG_PATH);
        foreach (ABConfig.FileDirABName fileDir in abConfig.m_AllFileDirAB)
        {
            if (!m_AllFileDir.ContainsKey(fileDir.ABName))
            {
                m_AllFileDir.Add(fileDir.ABName, fileDir.Path);
                m_AllFileAB.Add(fileDir.Path);
                m_ConfigFile.Add(fileDir.Path);
            }
            else
            {
                Debug.LogError("AB包名配置重复,请检查. 重复包名:" + fileDir.ABName);
            }
        }


        // 数组获取到的是文件的GUID
        string[] allStr = AssetDatabase.FindAssets("t:Prefab", abConfig.m_AllPrefabPath.ToArray());
        for(int i = 0; i < allStr.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(allStr[i]);
            EditorUtility.DisplayProgressBar("查找Prefab", "Prefab:" + path, i * 1.0f / allStr.Length);
            m_ConfigFile.Add(path);
            if (!ContainAllFileAB(path))
            {
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                string[] allDepend = AssetDatabase.GetDependencies(path);
                List<string> allDependPath = new List<string>();
                for (int j = 0; j < allDepend.Length; j++)
                {
                    if (!ContainAllFileAB(allDepend[j]) && !allDepend[j].EndsWith(".cs"))
                    {
                        allDependPath.Add(allDepend[j]);
                        m_AllFileAB.Add(allDepend[j]);
                    }
                }
                if (!m_AllPrefabDir.ContainsKey(obj.name))
                {
                    m_AllPrefabDir.Add(obj.name, allDependPath);
                }
                else
                {
                    Debug.LogError("AB包存在相同Prefab,请检查. 重复Prefab:" + obj.name);
                }
                
            }
        }

        foreach(string name in m_AllFileDir.Keys)
        {
            SetABName(name, m_AllFileDir[name]);
        }
        foreach (string name in m_AllPrefabDir.Keys)
        {
            SetABName(name, m_AllPrefabDir[name]);
        }

        BuildAssetBundle();

        string[] oldABNames = AssetDatabase.GetAllAssetBundleNames();
        for(int i = 0; i< oldABNames.Length; i++)
        {
            AssetDatabase.RemoveAssetBundleName(oldABNames[i], true);
            EditorUtility.DisplayProgressBar("清除AB包名", "包名:" + oldABNames[i], i * 1.0f / oldABNames.Length);
        }

        if (bHotFix)
        {
            ReadABMd5Com(filePath, hotfixCount);
        }
        else
        {
            WriteABMD5();
        }

        

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }

    static void WriteABMD5()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(M_BUNDLETARGET_PATH);
        FileInfo[] fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
        ABMD5 abMd5 = new ABMD5();
        abMd5.ABMD5List = new List<ABMD5Base>();
        foreach (var item in fileInfos)
        {
            if (!item.Name.EndsWith(".meta") && !item.Name.EndsWith(".manifest"))
            {
                var abMd5Base = new ABMD5Base();
                
                abMd5Base.Name = item.Name;
                abMd5Base.Md5 = MD5Manager.Instance.BuildFileMd5(item.FullName);
                abMd5Base.Size = item.Length / 1024.0f;

                abMd5.ABMD5List.Add(abMd5Base);
            }
        }

        string path = Path.Combine(Application.dataPath, "Resources/ABMD5.bytes");
        BinarySerializeOption.BinarySerilize(path, abMd5);

        // 拷贝到外部
        if (!Directory.Exists(M_VERSIONMD5PATH))
        {
            Directory.CreateDirectory(M_VERSIONMD5PATH);
        }
        var targetPath = M_VERSIONMD5PATH + "/ABMD5_" + PlayerSettings.bundleVersion + ".bytes";
        if (File.Exists(targetPath))
        {
            File.Delete(targetPath);
        }
        File.Copy(path, targetPath);
    }

    private static void ReadABMd5Com(string filePath, string hotfixCount)
    {
        m_PackedMd5.Clear();

        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            var bf = new BinaryFormatter();
            var abMd5 = bf.Deserialize(fs) as ABMD5;
            foreach (var item in abMd5.ABMD5List)
            {
                m_PackedMd5.Add(item.Name, item);
            }
        }
        var changeList = new List<string>();
        DirectoryInfo directoryInfo = new DirectoryInfo(M_BUNDLETARGET_PATH);
        var files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
        foreach (var item in files)
        {
            var name = item.Name;
            if (!name.EndsWith(".meta") && !name.EndsWith(".manifest"))
            {
                var md5 = MD5Manager.Instance.BuildFileMd5(item.FullName);
                if (!m_PackedMd5.ContainsKey(name))
                {
                    changeList.Add(name);
                }
                else 
                {
                    if (m_PackedMd5[name].Md5 != md5)
                    {
                        changeList.Add(name);
                    }
                }
            }
        }

        CopyABAndGeneratXml(changeList, hotfixCount);
    }

    private static void CopyABAndGeneratXml(List<string> changeList, string hotFixCount)
    {
        if (!Directory.Exists(M_HOTPATH))
        {
            Directory.CreateDirectory(M_HOTPATH);
        }
        DeleteAllFile(M_HOTPATH);
        foreach (var item in changeList)
        {
            if (!item.EndsWith(".manifest"))
            {
                File.Copy(M_BUNDLETARGET_PATH + "/" + item, M_HOTPATH + "/" + item);
            }
        }

        // 生成服务器Patch
        DirectoryInfo directory = new DirectoryInfo(M_HOTPATH);
        var files = directory.GetFiles("*", SearchOption.AllDirectories);
        Patches patches = new Patches();
        patches.Version = 1;
        patches.Files = new List<Patch>();
        foreach (var item in files)
        {
            Patch patch = new Patch();
            patch.Md5 = MD5Manager.Instance.BuildFileMd5(item.FullName);
            patch.Name = item.Name;
            patch.Size = item.Length / 1024.0f;
            patch.Platform = EditorUserBuildSettings.activeBuildTarget.ToString();
            patch.Url = "http://127.0.0.1/AssetBundle/" + PlayerSettings.bundleVersion + "/" + hotFixCount + "/" + item.Name;
            patches.Files.Add(patch);
        }
        BinarySerializeOption.XmlSerialize(M_HOTPATH + "/Patch.xml", patches);
    }

    static void SetABName(string name, string path)
    {
        AssetImporter assetImporter = AssetImporter.GetAtPath(path);
        if (assetImporter != null)
        {
            assetImporter.assetBundleName = name;
        }
        else
        {
            Debug.Log("不存在此路径文件:" + path);
        }
    }

    static void SetABName(string name, List<string> paths)
    {
        for (int i =0; i< paths.Count; i++)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(paths[i]);
            if (assetImporter != null)
            {
                assetImporter.assetBundleName = name;
            }
            else
            {
                Debug.Log("不存在此路径文件:" + paths[i]);
            }
        }
        
    }

    private static bool ContainAllFileAB(string path)
    {
        for(int i = 0; i < m_AllFileAB.Count; i++)
        {
            if(path == m_AllFileAB[i] || (path.Contains(m_AllFileAB[i]) && path.Replace(m_AllFileAB[i], "")[0] == '/'))
            {
                return true;
            }
        }
        return false;
    }

    private static void BuildAssetBundle()
    {
        string[] allBundles = AssetDatabase.GetAllAssetBundleNames();
        // key: 全路径  value: 包名
        Dictionary<string, string> resPathDic = new Dictionary<string, string>();
        for (int i = 0; i< allBundles.Length; i++)
        {
            string[] allBundlePath = AssetDatabase.GetAssetPathsFromAssetBundle(allBundles[i]);
            for (int j = 0; j < allBundlePath.Length; j++)
            {
                if (allBundlePath[j].EndsWith(".cs") || !ValidPath(allBundlePath[j]))
                    continue;
                //Debug.Log("AB包:" + allBundles[i] + "下所包含的文件路径:" + allBundlePath[j]);
                
                resPathDic.Add(allBundlePath[j], allBundles[i]);
            }
        }
        // 删除无用AB包
        DeleteUselessAB();
        // 生成配置表
        WriteDataConfig(resPathDic);
        // 打包
        if (!Directory.Exists(M_BUNDLETARGET_PATH))
        {
            Directory.CreateDirectory(M_BUNDLETARGET_PATH);
        }

        AssetBundleManifest manifes = BuildPipeline.BuildAssetBundles(M_BUNDLETARGET_PATH, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        if (manifes)
        {
            Debug.Log("打包完成");
        }
        else
        {
            Debug.LogError("AB包打包失败");
        }
    }

    /// <summary>
    /// 生成配置表
    /// </summary>
    /// <param name="resPathDic"></param>
    private static void WriteDataConfig(Dictionary<string,string> resPathDic)
    {
        AssetBundleConfig config = new AssetBundleConfig();
        config.ABList = new List<ABBase>();
        foreach (string path in resPathDic.Keys)
        {
            if (!ValidPath(path))
                continue;

            ABBase abBase = new ABBase();
            abBase.Path = path;
            abBase.Crc = CRC32.GetCRC32(path);
            //Debug.Log($"将要写入的资源路径:{path} Crc:{CRC32.GetCRC32(path)}");
            abBase.ABName = resPathDic[path];
            abBase.AssetName = path.Remove(0, path.LastIndexOf("/") + 1);
            abBase.ABDependance = new List<string>();
            string[] resDependance = AssetDatabase.GetDependencies(path);
            for (int i = 0; i < resDependance.Length; i++)
            {
                string tmpPath = resDependance[i];
                if (tmpPath == path || tmpPath.EndsWith(".cs"))
                    continue;
                string abName = "";
                if (resPathDic.TryGetValue(tmpPath, out abName))
                {
                    if (abName == resPathDic[path])
                        continue;
                    if (!abBase.ABDependance.Contains(abName))
                    {
                        abBase.ABDependance.Add(abName);
                    }
                }
            }
            config.ABList.Add(abBase);
        }


        // 写入xml文件(方便查看)
        string xmlPath = Application.dataPath + "/AssetBundleConfig.xml";
        if (File.Exists(xmlPath))
            File.Delete(xmlPath);
        FileStream xmlFileStream = new FileStream(xmlPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(xmlFileStream, System.Text.Encoding.UTF8);
        XmlSerializer xml = new XmlSerializer(config.GetType());
        xml.Serialize(sw, config);
        sw.Close();
        xmlFileStream.Close();
        // 写入二进制文件
        // 二进制文件用于AB包的加载，只需要包名、资源名、依赖即可, 并不需要原始路径. 所以此处清空Path以减小二进制文件大小
        foreach (ABBase item in config.ABList)
        {
            item.Path = "";
        }

        string abConfigDirectory = M_ABBYTEPATH.Remove(M_ABBYTEPATH.LastIndexOf('/'));
        if (!Directory.Exists(abConfigDirectory))
        {
            Directory.CreateDirectory(abConfigDirectory);
        }

        FileStream binaryFileStream = new FileStream(M_ABBYTEPATH, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        binaryFileStream.Seek(0, SeekOrigin.Begin);
        binaryFileStream.SetLength(0);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(binaryFileStream, config);
        binaryFileStream.Close();
        AssetDatabase.Refresh();
        SetABName("assetbundleconfig", M_ABBYTEPATH);
        Debug.Log("配置表生成成功!");
        
    }

    /// <summary>
    /// 删除指定文件目录下的所有文件
    /// </summary>
    /// <param name="fullPath"></param>
    private static void DeleteAllFile(string fullPath)
    {
        if (Directory.Exists(fullPath))
        {
            var directory = new DirectoryInfo(fullPath);
            FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
            foreach (var item in files)
            {
                if (item.Name.EndsWith(".meta"))
                {
                    continue;
                }
                File.Delete(item.FullName);
            }
        }
    }

    /// <summary>
    /// 删除无用AB包
    /// </summary>
    private static void DeleteUselessAB()
    {
        if (!Directory.Exists(M_BUNDLETARGET_PATH))
            return;
        string[] allBundleName = AssetDatabase.GetAllAssetBundleNames();
        DirectoryInfo direction = new DirectoryInfo(M_BUNDLETARGET_PATH);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if ((ConatinABName(files[i].Name, allBundleName) 
                || files[i].Name.EndsWith(".meta"))
                || files[i].Name.EndsWith(".manifest")
                || files[i].Name.EndsWith("AssetBundleConfig"))
            {
                continue;
            }
            else
            {
                Debug.Log($"该AB包被删除或重命名了:{files[i].Name}");
                if (File.Exists(files[i].FullName))
                {
                    File.Delete(files[i].FullName);
                }
                if (File.Exists(files[i].FullName + ".manifest"))
                {
                    File.Delete(files[i].FullName + ".manifest");
                }
            }
        }
    }

    private static bool ConatinABName(string name, string[] names)
    {
        for (int i = 0; i < names.Length; i++)
        {
            if (name == names[i])
                return true;
        }
        return false;
    }

    private static bool ValidPath(string path)
    {
        for (int i = 0; i < m_ConfigFile.Count; i++)
        {
            if (path.Contains(m_ConfigFile[i]))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 根据脚本名称获取所在目录
    /// </summary>
    /// <param name="scriptName"></param>
    /// <returns></returns>
    public static string GetScriptInDirectory(string scriptName)
    {
        string[] path = UnityEditor.AssetDatabase.FindAssets(scriptName);
        foreach (var item in path)
        {
            string tmpPtah = AssetDatabase.GUIDToAssetPath(item);
            if (tmpPtah.EndsWith(".cs"))
            {
                return tmpPtah.Replace((@"/" + scriptName + ".cs"), "");
            }
        }
        return string.Empty;


    }

    [MenuItem("ResLoadFrame/打开AssetBundle配置文件", priority = 10)]
    private static void OpenABConfigAsset()
    {

        Selection.activeObject = AssetDatabase.LoadAssetAtPath<ABConfig>(M_ABCONFIG_PATH);
    }
}
