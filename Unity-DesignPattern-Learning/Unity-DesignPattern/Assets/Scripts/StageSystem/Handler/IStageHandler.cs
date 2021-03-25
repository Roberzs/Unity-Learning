/****************************************************
    文件：IStageHandler.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/22 13:56:30
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class IStageHandler
{
    protected StageSystem mStageSystem;
    protected int mLv;
    protected int mCountToFinished;
    protected IStageHandler mNextHandler;

    public IStageHandler(StageSystem stageSystem, int lv, int countToFinished)
    {
        mStageSystem = stageSystem;
        mLv = lv;
        mCountToFinished = countToFinished;
    }

    public IStageHandler SetNextHandler(IStageHandler handler)
    {
        mNextHandler = handler;
        return mNextHandler;
    }

    public void Handle(int level)
    {
        if (level == mLv)
        {
            UpdateStage();
            CheckIsFinished();
        }
        else
        {
            mNextHandler.Handle(level);
        }
    }

    protected virtual void UpdateStage() { }

    private void CheckIsFinished()
    {
        if (mStageSystem.GetCountOfEnemyKilled() >= mCountToFinished)
        {
            mStageSystem.EnterNextStage();
        }
    }
}
