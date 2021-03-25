using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** 状态信息显示UI */

public class GameStateInfoUI : IBaseUI
{
    private List<GameObject> mHearts;
    private Text mSoldierCount;
    private Text mEnemyCount;
    private Text mCurrentStage;
    private Button mPauseBtn;
    private GameObject mGameOverUI;
    private Button mBackMenuBtn;
    private Text mMessage;
    private Slider mEnergySlider;
    private Text mEnergyText;

    public override void Init()
    {
        base.Init();

        // 获取Canvas  在Canvas获取状态信息显示UI
        GameObject canvas = GameObject.Find("Canvas");
        mRootUI = UnityTool.FindChild(canvas, "GameStateInfoUI");

        mHearts = new List<GameObject>();
        GameObject heart1 = UnityTool.FindChild(mRootUI, "Heart1");
        GameObject heart2 = UnityTool.FindChild(mRootUI, "Heart2");
        GameObject heart3 = UnityTool.FindChild(mRootUI, "Heart3");
        mHearts.Add(heart1);
        mHearts.Add(heart2);
        mHearts.Add(heart3);

        mSoldierCount = UITool.FindChild<Text>(mRootUI, "SoldierCount");
        mEnemyCount = UITool.FindChild<Text>(mRootUI, "EnemyCount");
        mCurrentStage = UITool.FindChild<Text>(mRootUI, "StageCount");
        mPauseBtn = UITool.FindChild<Button>(mRootUI, "PauseBtn");
        mGameOverUI = UnityTool.FindChild(mRootUI, "GameOverUI");
        mBackMenuBtn = UITool.FindChild<Button>(mRootUI, "BackMenuBtn");
        mMessage = UITool.FindChild<Text>(mRootUI, "Message");
        mEnergySlider = UITool.FindChild<Slider>(mRootUI, "EnergySlider");
        mEnergyText = UITool.FindChild<Text>(mRootUI, "EnergyText");

        mGameOverUI.SetActive(false);
        mMessage.gameObject.SetActive(false);
    }
}
