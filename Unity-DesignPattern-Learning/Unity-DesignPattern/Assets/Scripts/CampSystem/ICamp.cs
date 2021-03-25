/****************************************************
    文件：ICamp.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/19 23:12:37
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICamp
{

    protected GameObject mGameObject;
    protected string mName;
    protected string mIconSprite;
    protected SoldierType mSoldierType;
    protected Vector3 mPosition;
    protected float mTrainTime;

    protected List<ITrainCommand> mCommands;
    private float mTrainTimer = 0f;

    // 策略模式
    protected IEnergyCostStrategy energyCostStrategy;
    protected int mEnergyCostCampUpgrade;
    protected int mEnergyCostWeaponUpgrade;
    protected int mEnergyCostTrain;

    public ICamp(GameObject gameObject, string name, string iconSprite, SoldierType soldierType, Vector3 position, float trainTime)
    {
        mGameObject = gameObject;
        mName = name;
        mIconSprite = iconSprite;
        mSoldierType = soldierType;
        mPosition = position;
        mTrainTime = trainTime;

        mTrainTimer = trainTime;

        mCommands = new List<ITrainCommand>();
    }

    public virtual void Update()
    {
        UpdateCommand();
    }

    // 更新命令
    private void UpdateCommand()
    {
        if (mCommands.Count <= 0) return;
        mTrainTimer -= Time.deltaTime;
        if (mTrainTimer <= 0)
        {
            mCommands[0].Execude();
            mCommands.RemoveAt(0);
            mTrainTimer = mTrainTime;
        }
    }

    // 更新策略
    protected abstract void UpdateEnergyCost();

    public string Name { get { return mName; } }
    public string IconSprite { get { return mIconSprite; } }

    public abstract int Lv { get; }

    public abstract WeaponType WeaponType { get; }

    public int TrainCount { get { return mCommands.Count; } }

    public float RemainingTrainTime { get { return mTrainTimer; } }


    // 各项升级所需能量返回值
    public abstract int EnergyCostCampUpgrade { get; }
    public abstract int EnergyCostWeaponUpgrade { get; }
    public abstract int EnergyCostTrain { get; }

    public abstract void Train();
    public  void CancelTrain()
    {
        if (mCommands.Count > 0)
        {
            mCommands.RemoveAt(mCommands.Count - 1);
            // 如果被取消的命令为当前命令 重置训练等待时间
            if (mCommands.Count == 0)
            {
                mTrainTimer = mTrainTime;
            }
        }
    }

    public abstract void UpgradeCamp();
    public abstract void UpgradeWeapon();
}
