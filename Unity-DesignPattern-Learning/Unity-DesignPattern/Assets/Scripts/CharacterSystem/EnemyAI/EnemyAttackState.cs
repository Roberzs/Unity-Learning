using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{

    private float mAttackTime = 1;  // 攻击间隔
    private float mAttackTimer = 1; // 攻击间隔计时器

    public EnemyAttackState(EnemyFSMSystem fsm, ICharacter character ):base(fsm, character)
    {
        mStateID = EnemyStateID.Attack;
        mAttackTimer = mAttackTime;
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
        // 1 如果没有敌人存在 转换到追击状态
        if (targets == null || targets.Count == 0)
        {
            mFSM.PerformTransition(EnemyTransition.LostSoldier);
            return;
        }
        // 2 如果敌人距离大于攻击范围 转换到追击状态
        float distance = Vector3.Distance(mCharacter.Position, targets[0].Position);
        if (distance >= mCharacter.AtkRang)
        {
            mFSM.PerformTransition(EnemyTransition.LostSoldier);
        }
    }
}

