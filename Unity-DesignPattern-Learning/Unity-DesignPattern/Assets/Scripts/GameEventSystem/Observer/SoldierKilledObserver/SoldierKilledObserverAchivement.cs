/****************************************************
    文件：SoldierKilledObserverAchivement.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/24 11:14:10
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierKilledObserverAchivement : IGameEventObserver
{
    private AchievementSystem mAchievementSystem;

    public SoldierKilledObserverAchivement(AchievementSystem achievementSystem)
    {
        mAchievementSystem = achievementSystem;
    }

    public override void SetSubject(IGameEventSubject sub)
    {
        
    }

    public override void Update()
    {
        mAchievementSystem.AddSoldierKilledCount();
    }
}
