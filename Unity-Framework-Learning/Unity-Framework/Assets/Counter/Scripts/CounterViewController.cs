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
        private ICountModel mCounterModel;

        private void Start()
        {
            mCounterModel = Counter.Get<ICountModel>();

            mCounterModel.Count.OnValueChanged += OnCountChanged;

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

            OnCountChanged(mCounterModel.Count.Value);
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
            mCounterModel.Count.OnValueChanged -= OnCountChanged;
        }
    }

    public interface ICountModel : IModel
    {
        BindableProperty<int> Count { get; }
    }

public class CounterModel : ICountModel
    {
        public void Init()
        {
            var storage = Architecture.GetUtility<IStorage>();

            Debug.Log(storage + " Loaded");

            Count.Value = storage.LoadInt("COUNTER_COUNT", 0);
            Count.OnValueChanged += count =>
            {
                storage.SaveInt("COUNTER_COUNT", count);
            };
        }

        public BindableProperty<int> Count { get; } = new BindableProperty<int>()
        {
            Value = 0
        };

        public IArchitecture Architecture { get; set; }

        
    }
}

