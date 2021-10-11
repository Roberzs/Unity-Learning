/****************************************************
    文件：BaseFactory.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class BaseFactory : IBaseFactory
{
    // 当前拥有的GameObject类型的资源 (UI, UIPanel, Game)
    private Dictionary<string, GameObject> factoryDict = new Dictionary<string, GameObject>();

    // 对象池字典
    private Dictionary<string, Stack<GameObject>> objectPoolDict = new Dictionary<string, Stack<GameObject>>();

    // 资源路径
    private string loadPath;

    // 父物体
    private GameObject parent;

    public BaseFactory(string loadPath, GameObject parent)
    {
        this.loadPath = loadPath;
        this.parent = parent;
    }

    public void PushItem(string itemName, GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(parent.transform);
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
                itemGo = GameObject.Instantiate(go, parent.transform);
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
            itemGo = GameObject.Instantiate(go, parent.transform);
        }

        if (itemGo == null)
        {
            Debug.LogError("没有获取到" + itemName + "的实例");
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
            Debug.LogError("没有获取到" + itemName + "的资源  路径:" + itemLoadPath);
        }
        return itemGo;
    }
}
