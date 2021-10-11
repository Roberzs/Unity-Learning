/****************************************************
    文件：View.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    public abstract string Name { get; }

    public List<string> AttentionList = new List<string>();

    public void RegisterAttentionEvent()
    {

    }

    public abstract void HandleEvent(string name, object data);

    protected void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    }

    // 获取模型
    protected T GetModel<T>() where T : Model
    {
        return MVC.GetModel<T>() as T;
    }

}
