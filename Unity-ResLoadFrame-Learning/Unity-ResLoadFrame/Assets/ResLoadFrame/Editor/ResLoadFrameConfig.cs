using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ResLoadFrameConfig", menuName = "CreateResLoadFrameConfig", order = 0)]
public class ResLoadFrameConfig : ScriptableObject
{
    public string m_XmlDataPath;
    public string m_BinaryPath;
    public string m_ScriptDataPath;
}

[CustomEditor(typeof(ResLoadFrameConfig))]
public class ResLoadFrameConfigInspector : Editor
{
    public SerializedProperty m_XmlDataPath;
    public SerializedProperty m_BinaryPath;
    public SerializedProperty m_ScriptDataPath;

    private void OnEnable()
    {
        m_XmlDataPath = serializedObject.FindProperty("m_XmlDataPath");
        m_BinaryPath = serializedObject.FindProperty("m_BinaryPath");
        m_ScriptDataPath = serializedObject.FindProperty("m_ScriptDataPath");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_XmlDataPath, new GUIContent("Xml数据路径"));
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(m_BinaryPath, new GUIContent("二进制数据路径"));
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(m_ScriptDataPath, new GUIContent("配置表脚本路径"));
        GUILayout.Space(5);
        serializedObject.ApplyModifiedProperties();
    }
}

public class ResLoadConfig 
{
    private const string ResLoadFrameConfigPath = "";

    //private ResLoadFrameConfig m_ResLoadFrameConfig;

    public static ResLoadFrameConfig GetResLoadConfig()
    {
        //if (!m_ResLoadFrameConfig)
        //{

        //}
        return AssetDatabase.LoadAssetAtPath<ResLoadFrameConfig>(ResLoadFrameConfigPath);
    }
}
