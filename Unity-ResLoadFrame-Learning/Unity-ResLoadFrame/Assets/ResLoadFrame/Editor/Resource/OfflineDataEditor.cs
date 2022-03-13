/****************************************************
	文件：OfflineDataEditor.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEditor;

public class OfflineDataEditor
{
	[MenuItem("Assets/ResLoadFrame/离线数据/生成Prefab 离线数据")]
	public static void AssetCreateOfflineData()
    {
		GameObject[] objects = Selection.gameObjects;
		for(int i = 0; i < objects.Length; i++)
        {
			EditorUtility.DisplayProgressBar("添加离线数据", $"正在修改{objects[i].name}的离线数据...", 1.0f / objects.Length * i);
			CreateOfflineData(objects[i]);
        }
		EditorUtility.ClearProgressBar();
    }

    public static void CreateOfflineData(GameObject obj)
    {
		OfflineData offlineData = obj.GetComponent<OfflineData>();
		if (offlineData == null)
        {
			offlineData = obj.AddComponent<OfflineData>();
        }
		offlineData.BindData();
		EditorUtility.SetDirty(obj);
		Debug.Log($"Prefab:{obj.name}属性已保存!");
		Resources.UnloadUnusedAssets();
		AssetDatabase.Refresh();
    }

    [MenuItem("Assets/ResLoadFrame/离线数据/生成UI Prefab 离线数据")]
    public static void AssetCreateUIOfflineData()
    {
        GameObject[] objects = Selection.gameObjects;
        for (int i = 0; i < objects.Length; i++)
        {
            EditorUtility.DisplayProgressBar("添加UI离线数据", $"正在修改{objects[i].name}的离线数据...", 1.0f / objects.Length * i);
            CreateUIOfflineData(objects[i]);
        }
        EditorUtility.ClearProgressBar();
    }

    public static void CreateUIOfflineData(GameObject obj)
    {
		obj.layer = LayerMask.NameToLayer("UI");

        UIOfflineData offlineData = obj.GetComponent<UIOfflineData>();
        if (offlineData == null)
        {
            offlineData = obj.AddComponent<UIOfflineData>();
        }
        offlineData.BindData();
        EditorUtility.SetDirty(obj);
        Debug.Log($"UI Prefab:{obj.name}属性已保存!");
        Resources.UnloadUnusedAssets();
        AssetDatabase.Refresh();
    }


    [MenuItem("ResLoadFrame/离线数据/生成所有UI Prefab 离线数据")]
    public static void AllCreateUIOfflineData()
    {
        string[] allStr = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/GameData/Prefabs/UGUI" });
        for (int i = 0; i < allStr.Length; i++)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(allStr[i]);
            EditorUtility.DisplayProgressBar("添加UI离线数据", $"正在扫描路径:{prefabPath}", 1.0f / allStr.Length * i);
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (obj)
            {
                CreateUIOfflineData(obj);
            }
        }
        Debug.Log("UI离线数据全部生成!");
        EditorUtility.ClearProgressBar();
    }

    public static void CreateEffectOfflineData(GameObject obj)
    {
        EffectOfflineData offlineData = obj.GetComponent<EffectOfflineData>();
        if (offlineData == null)
        {
            offlineData = obj.AddComponent<EffectOfflineData>();
        }
        offlineData.BindData();
        EditorUtility.SetDirty(obj);
        Debug.Log($"Effect Prefab:{obj.name}属性已保存!");
        Resources.UnloadUnusedAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/ResLoadFrame/离线数据/生成Effect Prefab 离线数据")]
    public static void AssetCreateEffectOfflineData()
    {
        GameObject[] objects = Selection.gameObjects;
        for (int i = 0; i < objects.Length; i++)
        {
            EditorUtility.DisplayProgressBar("添加离线数据", $"正在修改{objects[i].name}的离线数据...", 1.0f / objects.Length * i);
            CreateEffectOfflineData(objects[i]);
        }
        EditorUtility.ClearProgressBar();
    }

    [MenuItem("ResLoadFrame/离线数据/生成所有Effect Prefab 离线数据")]
    public static void AllCreateEffectOfflineData()
    {
        string[] allStr = AssetDatabase.FindAssets("t:prefab", new string[] { "Assets/GameData/Prefabs/Effect" });
        for (int i = 0; i < allStr.Length; i++)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(allStr[i]);
            EditorUtility.DisplayProgressBar("添加UI离线数据", $"正在扫描路径:{prefabPath}", 1.0f / allStr.Length * i);
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (obj)
            {
                CreateEffectOfflineData(obj);
            }
        }
        Debug.Log("UI离线数据全部生成!");
        EditorUtility.ClearProgressBar();
    }
}

