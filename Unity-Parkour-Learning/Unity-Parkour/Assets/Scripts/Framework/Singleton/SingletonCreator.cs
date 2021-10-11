/****************************************************
    文件：SingletonCreator.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/29 14:8:29
    功能：Nothing
*****************************************************/

using System;
using System.Reflection;

namespace FrameworkDesign
{
    public static class SingletonCreator
    {
        public static T CreateSingleton<T>() where T : class, ISingleton
        {
            // 通过反射获取构造
            var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            // 获取无参非 public 的构造
            var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

            if (ctor == null)
            {
                throw new Exception("Non-Public Constructor() not found in " + typeof(T));
            }

            var retInstance = ctor.Invoke(null) as T;
            retInstance.OnSingletonInit();

            return retInstance;
        }
    }
}

