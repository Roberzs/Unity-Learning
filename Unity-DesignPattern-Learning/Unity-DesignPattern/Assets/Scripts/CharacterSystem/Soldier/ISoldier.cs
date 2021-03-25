using System;
using System.Collections.Generic;

public enum SoldierType
{
    Rookie = 0,
    Sergeant = 1,
    Captain = 2,
    MAX = 3
}

public abstract class ISoldier : ICharacter
{
    protected SoldierFSMSystem mFSMSystem;

    public ISoldier() : base()
    {
        MakeFSM();
    }

    // 刷新状态机
    public override void UpdateFSMAI(List<ICharacter> targets)
    {
        mFSMSystem.CurrentState.Reason(targets);
        mFSMSystem.CurrentState.Act(targets);
    }

    private void MakeFSM()
    {
        mFSMSystem = new SoldierFSMSystem();

        SoldierIdleState idleState = new SoldierIdleState(mFSMSystem, this);
        idleState.AddTransition(SoldierTransition.SeeEnemy, SoldierStateID.Chase);

        SoldierChaseState chaseState = new SoldierChaseState(mFSMSystem, this);
        chaseState.AddTransition(SoldierTransition.NoEnemy, SoldierStateID.Idle);
        chaseState.AddTransition(SoldierTransition.CanAttack, SoldierStateID.Attack);

        SoldierAttackState attackState = new SoldierAttackState(mFSMSystem, this);
        attackState.AddTransition(SoldierTransition.NoEnemy, SoldierStateID.Idle);
        attackState.AddTransition(SoldierTransition.SeeEnemy, SoldierStateID.Chase);

        mFSMSystem.AddState(idleState, chaseState, attackState);
    }

    public override void UnderAttack(int damage)
    {
        base.UnderAttack(damage);

        if (mAttr.CurrentHP <= 0)
        {
            PlayEffect();
            PlaySound();
            Killed();
        }
    }

    protected abstract void PlayEffect();
    protected abstract void PlaySound();
}

