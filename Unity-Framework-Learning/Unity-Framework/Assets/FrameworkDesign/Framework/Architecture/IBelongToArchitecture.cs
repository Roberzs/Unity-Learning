/****************************************************
    文件：IBelongToArchitecture.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/21 13:45:15
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign
{
    public interface IBelongToArchitecture
    {
        IArchitecture Architecture { get; set; }
    }
}

