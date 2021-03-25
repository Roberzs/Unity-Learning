/****************************************************
    文件：ResourcesAssetFactory.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/9 13:54:41
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class TrainSoldierCommand : ITrainCommand
{
    SoldierType mSoldierType;
    WeaponType mWeaponType;
    Vector3 mPosition;
    int mLv;

    public TrainSoldierCommand(SoldierType soldierType, WeaponType weaponType, Vector3 position, int lv)
    {
        mSoldierType = soldierType;
        mWeaponType = weaponType;
        mPosition = position;
        mLv = lv;
    }

    protected override void Execude()
    {
        switch (mSoldierType)
        {
            case SoldierType.Rookie:
                FactoryManager.SoldierFactory.CreateCharacter<SoldierRookie>(mWeaponType, mPosition, mLv);
                break;
            case SoldierType.Sergeant:
                FactoryManager.SoldierFactory.CreateCharacter<SoldierSergeant>(mWeaponType, mPosition, mLv);
                break;
            case SoldierType.Captain:
                FactoryManager.SoldierFactory.CreateCharacter<SoldierCaptain>(mWeaponType, mPosition, mLv);
                break;
            default:
                Debug.LogError("无法根据战士类型" + mSoldierType + "创建战士");
                break;
        }
    }
}
