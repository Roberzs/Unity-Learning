using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    private Vector3 mTargetPosition;

    public EnemyChaseState(EnemyFSMSystem fsm, ICharacter character):base(fsm, character)
    {
        mStateID = EnemyStateID.Chase;
    }

    public override void DoBeforeEntering()
    {
        mTargetPosition = GameFacade.Instance.GetEnemyTargetPosition();
    }

    public override void Act(List<ICharacter> targets)
    {
        // 有敌人追击敌人 没有敌人追击最终目标
        if (targets != null && targets.Count > 0)
        {
            mCharacter.MoveTo(targets[0].Position);
        }
        else
        {
            mCharacter.MoveTo(mTargetPosition);
        }
    }

    public override void Reason(List<ICharacter> targets)
    {
        if (targets != null && targets.Count > 0)
        {
            // 检查敌人是否进入攻击范围
            float distance = Vector3.Distance(mCharacter.Position, targets[0].Position);
            if (distance <= mCharacter.AtkRang)
            {
                mCharacter.StopMove();
                mFSM.PerformTransition(EnemyTransition.CanAttack);
            }
        }
    }
}

