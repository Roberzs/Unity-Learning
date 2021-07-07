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
    public class GameModel
    {

        public static BindableProperty<int> KillCount = new BindableProperty<int>()
        {
            Value = 0
        };

        public static BindableProperty<int> Glod = new BindableProperty<int>()
        {
            Value = 0
        };

        public static BindableProperty<int> Score = new BindableProperty<int>()
        {
            Value = 0
        };

        public static BindableProperty<int> BestScore = new BindableProperty<int>()
        {
            Value = 0
        };

    }
}

