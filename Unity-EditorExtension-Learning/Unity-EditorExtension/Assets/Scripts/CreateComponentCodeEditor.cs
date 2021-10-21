/****************************************************
    文件：CreateComponentCodeEditor.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class CreateComponentCodeEditor : MonoBehaviour
{
    // Hierarchy 下的物体右键点击只显示GameObject菜单层级较高的内容
    [MenuItem("GameObject/@EditorExtension-CreateCode", priority = 0)]
    static void CreateCode()
    {
        GameObject obj = Selection.activeObject as GameObject;
        if (!obj)
        {
            Debug.Log("当前未选中GameObject物体");
            return;
        }

        var scriptsFolder = Application.dataPath + "/Scripts";
        if (!Directory.Exists(scriptsFolder)) Directory.CreateDirectory(scriptsFolder);

        var scriptFile = scriptsFolder + $"/{obj.name}.cs";
        var stream = File.CreateText(scriptFile);
        WriteTemplateCode(stream, obj.name);
        stream.Close();
        
        AssetDatabase.Refresh();    // 刷新Assets文件夹
    }

    // 脚本完成编译后的回调
    [DidReloadScripts]
    static void CreateComponentScriptCb()
    {

    }

    static void WriteTemplateCode(StreamWriter stream, string fileName) 
    {
        stream.WriteLine("using UnityEngine;");
        stream.WriteLine();
        stream.WriteLine($"public class {fileName} : MonoBehaviour");
        stream.WriteLine("{");
        stream.WriteLine();
        stream.WriteLine("}");
    }
}
