/****************************************************
    文件：GameModel.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/6 16:1:40
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace FrameworkDesign.Example
{
    public interface IGameModel : IModel
    {
        BindableProperty<int> KillCount { get; }

        BindableProperty<int> Glod { get; }

        BindableProperty<int> Score { get; }

        BindableProperty<int> BestScore { get; }
    }

    public class GameModel : AbstractModel, IGameModel
    {

        public BindableProperty<int> KillCount { get; } = new BindableProperty<int>()
        {
            Value = 0
        };

        public BindableProperty<int> Glod { get; } = new BindableProperty<int>()
        {
            Value = 0
        };

        public BindableProperty<int> Score { get; } = new BindableProperty<int>()
        {
            Value = 0
        };

        public BindableProperty<int> BestScore { get; } = new BindableProperty<int>()
        {
            Value = 0
        };

        protected override void OnInit()
        {
            
        }
    }
}

