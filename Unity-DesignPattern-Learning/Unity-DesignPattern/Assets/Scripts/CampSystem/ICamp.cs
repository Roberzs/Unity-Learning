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

    public ICamp(GameObject gameObject, string name, string iconSprite, SoldierType soldierType, Vector3 position, float trainTime)
    {
        mGameObject = gameObject;
        mName = name;
        mIconSprite = iconSprite;
        mSoldierType = soldierType;
        mPosition = position;
        mTrainTime = trainTime;

        mCommands = new List<ITrainCommand>();
    }

    public virtual void Update()
    {

    }

    public string Name { get { return mName; } }
    public string IconSprite { get { return mIconSprite; } }

    public abstract int Lv { get; }
    public abstract WeaponType WeaponType { get; }

    public abstract void Train();
    public abstract void CancelTrain();
}
