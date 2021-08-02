/****************************************************
    文件：AchievementSystem.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/26 11:10:9
    功能：Nothing
*****************************************************/

using Counter;
using UnityEngine;

namespace FrameworkDesign
{
    public interface IAchievementSystem : ISystem
    {

    }

    public class AchievementSystem : AbstractSystem, IAchievementSystem
    {
        protected override void OnInit()
        {
            var counterModel = GetArchitecture().GetModel<ICountModel>();
            var previousCount = counterModel.Count.Value;
            
            counterModel.Count.OnValueChanged += newCount =>
            {
                if (newCount >= 10 && previousCount < 10)
                {
                    Debug.Log("点击10次, 解锁新成就!");
                }
                if (newCount >= 20 && previousCount < 20)
                {
                    Debug.Log("点击20次, 解锁新成就!");
                }
            };

        }
    }
}

