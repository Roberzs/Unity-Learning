/****************************************************
    文件：RuntimeAnimatorControllerFactory.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/11 22:23:25
    功能：动画控制器资源工厂
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeAnimatorControllerFactory : IBaseResourcesFactory<RuntimeAnimatorController>
{
    protected Dictionary<string, RuntimeAnimatorController> factoryDict = new Dictionary<string, RuntimeAnimatorController>();
    protected string loadPath;

    public RuntimeAnimatorControllerFactory()
    {
        loadPath = "Animator/AnimatorController/";
    }

    public RuntimeAnimatorController GetSingleResources(string resourcePath)
    {
        string itemLoadPath = loadPath + resourcePath;
        RuntimeAnimatorController itemGo;
        if (factoryDict.ContainsKey(resourcePath))
        {
            itemGo = factoryDict[resourcePath];
        }
        else
        {
            itemGo = Resources.Load<RuntimeAnimatorController>(itemLoadPath);
            factoryDict.Add(resourcePath, itemGo);
        }
        if (itemGo == null)
        {
            Debug.LogError("没有获取到" + resourcePath + "的资源  路径:" + itemLoadPath);
        }
        return itemGo;
    }
}
