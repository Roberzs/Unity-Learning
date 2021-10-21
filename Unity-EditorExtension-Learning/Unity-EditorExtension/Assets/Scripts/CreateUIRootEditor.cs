/****************************************************
    文件：CreateUIRoot.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using System.IO;

public class CreateUIRootEditor : EditorWindow
{
    /// <summary>
    /// 验证按钮功能是否可用
    /// </summary>
    /// <returns></returns>
    [MenuItem("编辑器扩展/CreateUIRoot", validate = true)]
    static bool ValidateUIRoot()
    {
        return !GameObject.Find("UIRoot");
    }

    [MenuItem("编辑器扩展/CreateUIRoot")]
    static void CreateUIRoot()
    {
        var window = GetWindow<CreateUIRootEditor>();
        window.Show();

        
    }

    private void OnGUI()
    {
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Width:", GUILayout.Width(50f));
        var width = GUILayout.TextField("1920");
        GUILayout.Label("x", GUILayout.Width(10f));
        GUILayout.Label("Height:", GUILayout.Width(55f));
        var height = GUILayout.TextField("1080");
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Create"))
        {
            Create(float.Parse(width), float.Parse(height));
            Close();
        }
    }

    private void Create(float width, float height)
    {
        /** UIRoot 基础结构 */
        var uiRootObj = new GameObject("UIRoot");
        uiRootObj.layer = LayerMask.NameToLayer("UI");
        var canvasObj = new GameObject("Canvas");
        canvasObj.transform.SetParent(uiRootObj.transform);
        canvasObj.layer = LayerMask.NameToLayer("UI");
        canvasObj.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        var canvasScaler = canvasObj.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(width, height);
        canvasObj.AddComponent<GraphicRaycaster>();
        var eventSystemObj = new GameObject("EventSystem");
        eventSystemObj.transform.SetParent(uiRootObj.transform);
        eventSystemObj.layer = LayerMask.NameToLayer("UI");
        eventSystemObj.AddComponent<EventSystem>();
        eventSystemObj.AddComponent<StandaloneInputModule>();

        /** UIRoot 扩展物体 */
        UIRoot uiRootScript = uiRootObj.AddComponent<UIRoot>();
        // 序列化方式设置脚本私有变量 (修改完记得ApplyModifiedPropertiesWithoutUndo)
        var uiRootScriptSerializedObj = new SerializedObject(uiRootScript);
        uiRootScriptSerializedObj.FindProperty("mRootCanvas").objectReferenceValue = canvasObj.GetComponent<Canvas>();
        uiRootScriptSerializedObj.ApplyModifiedPropertiesWithoutUndo();
        // 普通方式设置脚本公共变量
        var bgObj = new GameObject("bgObj");
        bgObj.transform.SetParent(canvasObj.transform);
        bgObj.AddComponent<RectTransform>();
        uiRootScript.bgTrans = bgObj.transform;

        var saveFolderPath = Application.dataPath + "/Resources/Prefabs";
        if (!Directory.Exists(saveFolderPath)) Directory.CreateDirectory(saveFolderPath);
        PrefabUtility.SaveAsPrefabAssetAndConnect(uiRootObj, saveFolderPath + "/UIRoot.prefab", InteractionMode.AutomatedAction);

    }
}
