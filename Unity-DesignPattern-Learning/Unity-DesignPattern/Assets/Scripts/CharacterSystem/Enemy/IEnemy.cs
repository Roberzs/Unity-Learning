using System;
using System.Collections.Generic;


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
        mFSMSystem.CurrentState.Reason(targets);
        mFSMSystem.CurrentState.Act(targets);
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
        base.UnderAttack(damage);

        PlayEffect();

        if (mAttr.CurrentHP <=0 )
        {
            Killed();
        }
    }

    protected abstract void PlayEffect();   // 特效的播放
    

}

