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
using System;
using System.Linq;
using System.Collections.Generic;

//[ExecuteInEditMode] // 脚本可在编辑模式下运行
public static class CreateComponentCodeEditor 
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

        var lists = new List<BindInfo>();
        SearchBinds("", obj.transform, lists);

        var scriptsFolder = Application.dataPath + "/Scripts";
        if (!Directory.Exists(scriptsFolder)) Directory.CreateDirectory(scriptsFolder);

        WriteTemplateCode(obj.name, scriptsFolder);
        WriteDesignerTemplateCode(obj.name, scriptsFolder, lists);

        EditorPrefs.SetString("SCRIPT_NAME", obj.name);

        AssetDatabase.Refresh();    // 刷新Assets文件夹
    }

    // 脚本完成编译后的回调
    [DidReloadScripts]
    static void CreateComponentScriptCb()
    {
        string scriptName = EditorPrefs.GetString("SCRIPT_NAME");
        EditorPrefs.DeleteKey("SCRIPT_NAME");

        if (!string.IsNullOrWhiteSpace(scriptName))
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var defaultAssembly = assemblies.First(assembly => assembly.GetName().Name == "Assembly-CSharp");
            //var type = "命名空间." + defaultAssembly.GetType(scriptName);
            var type = defaultAssembly.GetType(scriptName);
            var gameObject = GameObject.Find(scriptName);

            var scriptComponent = gameObject.GetComponent(type);
            if (!scriptComponent)
            {
                scriptComponent = gameObject.AddComponent(type);
            }
            

            var lists = new List<BindInfo>();
            SearchBinds("", gameObject.transform, lists);

            var serializedScript = new SerializedObject(scriptComponent);
            foreach (var item in lists)
            {
                var name = item.FindPath.Split('/').Last();
                serializedScript.FindProperty(name).objectReferenceValue = gameObject.transform.Find(item.FindPath).gameObject;
            }
            serializedScript.ApplyModifiedPropertiesWithoutUndo();
        }
        
    }

    static void WriteTemplateCode(string name, string scriptsFolder)
    {
        var scriptFile = scriptsFolder + $"/{name}.cs";

        if (File.Exists(scriptFile)) return;

        var stream = File.CreateText(scriptFile);

        stream.WriteLine("using UnityEngine;");
        stream.WriteLine();
        stream.WriteLine($"public partial class {name} : MonoBehaviour");
        stream.WriteLine("{");
        stream.WriteLine();
        stream.WriteLine("}");

        stream.Close();
    }

    static void WriteDesignerTemplateCode(string name, string scriptsFolder, List<BindInfo> binds) 
    {
        var scriptFile = scriptsFolder + $"/{name}.Designer.cs";
        var stream = File.CreateText(scriptFile);

        stream.WriteLine("using UnityEngine;");
        stream.WriteLine();
        stream.WriteLine($"public partial class {name} : MonoBehaviour");
        stream.WriteLine("{");
        foreach (var item in binds)
        {
            stream.WriteLine($"\t public GameObject {item.FindPath.Split('/').Last()};");
        }
        stream.WriteLine();
        stream.WriteLine("}");

        stream.Close();
    }

    static void SearchBinds(string path, Transform transform, List<BindInfo> binds)
    {
        var bind = transform.GetComponent<Bind>();
        var isRoot = string.IsNullOrWhiteSpace(path);
        if (bind && !isRoot)
        {
            binds.Add(new BindInfo() { FindPath = path });
        }
        foreach (Transform item in transform)
        {
            SearchBinds(isRoot ? item.name : (path + "/" + item.name), item, binds);
        }
    }
}
