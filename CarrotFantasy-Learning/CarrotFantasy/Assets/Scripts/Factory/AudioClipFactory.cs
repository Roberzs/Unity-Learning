/****************************************************
    文件：AudioClipFactory.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/11 10:39:13
    功能：音频资源工厂
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipFactory : IBaseResourcesFactory<AudioClip>
{

    protected Dictionary<string, AudioClip> factoryDict = new Dictionary<string, AudioClip>();
    protected string loadPath;

    public AudioClipFactory()
    {
        loadPath = "AudioClips/";
    }

    public AudioClip GetSingleResources(string resourcePath)
    {
        string itemLoadPath = loadPath + resourcePath;
        AudioClip itemGo;
        if (factoryDict.ContainsKey(resourcePath))
        {
            itemGo = factoryDict[resourcePath];
        }
        else
        {
            itemGo = Resources.Load<AudioClip>(itemLoadPath);
            factoryDict.Add(resourcePath, itemGo);
        }
        if (itemGo == null)
        {
            Debug.LogError("没有获取到" + resourcePath + "的资源  路径:" + itemLoadPath);
        }
        return itemGo;
    }
}
