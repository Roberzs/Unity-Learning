/****************************************************
    文件：TrainCaptiveCommand.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/25 12:42:06
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class TrainCaptiveCommand : ITrainCommand
{
    EnemyType mEnemyType;
    WeaponType mWeaponType;
    Vector3 mPosition;
    int mLv;

    public TrainCaptiveCommand(EnemyType enemyType, WeaponType weaponType, Vector3 position, int lv = 1)
    {
        mEnemyType = enemyType;
        mWeaponType = weaponType;
        mPosition = position;
        mLv = lv;
    }

    public override void Execude()
    {
        IEnemy enemy = null;
        switch (mEnemyType)
        {
            case EnemyType.Elf:
                enemy = FactoryManager.EnemyFactory.CreateCharacter<EnemyElf>(mWeaponType, mPosition, mLv) as IEnemy;
                break;
            case EnemyType.Ogre:
                enemy = FactoryManager.EnemyFactory.CreateCharacter<EnemyOgre>(mWeaponType, mPosition, mLv) as IEnemy;
                break;
            case EnemyType.Troll:
                enemy = FactoryManager.EnemyFactory.CreateCharacter<EnemyTroll>(mWeaponType, mPosition, mLv) as IEnemy;
                break;
            default:
                Debug.LogError("无法根据俘兵类型:" + mEnemyType + "创建俘兵");
                return;
        }
        GameFacade.Instance.RemoveEnemy(enemy);
        ISoldier captive = new SoldierCaptive(enemy);
        GameFacade.Instance.AddSoldier(captive);
    }
}
