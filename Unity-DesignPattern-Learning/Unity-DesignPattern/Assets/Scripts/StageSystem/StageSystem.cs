/****************************************************
    文件：StageSystem.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/22 13:56:56
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class StageSystem : IGameSystem
{

    private int mLv = 1;
    private List<Vector3> mPosList;
    private Vector3 mTargetPosition;
    private IStageHandler mRootHandler;
    private int mCountOfEnemyKilled = 0;        

    public override void Init()
    {
        base.Init();

        InitPosition();
        InitStageChain();

        mFacade.RegisterObserver(GameEventType.EnemyKilled, new EnemyKilledObserverStage(this));
    }

    public override void Update()
    {
        base.Update();
        mRootHandler.Handle(mLv);
    }

    private void InitPosition()
    {
        mPosList = new List<Vector3>();
        int num = 1;
        while (true)
        {
            GameObject go = GameObject.Find("EnemySpawnPos" + num++);
            if (go != null)
            {
                mPosList.Add(go.transform.position);
                go.SetActive(false);
            }
            else
            {
                break;
            }
        }

        GameObject targetGo = GameObject.Find("TargetPosition");
        mTargetPosition = targetGo.transform.position;
        targetGo.SetActive(false);
    }

    // 返回敌人目标位置
    public Vector3 GetEnemyTargetPosition
    {
        get { return mTargetPosition; }
    }

    private Vector3 GetRandomPos()
    {
        return mPosList[UnityEngine.Random.Range(0, mPosList.Count)];
    }

    private void InitStageChain()
    {
        int lv = 1;
        IStageHandler handler1 = new NormalStageHandler(this, lv++, 3, EnemyType.Elf, WeaponType.Gun, 3, GetRandomPos());
        IStageHandler handler2 = new NormalStageHandler(this, lv++, 7, EnemyType.Elf, WeaponType.Rifle, 4, GetRandomPos());
        IStageHandler handler3 = new NormalStageHandler(this, lv++, 12, EnemyType.Elf, WeaponType.Rocket, 5, GetRandomPos());
        IStageHandler handler4 = new NormalStageHandler(this, lv++, 15, EnemyType.Ogre, WeaponType.Gun, 3, GetRandomPos());
        IStageHandler handler5 = new NormalStageHandler(this, lv++, 19, EnemyType.Ogre, WeaponType.Rifle, 4, GetRandomPos());
        IStageHandler handler6 = new NormalStageHandler(this, lv++, 24, EnemyType.Ogre, WeaponType.Rocket, 5, GetRandomPos());
        IStageHandler handler7 = new NormalStageHandler(this, lv++, 27, EnemyType.Troll, WeaponType.Gun, 3, GetRandomPos());
        IStageHandler handler8 = new NormalStageHandler(this, lv++, 31, EnemyType.Troll, WeaponType.Rifle, 4, GetRandomPos());
        IStageHandler handler9 = new NormalStageHandler(this, lv++, 35, EnemyType.Troll, WeaponType.Rocket, 5, GetRandomPos());

        handler1.SetNextHandler(handler2)
            .SetNextHandler(handler3)
            .SetNextHandler(handler4)
            .SetNextHandler(handler5)
            .SetNextHandler(handler6)
            .SetNextHandler(handler7)
            .SetNextHandler(handler8)
            .SetNextHandler(handler9);

        mRootHandler = handler1;
    }

    // 返回已死亡的敌人数量
    public int GetCountOfEnemyKilled()
    {
        return mCountOfEnemyKilled;
    }

    public int CountOfEnemyKilled { set { mCountOfEnemyKilled = value; } }

    // 进入下一关卡
    public void EnterNextStage()
    {
        mLv++;

        mFacade.UpdateCurrentStage(mLv);
        mFacade.NotifySubject(GameEventType.NewStage);
    }


}
