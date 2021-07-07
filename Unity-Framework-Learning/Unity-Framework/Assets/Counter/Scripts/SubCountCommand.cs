/****************************************************
    文件：SubCountCommand.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/7 14:1:32
    功能：Nothing
*****************************************************/

using FrameworkDesign;
using UnityEngine;

namespace Counter 
{
    public class SubCountCommand : ICommand
    {
        public void Execute()
        {
            CounterModel.Count.Value--;
        }
    }
}

