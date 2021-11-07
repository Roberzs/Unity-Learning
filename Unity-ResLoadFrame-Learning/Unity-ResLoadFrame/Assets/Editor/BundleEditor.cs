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
using System.Threading;
using System.IO;

public class BundleEditor
{
    public static string M_ABCONFIG_PATH = "Assets/Editor/ABConfig.asset";
    public static string M_BUNDLETARGET_PATH = Application.streamingAssetsPath;

    /// <summary>
    /// 存储所有资源类AB包的路径 key:包名 value:路径
    /// </summary>
    public static Dictionary<string, string> m_AllFileDir = new Dictionary<string, string>();
    public static Dictionary<string, List<string>> m_AllPrefabDir = new Dictionary<string, List<string>>();
    public static List<string> m_AllFileAB = new List<string>();

    [MenuItem("Tools/打包")]
    private static void Build()
    {
        /**
        // BuildPipeline.BuildAssetBundles(路径, 压缩格式, 打包平台);
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        // 刷新编辑器, 使刚刚打包的AB包资源立即导入
        AssetDatabase.Refresh();
        */

        m_AllFileDir.Clear();
        m_AllPrefabDir.Clear();
        m_AllFileAB.Clear();

        ABConfig abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(M_ABCONFIG_PATH);
        foreach (ABConfig.FileDirABName fileDir in abConfig.m_AllFileDirAB)
        {
            if (!m_AllFileDir.ContainsKey(fileDir.ABName))
            {
                m_AllFileDir.Add(fileDir.ABName, fileDir.Path);
                m_AllFileAB.Add(fileDir.Path);
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
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
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
            if(path == m_AllFileAB[i] || path.Contains(m_AllFileAB[i]))
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
                if (allBundlePath[j].EndsWith(".cs"))
                    continue;
                //Debug.Log("AB包:" + allBundles[i] + "下所包含的文件路径:" + allBundlePath[j]);
                resPathDic.Add(allBundlePath[j], allBundles[i]);
            }
        }
        // 删除无用AB包
        DeleteUselessAB();
        // 生成配置表

        // 打包
        BuildPipeline.BuildAssetBundles(M_BUNDLETARGET_PATH, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
    }

    public static void DeleteUselessAB()
    {
        string[] allBundleName = AssetDatabase.GetAllAssetBundleNames();
        DirectoryInfo direction = new DirectoryInfo(M_BUNDLETARGET_PATH);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (!(ConatinABName(files[i].Name, allBundleName) || files[i].Name.EndsWith(".meta")))
            {
                if (File.Exists(files[i].FullName)){
                    File.Delete(files[i].FullName);
                }
            }
            else
            {
                continue;
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
}
