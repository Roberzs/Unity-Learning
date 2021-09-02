/****************************************************
    文件：KilledEnemyCommand.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/7 14:44:2
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign.Example
{
    public class KilledEnemyCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var gameModel = this.GetModel<IGameModel>();

            gameModel.KillCount.Value++;

            if (gameModel.KillCount.Value == 9)
            {
                this.SendEvent<GamePassEvent>();
            }
        }
    }
}
