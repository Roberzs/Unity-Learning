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
    public interface ISystem : IBelongToArchitecture, ICanSetArchitecture
    {
        void Init();
    }

    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture mArchitecture = null;
        public IArchitecture GetArchitecture()
        {
            return mArchitecture;
        }

        void ISystem.Init()
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

