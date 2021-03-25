using System;
using System.Collections.Generic;
using UnityEngine;


public class EnemyFSMSystem
{
    private List<IEnemyState> mStates = new List<IEnemyState>();

    private IEnemyState mCurrentState;
    public IEnemyState CurrentState { get { return mCurrentState; } }

    public void AddState(params IEnemyState[] states)
    {
        foreach (var s in states)
        {
            AddState(s);
        }
    }

    public void AddState(IEnemyState state)
    {
        if (state == null)
        {
            Debug.LogError("EnemyFSMSystem Error: 要添加的状态为空"); return;
        }
        if (mStates.Count == 0)
        {
            mStates.Add(state);
            mCurrentState = state;
            mCurrentState.DoBeforeEntering();
            return;
        }
        foreach (IEnemyState s in mStates)
        {
            if (s.StateID == state.StateID)
            {
                Debug.LogError("EnemyFSMSystem Error: 要添加的状态" + state.StateID + "已被添加"); return;
            }
        }
        mStates.Add(state);
    }

    public void DeleteState(EnemyStateID stateID)
    {
        if (stateID == EnemyStateID.NullState)
        {
            Debug.LogError("EnemyFSMSystem Error: 要删除的状态为空"); return;
        }
        foreach (IEnemyState s in mStates)
        {
            if (s.StateID == stateID)
            {
                mStates.Remove(s); return;
            }
        }
        Debug.LogError("EnemyFSMSystem Error: 要删除的状态" + stateID + "不存在"); return;

    }

    public void PerformTransition(EnemyTransition trans)
    {
        if (trans == EnemyTransition.NullTansition)
        {
            Debug.LogError("EnemyFSMSystem Error: 要执行的转换条件" + trans + "不存在"); return;
        }
        EnemyStateID nextStateID = mCurrentState.GetOutPutState(trans);
        if (nextStateID == EnemyStateID.NullState)
        {
            Debug.LogError("EnemyFSMSystem Error: 转换条件" + trans + "下，没有对应的状态"); return;
        }
        foreach (IEnemyState s in mStates)
        {
            if (s.StateID == nextStateID)
            {
                mCurrentState.DoBeforeLeaving();
                mCurrentState = s;
                mCurrentState.DoBeforeEntering();
            }
        }
    }
}

