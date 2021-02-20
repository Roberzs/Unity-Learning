using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** 游戏暂停UI */

public class GamePauseUI : IBaseUI
{
    private Text currentStageLv;
    private Button continueBtn;
    private Button backMenuBtn;

    public override void Init()
    {
        base.Init();

        // 获取Canvas  在Canvas获取暂停UI
        GameObject canvas = GameObject.Find("Canvas");
        mRootUI = UnityTool.FindChild(canvas, "GamePauseUI");

        currentStageLv = UITool.FindChild<Text>(mRootUI, "CurrentStageLv");
        continueBtn = UITool.FindChild<Button>(mRootUI, "ContinueBtn");
        backMenuBtn = UITool.FindChild<Button>(mRootUI, "BackMenuBtn");

        Hide();
    }
}
