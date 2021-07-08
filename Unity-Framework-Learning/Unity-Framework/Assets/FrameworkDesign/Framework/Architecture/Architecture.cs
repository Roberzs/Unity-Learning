/****************************************************
    文件：Architecture.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/8 15:28:13
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign
{
    public abstract class Architecture<T> where T : Architecture<T>, new()
    {
        private static T mArchitecture = null;

        private static void MakeSureArchitecture()
        {
            if (mArchitecture == null)
            {
                mArchitecture = new T();
                mArchitecture.Init();
            }
        }

        private IOCContainer mContainer = new IOCContainer();

        // 子类的注册模块
        protected abstract void Init();

        // 注册模块的API
        public void Register<T>(T instance)
        {
            MakeSureArchitecture();
            mArchitecture.mContainer.Register<T>(instance);
        }

        // 获取模块的API
        public static T Get<T>() where T : class
        {
            MakeSureArchitecture();
            return mArchitecture.mContainer.Get<T>();
        }
    }
}

