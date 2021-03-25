/****************************************************
    文件：CaptiveCamp.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/25 13:07:03
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class CaptiveCamp : ICamp
{
    private WeaponType mWeaponType = WeaponType.Gun;
    private EnemyType mEnemyType;

    public CaptiveCamp(GameObject gameObject, string name, string iconSprite,
        EnemyType enemyType, Vector3 position, float trainTime)
        : base(gameObject, name, iconSprite, SoldierType.Captive, position, trainTime)
    {
        energyCostStrategy = new SoldierEnergyCostStrategy();
        mEnemyType = enemyType;
        UpdateEnergyCost();
    }

    public override int Lv
    {
        get { return 1; }
    }

    public override WeaponType WeaponType
    {
        get { return mWeaponType; }
    }

    public override int EnergyCostCampUpgrade
    {
        get { return 0; }
    }

    public override int EnergyCostWeaponUpgrade
    {
        get { return 0; }
    }

    public override int EnergyCostTrain
    {
        get
        {
            return mEnergyCostTrain;
        }
    }

    public override void Train()
    {
        // 添加训练命令
        TrainCaptiveCommand cmd = new TrainCaptiveCommand(mEnemyType, mWeaponType, mPosition, 1);
        mCommands.Add(cmd);
    }

    public override void UpgradeCamp()
    {
        return;
    }

    public override void UpgradeWeapon()
    {
        return;
    }

    protected override void UpdateEnergyCost()
    {
        mEnergyCostTrain = energyCostStrategy.GetSoldierTrainCost(mSoldierType, 1);
    }
}
