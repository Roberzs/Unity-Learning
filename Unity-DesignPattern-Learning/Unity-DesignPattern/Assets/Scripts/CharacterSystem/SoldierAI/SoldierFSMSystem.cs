using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFSMSystem
{
    private List<ISoldierState> mStates = new List<ISoldierState>();

    private ISoldierState mCurrentState;
    public ISoldierState CurrentState { get { return mCurrentState; } }

    public void AddState(params ISoldierState[] states)
    {
        foreach (var s in states)
        {
            AddState(s);
        }
    }

    public void AddState(ISoldierState state)
    {
        if (state == null)
        {
            Debug.LogError("SoldierFSMSystem Error: 要添加的状态为空"); return;
        }
        if (mStates.Count == 0)
        {
            mStates.Add(state);
            mCurrentState = state;
            return;
        }
        foreach (ISoldierState s in mStates)
        {
            if (s.StateID == state.StateID)
            {
                Debug.LogError("SoldierFSMSystem Error: 要添加的状态" + state.StateID + "已被添加"); return;
            }
        }
        mStates.Add(state);
    }

    public void DeleteState(SoldierStateID stateID)
    {
        if (stateID == SoldierStateID.NullState)
        {
            Debug.LogError("SoldierFSMSystem Error: 要删除的状态为空"); return;
        }
        foreach (ISoldierState s in mStates)
        {
            if (s.StateID == stateID)
            {
                mStates.Remove(s); return;
            }
        }
        Debug.LogError("SoldierFSMSystem Error: 要删除的状态" + stateID + "不存在"); return;

    }

    public void PerformTransition(SoldierTransition trans)
    {
        if (trans == SoldierTransition.NullTansition)
        {
            Debug.LogError("SoldierFSMSystem Error: 要执行的转换条件" + trans + "不存在"); return;
        }
        SoldierStateID nextStateID = mCurrentState.GetOutPutState(trans);
        if (nextStateID == SoldierStateID.NullState)
        {
            Debug.LogError("SoldierFSMSystem Error: 转换条件" + trans + "下，没有对应的状态"); return;
        }
        foreach (ISoldierState s in mStates)
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

