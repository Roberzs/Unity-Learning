/****************************************************
    文件：EnemyKilledObserverStageSystem.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/24 10:21:58
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKilledObserverStage : IGameEventObserver
{
    private EnemyKilledSubject mSubject;
    private StageSystem mStageSystem;

    public EnemyKilledObserverStage(StageSystem stageSystem)
    {
        mStageSystem = stageSystem;
    }

    public override void SetSubject(IGameEventSubject sub)
    {
        mSubject = sub as EnemyKilledSubject;
    }

    public override void Update()
    {
        mStageSystem.CountOfEnemyKilled = mSubject.KilledCount;
    }
}
