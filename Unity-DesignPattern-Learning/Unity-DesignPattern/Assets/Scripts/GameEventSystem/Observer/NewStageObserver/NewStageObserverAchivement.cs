/****************************************************
    文件：NewStageObserverAchivement.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/24 11:20:56
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class NewStageObserverAchivement : IGameEventObserver
{
    private NewStageSubject mSubject;

    private AchievementSystem mAchievementSystem;

    public NewStageObserverAchivement(AchievementSystem achievementSystem)
    {
        mAchievementSystem = achievementSystem;
    }

    public override void SetSubject(IGameEventSubject sub)
    {
        mSubject = sub as NewStageSubject;
    }

    public override void Update()
    {
        mAchievementSystem.SetMaxStageLv(mSubject.StageCount);
    }

}
