/****************************************************
    文件：StartGameCommand.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/8 14:21:56
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign.Example
{
    public class StartGameCommand :AbstractCommand, ICommand
    {

        protected override void OnExecute()
        {
            this.SendEvent<GameStartEvent>();
        }
    }
}

