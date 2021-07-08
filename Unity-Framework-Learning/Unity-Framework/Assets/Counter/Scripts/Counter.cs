/****************************************************
    文件：Counter.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/8 16:26:0
    功能：Nothing
*****************************************************/

using FrameworkDesign;
using UnityEngine;

namespace Counter
{
    public class Counter : Architecture<Counter>
    {
        protected override void Init()
        {
            Register<CounterModel>(new CounterModel());
        }
    }
}

