/****************************************************
    文件：UnityTool.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/10 18:49:37
    功能：工具类
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public static class UnityTool
{
    // 获取父物体下指定子物体
    public static GameObject FindChild(GameObject parent, string childName)
    {
        if (parent == null)
        {
            Debug.LogError("查找" + childName + "时，父物体为空");
            return null;
        }
        Transform[] children = parent.transform.GetComponentsInChildren<Transform>();
        bool isFinded = false;
        Transform child = null;
        foreach (var t in children)
        {
            if (t.name == childName)
            {
                if (isFinded)
                {
                    Debug.LogWarning("在游戏物体" + parent.name + "上，存在多个子物体" + childName);
                }
                isFinded = true;
                child = t;
            }
        }
        if (isFinded) return child.gameObject;
        return null;
    }

    // 将子物体挂载到指定物体上
    public static void Attach(GameObject parent, GameObject child)
    {
        child.transform.parent = parent.transform;
        child.transform.localPosition = Vector3.zero;
        child.transform.localScale = Vector3.one;
        child.transform.localEulerAngles = Vector3.zero;
    }
}
