/****************************************************
    文件：AddCountCommand.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/7 13:50:44
    功能：Nothing
*****************************************************/

using FrameworkDesign;
using UnityEngine;

namespace Counter
{
    public class AddCountCommand : AbstractCommand 
    {
        protected override void OnExecute()
        {
            GetArchitecture().GetModel<ICountModel>().Count.Value++;
        }
    }
}

