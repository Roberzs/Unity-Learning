/****************************************************
    文件：ICanRegisterEvent.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/9/2 15:52:28
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

namespace FrameworkDesign
{
    public interface ICanRegisterEvent : IBelongToArchitecture
    {

    }

    public static class CanRegisterEventExtension
    {
        public static void RegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent) 
        {
            self.GetArchitecture().RegisterEvent<T>(onEvent);
        }

        public static void UnRegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            self.GetArchitecture().UnRegisterEvent<T>(onEvent);
        }
    }

}

