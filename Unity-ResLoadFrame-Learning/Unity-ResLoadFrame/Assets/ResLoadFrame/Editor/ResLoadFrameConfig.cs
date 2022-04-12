using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ResLoadFrameConfig", menuName = "CreateResLoadFrameConfig", order = 0)]
public class ResLoadFrameConfig : ScriptableObject
{
    
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
