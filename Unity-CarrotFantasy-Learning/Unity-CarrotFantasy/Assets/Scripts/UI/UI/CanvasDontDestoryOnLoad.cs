/****************************************************
    文件：CanvasDontDestoryOnLoad.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/12 18:17:45
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDontDestoryOnLoad : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
