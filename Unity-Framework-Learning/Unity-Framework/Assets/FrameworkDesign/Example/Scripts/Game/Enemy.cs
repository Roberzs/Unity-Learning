/****************************************************
    文件：Enemy.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/6 15:0:53
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign.Example
{
    public class Enemy : MonoBehaviour, IController
    {
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return PointGame.Interface;
        }

        private void OnMouseDown()
        {
            Destroy(gameObject);

            this.SendCommand<KilledEnemyCommand>();
        }
    }
}

