/****************************************************
    文件：Architecture.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/8 15:28:13
    功能：用于注册获取模块与Model
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameworkDesign
{
    public interface IArchitecture
    {
        // 注册System
        void RegisterSystem<T>(T instance) where T : ISystem;

        // 注册Model
        void RegisterModel<T>(T instance) where T : IModel;

        // 注册Utility
        void RegisterUtility<T>(T instance);

        // 获取Model
        T GetModel<T>() where T : class, IModel;

        // 获取工具
        T GetUtility<T>() where T : class;

        // 发送命令
        void SendCommand<T>() where T : ICommand, new();
        void SendCommand<T>(T command) where T : ICommand;
    }

    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        private static T mArchitecture = null;

        public static IArchitecture Interface
        {
            get
            {
                if (mArchitecture == null) 
                {
                    MakeSureArchitecture();
                }
                return mArchitecture;
            }
        }

        // 是否已经初始化完成
        private bool mInited = false;

        // 用于初始化的 Models 的缓存
        private List<IModel> mModels = new List<IModel>();

        // 注册Model的API
        public void RegisterModel<T>(T instance) where T: IModel
        {
            instance.SetArchitecture(this);
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

                // 初始化System
                foreach (var architectureSystem in mArchitecture.mSystems)
                {
                    architectureSystem.Init();
                }

                // 清空Models
                mArchitecture.mModels.Clear();
                // 清空Systems
                mArchitecture.mSystems.Clear();
                mArchitecture.mInited = true;
            }
        }

        // 存储初始化的Systems的缓存
        private List<ISystem> mSystems = new List<ISystem>();
        public void RegisterSystem<T>(T instance) where T : ISystem
        {
            instance.SetArchitecture(this);
            mContainer.Register<T>(instance);

            if (mInited)
            {
                instance.Init();
            }
            else
            {
                mSystems.Add(instance);
            }
        }

        private IOCContainer mContainer = new IOCContainer();

        // 子类的注册模块初始化
        protected abstract void Init();

        // 注册模块的API
        public static void Register<T>(T instance)
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

        public void RegisterUtility<T>(T instance)
        {
            mContainer.Register<T>(instance);
        }

        public T GetUtility<T>() where T : class
        {
            return mContainer.Get<T>();
        }
        
        public T GetModel<T>() where T : class, IModel
        {
            return mContainer.Get<T>();
        }

        public void SendCommand<T>() where T : ICommand, new()
        {
            var command = new T();
            command.SetArchitecture(this);
            command.Execute();
        }

        public void SendCommand<T>(T command) where T : ICommand
        {
            command.SetArchitecture(this);
            command.Execute();
        }
    }
}

