/****************************************************
    文件：DestoryForTime.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/10 21:03:35
    功能：用于到达销毁物体
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class DestoryForTime:MonoBehaviour
{
    public float time = 1;

    private void Start()
    {
        Invoke("Destroy", time);
    }

    private void Destroy()
    {
        GameObject.DestroyImmediate(this.gameObject);
    }
}
