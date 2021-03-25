using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Elf = 0,
    Ogre = 1,
    Troll = 2,
}

public abstract class IEnemy : ICharacter
{
    protected EnemyFSMSystem mFSMSystem;

    public IEnemy() : base()
    {
        MakeFSM();
    }

    // 刷新状态机
    public override void UpdateFSMAI(List<ICharacter> targets)
    {
        if (mIsKilled) return;
        mFSMSystem.CurrentState.Reason(targets);
        mFSMSystem.CurrentState.Act(targets);
    }

    public override void RunVisitor(ICharacterVisitor visitor)
    {
        visitor.VisitEnemy(this);
    }

    private void MakeFSM()
    {
        mFSMSystem = new EnemyFSMSystem();

        EnemyChaseState chaseState = new EnemyChaseState(mFSMSystem, this);
        chaseState.AddTransition(EnemyTransition.CanAttack, EnemyStateID.Attack);

        EnemyAttackState attackState = new EnemyAttackState(mFSMSystem, this);
        attackState.AddTransition(EnemyTransition.LostSoldier, EnemyStateID.Chase);

        mFSMSystem.AddState(chaseState, attackState);
    }

    public override void UnderAttack(int damage)
    {
        if (mIsKilled) return;

        base.UnderAttack(damage);
        PlayEffect();

        if (mAttr.CurrentHP <= 0 )
        {    
            Killed();
        }
    }

    public abstract void PlayEffect();   // 特效的播放

    public override void Killed()
    {
        base.Killed();

        GameFacade.Instance.NotifySubject(GameEventType.EnemyKilled);
    }
}

