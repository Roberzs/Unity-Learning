/****************************************************
    文件：IController.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/26 14:12:43
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign 
{
    public interface IController : IBelongToArchitecture, ICanGetSystem, ICanGetModel, ICanGetUtility, ICanSendCommand, ICanRegisterEvent
    {

    }
}


