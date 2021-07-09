/****************************************************
    文件：PointGame.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/8 16:57:39
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign.Example
{
    public class PointGame : Architecture<PointGame>
    {
        protected override void Init()
        {
            Register<IGameModel>(new GameModel());
        }
    }
}

