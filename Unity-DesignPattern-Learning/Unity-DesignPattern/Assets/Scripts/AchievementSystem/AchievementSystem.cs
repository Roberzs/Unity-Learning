/****************************************************
    文件：AchievementSystem.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/24 10:21:58
    功能：成就系统
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementSystem : IGameSystem
{
    private int mEnemykilledCount = 0;
    private int mSoldierKilledCount = 0;
    private int mMaxStageLv = 1;

    public override void Init()
    {
        base.Init();
        mFacade.RegisterObserver(GameEventType.EnemyKilled, new EnemyKilledObserverAchievement(this));
        mFacade.RegisterObserver(GameEventType.SoldierKilled, new SoldierKilledObserverAchivement(this));
        mFacade.RegisterObserver(GameEventType.NewStage, new NewStageObserverAchivement(this));
    }

    public void AddEnemyKilledCount(int number = 1)
    {
        mEnemykilledCount += number;
    }

    public void AddSoldierKilledCount(int number = 1)
    {
        mSoldierKilledCount += number;
    }

    public void SetMaxStageLv(int stageLv)
    {
        if (stageLv > mMaxStageLv)
        {
            mMaxStageLv = stageLv;
        }
    }

    public AchievementMemento CreateMemento()
    {
        AchievementMemento memento = new AchievementMemento();
        memento.enemykilledCount = mEnemykilledCount;
        memento.soldierKilledCount = mSoldierKilledCount;
        memento.maxStageLv = mMaxStageLv;
        return memento;
    }

    public void SetMemento(AchievementMemento memento)
    {
        mEnemykilledCount = memento.enemykilledCount;
        mSoldierKilledCount = memento.soldierKilledCount;
        mMaxStageLv = memento.maxStageLv;
    }
}
