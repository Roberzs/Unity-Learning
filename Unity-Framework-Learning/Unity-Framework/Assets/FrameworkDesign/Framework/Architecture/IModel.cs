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
    public interface IModel : IBelongToArchitecture, ICanSetArchitecture
    {
        void Init();
    }

    public abstract class AbstractModel : IModel
    {
        private IArchitecture mArchitecture = null;
        public IArchitecture GetArchitecture()
        {
            return mArchitecture;
        }

        void IModel.Init()
        {
            OnInit();
        }

        protected abstract void OnInit();

        public void SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
    }
}

