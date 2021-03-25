/****************************************************
    文件：Round.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/17 11:25:21
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class Round
{
    [System.Serializable]
    public struct RoundInfo
    {
        public int[] mMonsterIDList;
    }

    public RoundInfo roundInfo;

    protected Round mNextRound;
    protected int mRoundID;
    protected Level mLevel;

    public Round(int[] monsterIDist, int roundID, Level level)
    {
        mLevel = level;
        roundInfo.mMonsterIDList = monsterIDist;
        mRoundID = roundID;
    }

    public void SetNextRound(Round nextRound)
    {
        mNextRound = nextRound;
    }

    public void Handle(int roundID)
    {
        if (mRoundID < roundID)
        {
            mNextRound.Handle(roundID);
        }
        else
        {
            // 产生怪物  ( 将怪物列表传入GameController 执行CreateMonster() )
            GameController.Instance.mMonsterIDList = roundInfo.mMonsterIDList;
            GameController.Instance.CreateMonster();
        }
    }
}
