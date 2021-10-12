/****************************************************
    文件：MVC.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public static class MVC 
{
    public static Dictionary<string, Model> Models = new Dictionary<string, Model>();
    public static Dictionary<string, View> Views = new Dictionary<string, View>();
    public static Dictionary<string, Type> CommandMap = new Dictionary<string, Type>();

    /** 注册 */
    public static void RegisterController(string eventName, Type controllerType)
    {
        CommandMap[eventName] = controllerType;
    }

    public static void RegisterView(View view)
    {
        if (Views.ContainsKey(view.name))
        {
            Views.Remove(view.name);
        }
        view.RegisterAttentionEvent();
        Views[view.Name] = view;
    }

    public static void RegisterModel(Model model)
    {
        Models[model.Name] = model;
    }

    /** 获取 */
    public static T GetModel<T>() where T : Model
    {
        foreach(var m in Models.Values)
        {
            if (m is T)
            {
                return (T)m;
            }
        }
        return null;
    }

    public static T GetView<T>() where T : View
    {
        foreach (var m in Views.Values)
        {
            if (m is T)
            {
                return (T)m;
            }
        }
        return null;
    }

    /** 事件处理 */
    public static void SendEvent(string eventName, object data = null)
    {
        if (CommandMap.ContainsKey(eventName))
        {
            Type t = CommandMap[eventName];
            Controller c = Activator.CreateInstance(t) as Controller;
            c.Execute(data);
        }

        foreach (var v in Views.Values)
        {
            if (v.AttentionList.Contains(eventName))
            {
                v.HandleEvent(eventName, data);
            }
        }
    }
}
