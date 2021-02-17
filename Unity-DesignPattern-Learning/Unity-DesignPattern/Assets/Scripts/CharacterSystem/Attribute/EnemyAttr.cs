using System;
using System.Collections.Generic;


/** 敌人角色属性类 */
public class EnemyAttr : ICharacterAttr
{
    public EnemyAttr(IAttrStrategy strategy, int lv, CharacterBaseAttr baseAttr) : base(strategy, lv, baseAttr) { }
}

