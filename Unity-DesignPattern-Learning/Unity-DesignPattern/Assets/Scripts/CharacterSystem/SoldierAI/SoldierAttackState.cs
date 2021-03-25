using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAttackState : ISoldierState
{

    private float mAttackTime = 1;  // 攻击间隔
    private float mAttackTimer = 1; // 攻击间隔计时器


    public SoldierAttackState(SoldierFSMSystem fsm, ICharacter character) : base(fsm, character)
    {
        mStateID = SoldierStateID.Attack;
        mAttackTimer = mAttackTime;     // 攻击间隔器默认值为攻击间隔
    }

    public override void Act(List<ICharacter> targets)
    {
        if (targets == null || targets.Count == 0 || targets[0].IsKilled == true) return;

        mAttackTimer += Time.deltaTime;
        if (mAttackTimer >= mAttackTime)
        {
            mCharacter.Attack(targets[0]);
            mAttackTimer = 0;
        }
    }

    public override void Reason(List<ICharacter> targets)
    {
        if (targets == null || targets.Count == 0)
        {
            mFSM.PerformTransition(SoldierTransition.NoEnemy); return;
        }
        float distance = Vector3.Distance(mCharacter.Position, targets[0].Position);
        if (distance >= mCharacter.AtkRang)
        {
            mFSM.PerformTransition(SoldierTransition.SeeEnemy);
        }
    }
}

