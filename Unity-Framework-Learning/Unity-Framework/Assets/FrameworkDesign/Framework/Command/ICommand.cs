/****************************************************
    文件：ICommand.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/7 13:45:23
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign
{
    public interface ICommand : IBelongToArchitecture, ICanSetArchitecture
    {
        void Execute();
    }

    public abstract class AbstractCommand : ICommand
    {
        private IArchitecture mArchitecture;

        void ICommand.Execute()
        {
            OnExecute();
        }

        public IArchitecture GetArchitecture()
        {
            return mArchitecture;
        }

        public void SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }

        protected abstract void OnExecute();
    }
}


