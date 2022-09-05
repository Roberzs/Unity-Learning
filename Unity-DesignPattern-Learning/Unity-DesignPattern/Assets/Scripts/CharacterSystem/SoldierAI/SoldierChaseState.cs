using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierChaseState : ISoldierState
{
    public SoldierChaseState(SoldierFSMSystem fsm, ICharacter character) : base(fsm, character)
    {
        mStateID = SoldierStateID.Chase;
    }

    public override void Act(List<ICharacter> targets)
    {
        if (targets != null && targets.Count > 0)
        {
            mCharacter.MoveTo(targets[0].Position);
        }
    }

    public override void Reason(List<ICharacter> targets)
    {
        if (targets == null && targets.Count == 0)
        {
            // 没有敌人
            mFSM.PerformTransition(SoldierTransition.NoEnemy); return;
        }

        // 检查敌人是否进入攻击范围
        float distance = Vector3.Distance(mCharacter.Position, targets[0].Position);
        
        if (distance <= mCharacter.AtkRang)
        {
            mCharacter.StopMove();
            mFSM.PerformTransition(SoldierTransition.CanAttack);
        }
    }
}

