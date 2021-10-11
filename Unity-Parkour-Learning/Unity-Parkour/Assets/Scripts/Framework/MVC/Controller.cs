/****************************************************
    文件：Controller.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

 public abstract class Controller : MonoBehaviour
{
    public abstract void Execute(object data);

    protected void RegisterController(string eventName, Type controllerType)
    {
        MVC.RegisterController(eventName, controllerType);
    }

    protected void RegisterView(View view)
    {
        MVC.RegisterView(view);
    }

    protected void RegisterModel(Model model)
    {
        MVC.RegisterModel(model);
    }

    // 获取模型
    protected T GetModel<T>() where T : Model
    {
        return MVC.GetModel<T>() as T;
    }

    // 获取视图
    protected T GetView<T>() where T : View
    {
        return MVC.GetView<T>() as T;
    }

}
