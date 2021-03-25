/****************************************************
    文件：SpriteFactory.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/11 22:27:24
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFactory : IBaseResourcesFactory<Sprite>
{
    protected Dictionary<string, Sprite> factoryDict = new Dictionary<string, Sprite>();
    protected string loadPath;

    public SpriteFactory()
    {
        loadPath = "Pictures/";
    }

    public Sprite GetSingleResources(string resourcePath)
    {
        string itemLoadPath = loadPath + resourcePath;
        Sprite itemGo;
        if (factoryDict.ContainsKey(resourcePath))
        {
            itemGo = factoryDict[resourcePath];
        }
        else
        {
            itemGo = Resources.Load<Sprite>(itemLoadPath);
            factoryDict.Add(resourcePath, itemGo);
        }
        if (itemGo == null)
        {
            Debug.LogError("没有获取到" + resourcePath + "的资源  路径:" + itemLoadPath);
        }
        return itemGo;
    }
}
