using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** 兵营信息显示UI */

public class CampInfoUI : IBaseUI
{
    private ICamp mCamp;        // 当前选中的兵营

    private Image mCampIcon;
    private Text mCampName;
    private Text mCampLevel;
    private Text mWeaponLevel;
    private Button mCampUpgradeBtn;
    private Button mWeaponUpgradeBtn;
    private Button mTrainBtn;
    private Button mCancelTrainBtn;
    private Text mAliveCount;
    private Text mTrainingCount;
    private Text mTrainTime;

    public override void Init()
    {
        base.Init();

        // 获取Canvas  在Canvas获取兵营信息显示UI
        GameObject canvas = GameObject.Find("Canvas");
        mRootUI = UnityTool.FindChild(canvas, "CampInfoUI");

        mCampIcon = UITool.FindChild<Image>(mRootUI, "CampIcon");
        mCampName = UITool.FindChild<Text>(mRootUI, "CampName");
        mCampLevel = UITool.FindChild<Text>(mRootUI, "CampLv");
        mWeaponLevel = UITool.FindChild<Text>(mRootUI, "WeaponLv");
        mCampUpgradeBtn = UITool.FindChild<Button>(mRootUI, "CampUpgradeBtn");
        mWeaponUpgradeBtn = UITool.FindChild<Button>(mRootUI, "WeaponUpgradeBtn");
        mTrainBtn = UITool.FindChild<Button>(mRootUI, "TrainBtn");
        mCancelTrainBtn = UITool.FindChild<Button>(mRootUI, "CancelTrainBtn");
        mAliveCount = UITool.FindChild<Text>(mRootUI, "AliveCount");
        mTrainingCount = UITool.FindChild<Text>(mRootUI, "TrainingCount");
        mTrainTime = UITool.FindChild<Text>(mRootUI, "TrainTime");

        mTrainBtn.onClick.AddListener(OnTrainClick);
        mCancelTrainBtn.onClick.AddListener(OnCancelTrainClick);

        Hide();
    }

    public void ShowCampInfo(ICamp camp)
    {
        Show();
        mCamp = camp;

        mCampIcon.sprite = FactoryManager.AssetFactory.LoadSprite(camp.IconSprite);
        mCampName.text = camp.Name;
        mCampLevel.text = camp.Lv.ToString();
        ShowWeaponLevel(camp.WeaponType);
    }

    // 根据武器类型显示名称
    private void ShowWeaponLevel(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Gun:
                mWeaponLevel.text = "短枪";
                break;
            case WeaponType.Rifle:
                mWeaponLevel.text = "长枪";
                break;
            case WeaponType.Rocket:
                mWeaponLevel.text = "火箭";
                break;
            default:
                break;
        }
    }

    public void OnTrainClick()
    {
        // 判断能量是否足够

        mCamp.Train();
    }

    public void OnCancelTrainClick()
    {
        // 能量回收

        mCamp.CancelTrain();
    }
}
