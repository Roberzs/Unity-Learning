/****************************************************
    文件：CubeInfoInspector.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(CubeInfo), editorForChildClasses:true)]
public class CubeInfoInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var cubeInfo = target as CubeInfo;

        GUILayout.BeginVertical("Box");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Name:", GUILayout.Width(150));
        cubeInfo.Name = GUILayout.TextField(cubeInfo.Name);
        GUILayout.EndHorizontal();

        GUILayout.EndHorizontal();

        

    }
}
