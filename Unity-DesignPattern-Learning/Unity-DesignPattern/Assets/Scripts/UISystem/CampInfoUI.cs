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
    private Text mTrainBtnText;
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
        mTrainBtnText = UITool.FindChild<Text>(mRootUI, "TrainBtnText");
        mCancelTrainBtn = UITool.FindChild<Button>(mRootUI, "CancelTrainBtn");
        mAliveCount = UITool.FindChild<Text>(mRootUI, "AliveCount");
        mTrainingCount = UITool.FindChild<Text>(mRootUI, "TrainingCount");
        mTrainTime = UITool.FindChild<Text>(mRootUI, "TrainTime");

        mTrainBtn.onClick.AddListener(OnTrainClick);
        mCancelTrainBtn.onClick.AddListener(OnCancelTrainClick);
        mCampUpgradeBtn.onClick.AddListener(OnCampUpgradeClick);
        mWeaponUpgradeBtn.onClick.AddListener(OnWeaponUpgradeClick);


        Hide();
    }

    public override void Update()
    {
        base.Update();

        if (mCamp != null)
        {
            ShowTrainingInfo();
        }
    }

    public void ShowCampInfo(ICamp camp)
    {
        Show();
        mCamp = camp;

        mCampIcon.sprite = FactoryManager.AssetFactory.LoadSprite(camp.IconSprite);
        mCampName.text = camp.Name;
        mCampLevel.text = camp.Lv.ToString();
        ShowWeaponLevel(camp.WeaponType);
        mTrainBtnText.text = "训练\n" + mCamp.EnergyCostTrain + "点能量";

        ShowTrainingInfo();
    }

    // 更新显示训练状态
    private void ShowTrainingInfo()
    {
        mTrainingCount.text = mCamp.TrainCount.ToString();
        mTrainTime.text = mCamp.RemainingTrainTime.ToString("0.00");
        if (mCamp.TrainCount == 0)
        {
            mCancelTrainBtn.interactable = false;
        }
        else
        {
            mCancelTrainBtn.interactable = true;
        }
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

    // 训练按钮点击事件
    public void OnTrainClick()
    {
        // 判断能量是否足够
        int energy = mCamp.EnergyCostTrain;
        if (mFacade.TakeEnergy(energy))
        {
            mCamp.Train();
        }
        else
        {
            mFacade.ShowMsg("能量不足,训练士兵所需能量" + energy);
        }

        
    }

    // 取消训练按钮点击事件
    public void OnCancelTrainClick()
    {
        // 能量回收
        mFacade.RecycleEnery(mCamp.EnergyCostTrain);
        mCamp.CancelTrain();
    }

    // 兵营升级按钮点击事件
    private void OnCampUpgradeClick()
    {
        int energy = mCamp.EnergyCostCampUpgrade;
        if (energy < 0)
        {
            mFacade.ShowMsg("该兵营已升级到最大等级!");
            return;
        }
        if (mFacade.TakeEnergy(energy))
        {
            mCamp.UpgradeCamp();
            ShowCampInfo(mCamp);
        }
        else
        {
            mFacade.ShowMsg("能量不足,兵营升级所需能量" + energy);
        }
        
    }

    // 武器升级按钮点击事件
    private void OnWeaponUpgradeClick()
    {
        int energy = mCamp.EnergyCostWeaponUpgrade;
        if (energy < 0)
        {
            mFacade.ShowMsg("该兵营武器已升级到最大等级!");
            return;
        }
        if (mFacade.TakeEnergy(energy))
        {
            mCamp.UpgradeWeapon();
            ShowCampInfo(mCamp);
        }
        else
        {
            mFacade.ShowMsg("能量不足,武器升级所需能量" + energy);
        }
    }
}
