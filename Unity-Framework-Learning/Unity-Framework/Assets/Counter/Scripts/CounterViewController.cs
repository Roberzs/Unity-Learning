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
            CounterModel.Count.OnValueChanged += OnCountChanged;

            transform.Find("btnAdd").GetComponent<Button>().onClick
                .AddListener(() =>
                {
                    new AddCountCommand().Execute();
                });
            transform.Find("btnSub").GetComponent<Button>().onClick
                .AddListener(() =>
                {
                    new SubCountCommand().Execute();
                });

            OnCountChanged(CounterModel.Count.Value);
        }

        /// <summary>
        /// 表现逻辑
        /// </summary>
        private void OnCountChanged(int newValue)
        {
            transform.Find("txtCount").GetComponent<Text>()
                .text = newValue.ToString();
        }

        private void OnDestroy()
        {
            CounterModel.Count.OnValueChanged -= OnCountChanged;
        }
    }

    public static class CounterModel
    {
        public static BindableProperty<int> Count = new BindableProperty<int>()
        {
            Value = 0
        };
    }
}

