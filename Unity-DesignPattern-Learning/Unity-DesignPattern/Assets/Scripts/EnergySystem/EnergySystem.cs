/****************************************************
    文件：EnergySystem.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/21 11:52:33
    功能：能量系统
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
public class EnergySystem : IGameSystem
{
    private const int MAX_ENERGY = 100;
    private float mNowEnergy = MAX_ENERGY;

    private float mRecoverSpeed = 3;

    public override void Init()
    {
        base.Init();
    }

    public override void Update()
    {
        base.Update();

        mFacade.UpdateEnergySlider((int)mNowEnergy, MAX_ENERGY);

        if (mNowEnergy >= MAX_ENERGY) return;

        mNowEnergy += mRecoverSpeed * Time.deltaTime;

        mNowEnergy = Math.Min(mNowEnergy, MAX_ENERGY);
    }

    public bool TakeEnergy(int value)
    {
        if (mNowEnergy >= value)
        {
            mNowEnergy -= value;
            return true;
        }
        return false;
    }

    public void RecycleEnery(int value)
    {
        mNowEnergy += value;
        mNowEnergy = Math.Min(mNowEnergy, MAX_ENERGY);
    }
}
