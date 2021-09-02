/****************************************************
    文件：TypeEventSystemExample.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/9/2 13:56:44
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

namespace FrameworkDesign.Example
{
    public class TypeEventSystemExample : MonoBehaviour
    {
        public struct EventA
        {

        }

        public struct EventB
        {
            public int ParamB;
        }

        public interface IEventGroup
        {

        }

        public struct EventC : IEventGroup
        {

        }

        public struct EventD : IEventGroup
        {

        }

        private ITypeEventSystem mTypeEventSystem = null;

        private void Start()
        {
            mTypeEventSystem = new TypeEventSystem();

            mTypeEventSystem.Register<EventA>(eA =>
            {
                Debug.Log("OnEventA");
            }).UnRegisterWhenGameObjectDestory(gameObject);

            mTypeEventSystem.Register<EventB>(OnEventB);

            mTypeEventSystem.Register<IEventGroup>(group =>
            {
                Debug.Log(group.GetType());
            }).UnRegisterWhenGameObjectDestory(gameObject);
        }

        private void OnEventB(EventB e)
        {
            Debug.Log("OnEventB:" + e.ParamB);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mTypeEventSystem.Send<EventA>();
            }

            if (Input.GetMouseButtonDown(1))
            {
                mTypeEventSystem.Send(new EventB()
                {
                    ParamB = 999
                });
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mTypeEventSystem.Send<IEventGroup>(new EventC());
                mTypeEventSystem.Send<IEventGroup>(new EventD());
            }
        }

        private void OnDestroy()
        {
            mTypeEventSystem.UnRegister<EventB>(OnEventB);
        }
    }
}

