using System;
using System.Collections.Generic;
using UnityEngine;

/** 转换条件 */
public enum SoldierTransition
{
    NullTansition = 0,
    SeeEnemy,
    NoEnemy,
    CanAttack
}

/** 状态ID */
public enum SoldierStateID
{
    NullState = 0,
    Idle,
    Chase,
    Attack
}

public abstract class ISoldierState
{
    protected Dictionary<SoldierTransition, SoldierStateID> mMap = new Dictionary<SoldierTransition, SoldierStateID>();
    protected SoldierStateID mStateID;
    protected ICharacter mCharacter;
    protected SoldierFSMSystem mFSM;

    public ISoldierState(SoldierFSMSystem fsm, ICharacter character)
    {
        mFSM = fsm;
        mCharacter = character;
    }

    public SoldierStateID StateID { get { return mStateID; } }

    /** 添加转换条件 */
    public void AddTransition(SoldierTransition trans, SoldierStateID id)
    {
        if (trans == SoldierTransition.NullTansition)
        {
            Debug.LogError("SoldierState Error: [添加] 转换条件不能为空"); return;
        }
        if (id == SoldierStateID.NullState)
        {
            Debug.LogError("SoldierState Error: [添加] 状态ID不能为空"); return;
        }
        if (mMap.ContainsKey(trans))
        {
            Debug.LogError("SoldierState Error: [添加] 转换条件" + trans + "已被添加"); return;
        }
        mMap.Add(trans, id);
    }

    /** 删除转换条件 */
    public void DeleteTransition(SoldierTransition trans)
    {
        if (mMap.ContainsKey(trans) == false) 
        {
            Debug.LogError("SoldierState Error: [删除] 转换条件" + trans + "不存在"); return;
        }
        mMap.Remove(trans);
    }

    /** 根据转换条件获取状态ID */
    public SoldierStateID GetOutPutState (SoldierTransition trans)
    {
        if (mMap.ContainsKey(trans) == false)
        {
            Debug.LogError("SoldierState Error: [获取] 转换条件" + trans + "不存在");
            return SoldierStateID.NullState;
        }
        return mMap[trans];
    }

    public virtual void DoBeforeEntering() { }      // 进入状态 (初始化)
    public virtual void DoBeforeLeaving() { }       // 退出状态

    public abstract void Reason(List<ICharacter> targets);      // 判断是否状态转换
    public abstract void Act(List<ICharacter> targets);         // 当前状态需要处理的逻辑

}

