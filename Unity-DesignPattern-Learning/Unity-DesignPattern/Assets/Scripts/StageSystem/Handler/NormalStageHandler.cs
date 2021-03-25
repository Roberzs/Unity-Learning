/****************************************************
    文件：NormalStageHandler.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/22 13:56:56
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class NormalStageHandler: IStageHandler
{
    private EnemyType mEnemyType;
    private WeaponType mWeaponType;
    private int mCount;
    private Vector3 mPosition;

    // 敌人生成计时器以及生成数量
    private float mSpawnTime = 3.5f;
    private float mSpawnTimer = 0f;
    private int mCountSpawned = 0;

    public NormalStageHandler(StageSystem stageSystem, int lv, int countToFinished,EnemyType et, WeaponType wt,int count, Vector3 pos) : base(stageSystem, lv, countToFinished)
    {
        mEnemyType = et;
        mWeaponType = wt;
        mCount = count;
        mPosition = pos;

        mSpawnTimer = mSpawnTime;
    }

    protected override void UpdateStage()
    {
        base.UpdateStage();

        if (mCountSpawned < mCount)
        {
            mSpawnTimer -= Time.deltaTime;
            if (mSpawnTimer <= 0)
            {
                SpawnEnemy();
                mSpawnTimer = mSpawnTime;
            }
        }
    }

    // 生成敌人
    private void SpawnEnemy()
    {
        mCountSpawned++;

        switch (mEnemyType)
        {
            case EnemyType.Elf:
                FactoryManager.EnemyFactory.CreateCharacter<EnemyElf>(mWeaponType, mPosition);
                break;
            case EnemyType.Ogre:
                FactoryManager.EnemyFactory.CreateCharacter<EnemyOgre>(mWeaponType, mPosition);
                break;
            case EnemyType.Troll:
                FactoryManager.EnemyFactory.CreateCharacter<EnemyTroll>(mWeaponType, mPosition);
                break;
            default:
                Debug.Log("无法生成类型: " + mEnemyType + " 的敌人");
                break;
        }
    }
}
