/****************************************************
    文件：TestEditor.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEditor;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
using System.IO;

public class TestEditor : EditorWindow
{
    // 快捷键 # - Shift, & - Alt, % - Ctrl 如果只按一个按键 可在按键前 +_, 例如 _s,  表示按s触发
    [MenuItem("编辑器扩展/Test/LoadCube %&#x", priority = 0)]
    static void LoadCube()
    {
        
        GameObject tmpObj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Prefabs/Cube.prefab"); ;
        PrefabUtility.InstantiatePrefab(tmpObj);
        Debug.Log("加载成功");
    }

    [MenuItem("编辑器扩展/Test/SaveCube", priority = 1)]
    static void SaveCube()
    {
        GameObject obj = GameObject.Find("Cube");
        if (!obj)
        {
            Debug.Log("当前场景不存在Cube");
        }
        PrefabUtility.SaveAsPrefabAssetAndConnect(obj, Application.dataPath + "/Resources/Prefabs/Cube.prefab",InteractionMode.AutomatedAction, out bool success);
        Debug.Log(success ? "保存成功" : "保存失败");
    }
}
