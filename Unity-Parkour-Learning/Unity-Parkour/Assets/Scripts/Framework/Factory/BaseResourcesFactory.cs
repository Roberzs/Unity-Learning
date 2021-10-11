/****************************************************
    文件：BaseResourcesFactory.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class BaseResourcesFactory<T> : IBaseResourcesFactory<T> where T : Object
{
    private Dictionary<string, T> factoryDict = new Dictionary<string, T>();
    private string loadPath;

    public BaseResourcesFactory(string loadPath)
    {
        this.loadPath = loadPath;
    }


    public T GetSingleResources(string resourcePath)
    {
        string itemLoadPath = loadPath + resourcePath;
        T itemGo;
        if (factoryDict.ContainsKey(resourcePath))
        {
            itemGo = factoryDict[resourcePath];
        }
        else
        {
            itemGo = Resources.Load<T>(itemLoadPath);
            factoryDict.Add(resourcePath, itemGo);
        }
        if (itemGo == null)
        {
            Debug.LogError("没有获取到" + resourcePath + "的资源  路径:" + itemLoadPath);
        }
        return itemGo;
    }
}
