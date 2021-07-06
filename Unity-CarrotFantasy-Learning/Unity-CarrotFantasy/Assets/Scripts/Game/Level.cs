/****************************************************
    文件：Level.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/19 10:28:11
    功能：关卡控制者
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int totalRound;
    public Round[] roudList;
    public int currentRound;

    public Level(int roundNum, List<Round.RoundInfo> roundInfoList)
    {
        totalRound = roundNum;
        roudList = new Round[totalRound];
        // 对round数组的赋值
        for (int i = 0; i < totalRound; i++)
        {
            roudList[i] = new Round(roundInfoList[i].mMonsterIDList, i, this);
        }
        // 设置任务链
        for (int i = 0; i < totalRound; i++)
        {
            if (i == totalRound - 1)
            {
                break;
            }
            roudList[i].SetNextRound(roudList[i + 1]);
        }
    }

    public void HandleRound()
    {
        if (currentRound >= totalRound)
        {
            // 胜利处理
            currentRound--;
            GameController.Instance.normalModelPanel.ShowGameWinPage();
        }
        else if (currentRound == totalRound - 1)
        {
            // 最后一波怪物的具体处理（UI等）
            HandleLastRound();
            GameController.Instance.normalModelPanel.ShowFinalWaveUI();
        }
        else
        {
            roudList[currentRound].Handle(currentRound);
        }
    }

    // 最后一波怪的处理（Handle）
    public void HandleLastRound()
    {
        roudList[currentRound].Handle(currentRound);
    }

    // 波次增加
    public void AddRoundNum()
    {
        currentRound++;
    }

}
