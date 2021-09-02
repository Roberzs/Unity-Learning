/****************************************************
    文件：ICanSendCommand.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/9/2 11:46:22
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign
{
    public interface ICanSendCommand : IBelongToArchitecture
    {

    }

    public static class CanSendCommandExtension
    {
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new()
        {
            self.GetArchitecture().SendCommand<T>();
        }

        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand
        {
            self.GetArchitecture().SendCommand<T>(command);
        }
    }
}

