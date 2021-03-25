/****************************************************
    文件：IBaseResourcesFactory.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/9 10:48:59
    功能：其他资源工厂接口
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseResourcesFactory<T>
{
    T GetSingleResources(string resourcePath);
}
