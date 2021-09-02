/****************************************************
    文件：ICanGetSystem.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/9/2 11:35:48
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign
{
    public interface ICanGetSystem : IBelongToArchitecture
    {

    }

    public static class CanGetSystemExtension
    {
        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem
        {
            return self.GetArchitecture().GetSystem<T>();
        }
    }
}

