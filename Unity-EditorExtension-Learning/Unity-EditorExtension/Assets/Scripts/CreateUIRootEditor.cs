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
        }
    }

    private void Create(float width, float height)
    {
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
    }
}
