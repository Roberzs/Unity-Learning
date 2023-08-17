/****************************************************
	文件：BuildAppEditor.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;

public class BuildAppEditor
{
    public static string m_AppName = Application.productName.ToString();
    public static string m_AppVersion = Application.version.ToString();
    public static string m_BuildAndroidPath = Application.dataPath + "/../BuildTarget/Android/";
    public static string m_BuildIOSPath = Application.dataPath + "/../BuildTarget/IOS/";
    public static string m_BuildWindowsPath = Application.dataPath + "/../BuildTarget/Windows/";

    [MenuItem("ResLoadFrame/Build/标准包")]
    public static void Build()
    {
        BundleEditor.NormalBuild();

        SaveVersion(PlayerSettings.bundleVersion, PlayerSettings.applicationIdentifier);

        string abPath = Application.dataPath + "/../AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString() + "/";
        CopyDir(abPath, Application.streamingAssetsPath);

        string savePath = "";
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.iOS:
                savePath = $"{m_BuildIOSPath}{m_AppName}_{EditorUserBuildSettings.activeBuildTarget}_v{m_AppVersion}_{DateTime.Now:yyyy_MM_dd_HH_mm}";
                break;
            case BuildTarget.Android:
                savePath = $"{m_BuildAndroidPath}{m_AppName}_{EditorUserBuildSettings.activeBuildTarget}_v{m_AppVersion}_{DateTime.Now:yyyy_MM_dd_HH_mm}{(EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk")}";
                break;
            case BuildTarget.StandaloneWindows:
                savePath = $"{m_BuildWindowsPath}{m_AppName}_{EditorUserBuildSettings.activeBuildTarget}_v{m_AppVersion}_{DateTime.Now:yyyy_MM_dd_HH_mm}/{m_AppName}.exe";
                break;
            case BuildTarget.StandaloneWindows64:
                savePath = $"{m_BuildWindowsPath}{m_AppName}_{EditorUserBuildSettings.activeBuildTarget}_v{m_AppVersion}_{DateTime.Now:yyyy_MM_dd_HH_mm}/{m_AppName}.exe";
                break;
        }

        BuildPipeline.BuildPlayer(FindEnableEditorScenes(), savePath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);

        DeleteDir(Application.streamingAssetsPath);
    }

    [MenuItem("ResLoadFrame/TTT/标准包(不重新打AB包)")]
    public static void OldBuild()
    {
        string abPath = Application.dataPath + "/../AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString() + "/";
        CopyDir(abPath, Application.streamingAssetsPath);

        string savePath = "";
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.iOS:
                savePath = $"{m_BuildIOSPath}{m_AppName}_{EditorUserBuildSettings.activeBuildTarget}_v{m_AppVersion}_{DateTime.Now:yyyy_MM_dd_HH_mm}";
                break;
            case BuildTarget.Android:
                savePath = $"{m_BuildAndroidPath}{m_AppName}_{EditorUserBuildSettings.activeBuildTarget}_v{m_AppVersion}_{DateTime.Now:yyyy_MM_dd_HH_mm}{(EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk")}";
                break;
            case BuildTarget.StandaloneWindows:
                savePath = $"{m_BuildWindowsPath}{m_AppName}_{EditorUserBuildSettings.activeBuildTarget}_v{m_AppVersion}_{DateTime.Now:yyyy_MM_dd_HH_mm}/{m_AppName}.exe";
                break;
            case BuildTarget.StandaloneWindows64:
                savePath = $"{m_BuildWindowsPath}{m_AppName}_{EditorUserBuildSettings.activeBuildTarget}_v{m_AppVersion}_{DateTime.Now:yyyy_MM_dd_HH_mm}/{m_AppName}.exe";
                break;
        }

        BuildPipeline.BuildPlayer(FindEnableEditorScenes(), savePath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);

        DeleteDir(Application.streamingAssetsPath);
    }


	private static string[] FindEnableEditorScenes()
    {
		List<string> editorScenes = new List<string>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled)
                continue;

            editorScenes.Add(scene.path);
        }
        return editorScenes.ToArray();
    }

    private static void CopyDir(string srcPath, string targetPath)
    {
        try
        {
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            
            string srcDir = Path.Combine(targetPath, Path.GetFileName(srcPath));
            if (Directory.Exists(srcPath))
            {
                srcDir += Path.DirectorySeparatorChar;
            }
            if (!Directory.Exists(srcDir))
            {
                Directory.CreateDirectory(srcDir);
            }

            string[] files = Directory.GetFileSystemEntries(srcPath);
            foreach (var file in files)
            {
                if (Directory.Exists(file))
                {
                    CopyDir(file, srcDir);
                }
                else
                {
                    File.Copy(file, srcDir + Path.GetFileName(file), true);
                }
            }
        }
        catch
        {
            Debug.LogError("文件复制失败");
        }
    }

    public static void DeleteDir(string srcPath)
    {
        try
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileInfo = dir.GetFileSystemInfos();
            foreach (var info in fileInfo)
            {
                if (info is DirectoryInfo)
                {
                    DirectoryInfo subDir = new DirectoryInfo(info.FullName);
                    subDir.Delete(true);
                }
                else
                {
                    File.Delete(info.FullName);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }

    [MenuItem("ResLoadFrame/TTT/Save Version")]
    public static void TSaveVersion()
    {
        SaveVersion(PlayerSettings.bundleVersion, PlayerSettings.applicationIdentifier);
    }

    static void SaveVersion(string version, string packageName)
    {
        string path = Path.Combine(Application.dataPath, "Resources/Version.txt");

        string targetContent = "Version|" + version + ";PackageName|" + packageName + ";";
        string allContent = "";
        string firstLineContent = "";
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            using(StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8))
            {
                allContent = sr.ReadToEnd();
                firstLineContent = allContent.Split('\r')[0];
            }
        }
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
            {
                if (string.IsNullOrEmpty(allContent))
                {
                    allContent = targetContent;
                }
                else
                {
                    allContent.Replace(firstLineContent, targetContent);
                }
                sw.Write(allContent);
            }
        }
    }
}

