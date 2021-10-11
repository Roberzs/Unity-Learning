/****************************************************
    文件：Model.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

public abstract class Model 
{
    public abstract string Name { get; }

    protected void SendEvent(string eventName, object data = null) 
    {
        MVC.SendEvent(eventName, data);
    }
}
