/****************************************************
    文件：CounterViewController.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/6 17:35:3
    功能：Nothing
*****************************************************/

using FrameworkDesign;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Counter
{
    public class CounterViewController : MonoBehaviour
    {
        private void Start()
        {
            OnCountChangedEvent.Register(OnCountChanged);

            transform.Find("btnAdd").GetComponent<Button>().onClick
                .AddListener(() =>
                {
                    CounterModel.Count++;
                });
            transform.Find("btnSub").GetComponent<Button>().onClick
                .AddListener(() =>
                {
                    CounterModel.Count--;
                });

            OnCountChanged();
        }

        /// <summary>
        /// 表现逻辑
        /// </summary>
        private void OnCountChanged()
        {
            transform.Find("txtCount").GetComponent<Text>()
                .text = CounterModel.Count.ToString();
        }

        private void OnDestroy()
        {
            OnCountChangedEvent.UnRegister(OnCountChanged);
        }
    }

    public static class CounterModel
    {
        private static int mCount = 0;

        public static int Count
        {
            get => mCount;
            set
            {
                if (value != mCount)
                {
                    mCount = value;

                    OnCountChangedEvent.Trigger();
                }
            }
        }
    }

    // 定义事件
    public class OnCountChangedEvent : Event<OnCountChangedEvent>
    {

    }
}

