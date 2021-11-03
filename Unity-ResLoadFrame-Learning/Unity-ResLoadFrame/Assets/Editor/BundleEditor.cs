/****************************************************
    文件：BundleEditor.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEditor;

public class BundleEditor
{
    [MenuItem("Tools/打包AssetBundle")]
    private static void Build()
    {
        // BuildPipeline.BuildAssetBundles(路径, 压缩格式, 打包平台);
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        // 刷新编辑器, 使刚刚打包的AB包资源立即导入
        AssetDatabase.Refresh();
    }
}
