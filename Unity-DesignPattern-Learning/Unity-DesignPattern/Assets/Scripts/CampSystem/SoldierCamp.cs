/****************************************************
    文件：SoldierCamp.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/19 23:13:08
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierCamp : ICamp
{
    private const int MAX_LV = 4;       // 兵营最大等级
    private int mLv = 1;                // 兵营等级
    private WeaponType mWeaponType = WeaponType.Gun;

    public SoldierCamp(GameObject gameObject, string name, string iconSprite,
        SoldierType soldierType, Vector3 position, float trainTime,
        WeaponType weaponType = WeaponType.Gun, int lv = 1)
        : base(gameObject, name, iconSprite, soldierType, position, trainTime)
    {
        mLv = lv;
        mWeaponType = weaponType;
    }

    public override int Lv
    {
        get { return mLv; }
    }

    public override WeaponType WeaponType
    {
        get { return mWeaponType; }
    }

    public override void CancelTrain()
    {
        if (mCommands.Count > 0)
        {
            mCommands.RemoveAt(mCommands.Count - 1);
        }
    }

    public override void Train()
    {
        // 添加训练命令
        TrainSoldierCommand cmd = new TrainSoldierCommand(mSoldierType, mWeaponType, mPosition, mLv);
        mCommands.Add(cmd);
    }

    public override void Update()
    {
        base.Update();
    }

    
}
