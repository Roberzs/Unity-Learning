using System;
using System.Collections.Generic;

/** 角色属性类 */
public class ICharacterAttr
{
    protected CharacterBaseAttr mBaseAttr;

    /** 使用策略模式计算 */
    protected int mLv;      // 角色等级(战士拥有 增加血量与伤害减免)
    protected int mCurrentHP;       // 角色当前血量
    protected int mDmgDescValue;        // 伤害减免 因为是固定值 所以提前定义

    protected IAttrStrategy mStrategy;

    public ICharacterAttr(IAttrStrategy strategy, int lv, CharacterBaseAttr baseAttr)
    {
        
        mLv = lv;

        mBaseAttr = baseAttr;

        mStrategy = strategy;
        mDmgDescValue = mStrategy.GetDmgDescValue(mLv);
        mCurrentHP = baseAttr.MaxHP + mStrategy.GetExtraHPValue(mLv);
    }

    public int CritValue { get { return mStrategy.GetCritDmg(mBaseAttr.CritRate); } }        // 返回暴击伤害
    public int CurrentHP { get { return mCurrentHP; } }     // 返回当前血量

    public void TakeDamage(int damage)
    {
        damage -= mDmgDescValue;
        if (damage < 5) damage = 5;     // 至少造成5点上海
        mCurrentHP -= damage;
    }
}

