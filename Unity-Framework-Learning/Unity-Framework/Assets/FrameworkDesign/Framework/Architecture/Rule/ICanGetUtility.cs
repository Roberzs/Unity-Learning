/****************************************************
    文件：ICanGetUtility.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/9/2 10:48:48
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign
{
    public interface ICanGetUtility : IBelongToArchitecture
    {

    }

    public static class CanGetUtilityExtension
    {
        public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility
        {
            return self.GetArchitecture().GetUtility<T>();
        }
    }
}


