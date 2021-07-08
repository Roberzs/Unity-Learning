/****************************************************
    文件：IOCContainer.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/8 15:18:9
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameworkDesign
{
    public class IOCContainer
    {
        public Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public void Register<T>(T instance)
        {
            var key = typeof(T);

            if (mInstances.ContainsKey(key))
            {
                mInstances[key] = instance;
            }
            else
            {
                mInstances.Add(key, instance);
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : class
        {
            var key = typeof(T);

            object retObj;

            if (mInstances.TryGetValue(key, out retObj))
            {
                return retObj as T;
            }

            return null;
        }

    }
}

