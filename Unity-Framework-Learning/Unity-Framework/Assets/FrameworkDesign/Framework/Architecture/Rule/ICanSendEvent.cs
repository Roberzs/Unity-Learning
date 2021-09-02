/****************************************************
    文件：ICanSendEvent.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/9/2 15:44:2
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign
{
    public interface ICanSendEvent : IBelongToArchitecture
    {

    }

    public static class CanSendEventExtension
    {
        public static void SendEvent<T>(this ICanSendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvent<T>();
        }

        public static void SendEvent<T>(this ICanSendEvent self, T e)
        {
            self.GetArchitecture().SendEvent<T>(e);
        }
    }
}

