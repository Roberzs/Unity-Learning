using System;
using System.Collections.Generic;
using UnityEngine;

/** 转换条件 */
public enum EnemyTransition
{
    NullTansition = 0,
    CanAttack,
    LostSoldier
}

/** 状态ID */
public enum EnemyStateID
{
    NullState = 0,
    Chase,
    Attack
}

public abstract class IEnemyState
{
    protected Dictionary<EnemyTransition, EnemyStateID> mMap = new Dictionary<EnemyTransition, EnemyStateID>();
    protected EnemyStateID mStateID;
    protected ICharacter mCharacter;
    protected EnemyFSMSystem mFSM;

    public IEnemyState(EnemyFSMSystem fsm, ICharacter character)
    {
        mFSM = fsm;
        mCharacter = character;
    }

    public EnemyStateID StateID { get { return mStateID; } }

    /** 添加转换条件 */
    public void AddTransition(EnemyTransition trans, EnemyStateID id)
    {
        if (trans == EnemyTransition.NullTansition)
        {
            Debug.LogError("EnemyState Error: [添加] 转换条件不能为空"); return;
        }
        if (id == EnemyStateID.NullState)
        {
            Debug.LogError("EnemyState Error: [添加] 状态ID不能为空"); return;
        }
        if (mMap.ContainsKey(trans))
        {
            Debug.LogError("EnemyState Error: [添加] 转换条件" + trans + "已被添加"); return;
        }
        mMap.Add(trans, id);
    }

    /** 删除转换条件 */
    public void DeleteTransition(EnemyTransition trans)
    {
        if (mMap.ContainsKey(trans) == false)
        {
            Debug.LogError("EnemyState Error: [删除] 转换条件" + trans + "不存在"); return;
        }
        mMap.Remove(trans);
    }

    /** 根据转换条件获取状态ID */
    public EnemyStateID GetOutPutState(EnemyTransition trans)
    {
        if (mMap.ContainsKey(trans) == false)
        {
            Debug.LogError("EnemyState Error: [获取] 转换条件" + trans + "不存在");
            return EnemyStateID.NullState;
        }
        return mMap[trans];
    }

    public virtual void DoBeforeEntering() { }      // 进入状态 (初始化)
    public virtual void DoBeforeLeaving() { }       // 退出状态

    public abstract void Reason(List<ICharacter> targets);      // 判断是否状态转换
    public abstract void Act(List<ICharacter> targets);         // 当前状态需要处理的逻辑
}

