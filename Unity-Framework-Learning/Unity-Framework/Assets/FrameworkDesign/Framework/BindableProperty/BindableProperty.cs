/****************************************************
    文件：BindableProperty.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/7 10:7:24
    功能：数据绑定类 用于扩展数据属性
*****************************************************/

using System;
using UnityEngine;

namespace FrameworkDesign
{
    public class BindableProperty<T> where T: IEquatable<T>
    {
        private T mValue;

        public T Value
        {
            get => mValue;
            set
            {
                mValue = value;
                OnValueChanged?.Invoke(value);
            }
        }

        public Action<T> OnValueChanged;
    }
}

