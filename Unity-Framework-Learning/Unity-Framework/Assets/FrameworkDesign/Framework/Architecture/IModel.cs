/****************************************************
    文件：IModel.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/21 14:35:53
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign
{
    public interface IModel : IBelongToArchitecture
    {
        void Init();
    }
}

