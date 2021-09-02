/****************************************************
    文件：ICanGetModel.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/9/2 11:10:30
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign
{
    public interface ICanGetModel : IBelongToArchitecture
    {

    }

    public static class CanGetModelExtension
    {
        public static T GetModel<T>(this ICanGetModel self) where T : class, IModel
        {
            return self.GetArchitecture().GetModel<T>();
        }
    }
}


