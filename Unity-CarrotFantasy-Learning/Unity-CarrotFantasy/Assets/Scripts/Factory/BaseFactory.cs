/****************************************************
    文件：BaseFactory.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/9 11:04:00
    功能：物体类型工厂基类
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseFactory : IBaseFactory
{
    // 当前拥有的GameObject类型的资源 (UI, UIPanel, Game)
    protected Dictionary<string, GameObject> factoryDict = new Dictionary<string, GameObject>();

    // 对象池字典
    protected Dictionary<string, Stack<GameObject>> objectPoolDict = new Dictionary<string, Stack<GameObject>>();

    // 资源路径
    protected string loadPath;

    public BaseFactory()
    {
        loadPath = "Prefabs/";
    }

    public void PushItem(string itemName, GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(GameManager.Instance.transform);
        if (objectPoolDict.ContainsKey(itemName))
        {
            objectPoolDict[itemName].Push(item);
        }
        else
        {
            Debug.LogError("当前字典没有" + itemName + "的栈");
        }
    }

    public GameObject GetItem(string itemName)
    {
        GameObject itemGo;
        if (objectPoolDict.ContainsKey(itemName))
        {
            if (objectPoolDict[itemName].Count == 0)
            {
                GameObject go = GetResource(itemName);
                itemGo = GameManager.Instance.CreateItem(go);
            }
            else
            {
                itemGo = objectPoolDict[itemName].Pop();
                itemGo.SetActive(true);
            }
        }
        else
        {
            objectPoolDict.Add(itemName, new Stack<GameObject>());
            GameObject go = GetResource(itemName);
            itemGo = GameManager.Instance.CreateItem(go);
        }

        if (itemGo == null)
        {
            Debug.LogError("没有获取到" + itemName + "的实例" );
        }

        return itemGo;
    }

    // 获取资源
    private GameObject GetResource(string itemName)
    {
        GameObject itemGo = null;
        string itemLoadPath = loadPath + itemName;
        if (factoryDict.ContainsKey(itemName))
        {
            itemGo = factoryDict[itemName];
        }
        else
        {
            itemGo = Resources.Load<GameObject>(itemLoadPath);
            factoryDict.Add(itemName, itemGo);
        }
        if (itemGo == null)
        {
            Debug.LogError("没有获取到"+itemName+"的资源  路径:" + itemLoadPath);
        }
        return itemGo;
    }
}
