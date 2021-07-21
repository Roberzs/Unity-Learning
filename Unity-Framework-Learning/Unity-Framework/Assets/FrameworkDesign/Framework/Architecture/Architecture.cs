/****************************************************
    文件：Architecture.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/8 15:28:13
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameworkDesign
{
    public interface IArchitecture
    {
        // 注册Model
        void RegisterModel<T>(T instance) where T : IModel;

        // 注册Utility
        void RegisterUtility<T>(T instance);

        T GetUtility<T>() where T : class;
    }

    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        private static T mArchitecture = null;

        // 是否已经初始化完成
        private bool mInited = false;

        // 用于初始化的 Models 的缓存
        private List<IModel> mModels = new List<IModel>();

        // 注册Model的API
        public void RegisterModel<T>(T instance) where T: IModel
        {
            instance.Architecture = this;
            mArchitecture.mContainer.Register<T>(instance);

            if (mInited)
            {
                instance.Init();
            }
            else
            {
                mModels.Add(instance);
            }
        }

        // 注册Patch
        public static Action<T> OnRegisterPatch = architecture => { };

        private static void MakeSureArchitecture()
        {
            if (mArchitecture == null)
            {
                mArchitecture = new T();
                mArchitecture.Init();

                // 调用Patch
                OnRegisterPatch?.Invoke(mArchitecture);

                // 初始化Model
                foreach (var architectureModel in mArchitecture.mModels)
                {
                    architectureModel.Init();
                }

                // 清空Model
                mArchitecture.mModels.Clear();
                mArchitecture.mInited = true;
            }
        }

        private IOCContainer mContainer = new IOCContainer();

        // 子类的注册模块
        protected abstract void Init();

        // 注册模块的API
        public static void Register<T>(T instance)
        {
            MakeSureArchitecture();
            mArchitecture.mContainer.Register<T>(instance);
        }

        public T GetUtility<T>() where T : class
        {
            return mContainer.Get<T>();
        }

        public void RegisterUtility<T>(T instance)
        {
            mContainer.Register<T>(instance);
        }

        // 获取模块的API
        public static T Get<T>() where T : class
        {
            MakeSureArchitecture();
            return mArchitecture.mContainer.Get<T>();
        }
    }
}

