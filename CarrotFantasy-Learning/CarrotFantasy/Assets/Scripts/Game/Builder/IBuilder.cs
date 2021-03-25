/****************************************************
    文件：IBuilder.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/22 14:32:25
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilder<T>
{
    // 获取游戏物体身上的脚本对象
    T GetProductClass(GameObject gameObject);

    // 获取一个具体的游戏对象
    GameObject GetProduct();

    // 获取数据信息
    void GetData(T productClassGo);

    // 获取特有的数据信息
    void GetOtherResource(T productClassGo);
}
