/****************************************************
    文件：ISystem.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/26 10:51:51
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign
{
    public interface ISystem : IBelongToArchitecture
    {
        void Init();
    }
}

