/****************************************************
    文件：UITool.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/10 18:49:37
    功能：UI工具类
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public static class UITool
{
    public static GameObject GetCanvas(string name = "Canvas")
    {
        return GameObject.Find(name);
    }

    public static T FindChild<T>(GameObject parent, string childName)
    {
        GameObject uiGO = UnityTool.FindChild(parent, childName);
        if (uiGO == null) return default(T);
        return uiGO.GetComponent<T>();
    }
}