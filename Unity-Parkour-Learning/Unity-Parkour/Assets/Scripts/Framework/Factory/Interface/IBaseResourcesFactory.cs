/****************************************************
    文件：IBaseResourcesFactory.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：其他资源类工厂基类
*****************************************************/

using UnityEngine;

public interface IBaseResourcesFactory<T> 
{
    T GetSingleResources(string resourcePath);
}
