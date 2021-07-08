/****************************************************
    文件：Singleton.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/8 14:37:10
    功能：Nothing
*****************************************************/

using System;
using System.Reflection;

namespace FrameworkDesign
{
    public class Singleton<T> where T : class
    {
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    // 通过反射获取构造
                    var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    // 获取无参非 public 的构造
                    var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

                    if (ctor == null)
                    {
                        throw new Exception("Non-Public Constructor() not found in " + typeof(T));
                    }

                    mInstance = ctor.Invoke(null) as T;
                }

                return mInstance;
            }
        }

        private static T mInstance;

    }
}

