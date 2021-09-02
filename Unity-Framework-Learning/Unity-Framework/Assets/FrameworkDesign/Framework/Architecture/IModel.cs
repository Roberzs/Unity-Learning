/****************************************************
    文件：IModel.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/21 14:35:53
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

namespace FrameworkDesign
{
    public interface IModel : IBelongToArchitecture, ICanSetArchitecture, ICanGetUtility, ICanSendEvent
    {
        void Init();
    }

    public abstract class AbstractModel : IModel
    {
        private IArchitecture mArchitecture = null;

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return mArchitecture;
        }

        void IModel.Init()
        {
            OnInit();
        }

        protected abstract void OnInit();

        
    }
}

