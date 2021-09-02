/****************************************************
    文件：ITypeEventSyste.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/9/2 13:5:49
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameworkDesign
{
    public interface ITypeEventSystem
    {
        // 发送事件
        void Send<T>() where T : new();
        void Send<T>(T e);

        // 注册事件
        IUnRegister Register<T>(Action<T> onEvent);

        // 注销事件
        void UnRegister<T>(Action<T> onEvent);
    }

    /// <summary>
    /// 用于注销的接口
    /// </summary>
    public interface IUnRegister
    {
        void UnRegister();
    }

    public class TypeEventSystemUnRegister<T> : IUnRegister
    {
        public ITypeEventSystem TypeEventSystem { get; set; }
        public Action<T> OnEvent { get; set; }

        public void UnRegister()
        {
            TypeEventSystem.UnRegister(OnEvent);

            TypeEventSystem = null;
            OnEvent = null;
        }
    }

    /// <summary>
    /// 注销触发器
    /// </summary>
    public class UnRegisterOnDestroyTrigger: MonoBehaviour
    {
        private HashSet<IUnRegister> mUnRegisters = new HashSet<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister)
        {
            mUnRegisters.Add(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var unRegister in mUnRegisters)
            {
                unRegister.UnRegister();
            }

            mUnRegisters.Clear();
        }
    }

    /// <summary>
    /// 注销触发器的简化使用
    /// </summary>
    public static class UnRegisterExtension
    {
        public static void UnRegisterWhenGameObjectDestory(this IUnRegister unRegister, GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>();
            
            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
            }

            trigger.AddUnRegister(unRegister);
        }
    }

    public class TypeEventSystem : ITypeEventSystem
    {
        interface IRegistrations { }

        class Registrations<T> : IRegistrations
        {
            /// <summary>
            /// 委托的一对多注册
            /// </summary>
            public Action<T> OnEvent = obj => { };
        }

        private Dictionary<Type, IRegistrations> mEventRegistrations = new Dictionary<Type, IRegistrations>();

        public IUnRegister Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations eventRegistrations;

            if (!mEventRegistrations.TryGetValue(type, out eventRegistrations))
            {
                eventRegistrations = new Registrations<T>();
                mEventRegistrations.Add(type, eventRegistrations);
            }

            (eventRegistrations as Registrations<T>).OnEvent += onEvent;
            return new TypeEventSystemUnRegister<T>()
            {
                OnEvent = onEvent,
                TypeEventSystem = this
            };
        }

        public void Send<T>() where T : new()
        {
            var e = new T();
            Send<T>(e);
        }

        public void Send<T>(T e)
        {
            var type = typeof(T);
            IRegistrations eventRegistrations;

            if (mEventRegistrations.TryGetValue(type, out eventRegistrations))
            {
                (eventRegistrations as Registrations<T>)?.OnEvent.Invoke(e);
            }
        }

        public void UnRegister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations eventRegistrations;

            if (mEventRegistrations.TryGetValue(type, out eventRegistrations))
            {
                (eventRegistrations as Registrations<T>).OnEvent -= onEvent;
            }
        }
    }
}
