/****************************************************
    文件：CounterViewController.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/6 17:35:3
    功能：Nothing
*****************************************************/

using FrameworkDesign;
using UnityEngine;
using UnityEngine.UI;

namespace Counter
{
    public class CounterViewController : MonoBehaviour, IController
    {
        private ICountModel mCounterModel;


        private void Start()
        {
            mCounterModel = this.GetModel<ICountModel>();

            mCounterModel.Count.OnValueChanged += OnCountChanged;

            transform.Find("btnAdd").GetComponent<Button>().onClick
                .AddListener(() =>
                {
                    this.SendCommand<AddCountCommand>();
                });
            transform.Find("btnSub").GetComponent<Button>().onClick
                .AddListener(() =>
                {
                    this.SendCommand<SubCountCommand>();
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
            
            mCounterModel = null;
        }

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return Counter.Interface;
        }
    }

    public interface ICountModel : IModel
    {
        BindableProperty<int> Count { get; }
    }

    public class CounterModel : AbstractModel, ICountModel
    {

        protected override void OnInit()
        {
            var storage = this.GetUtility<IStorage>();

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


        
    }
}

