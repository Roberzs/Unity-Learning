using System;
using System.Collections.Generic;

/** 我方角色属性类 */
public class SoldierAttr : ICharacterAttr
{
    public SoldierAttr(IAttrStrategy strategy, int lv, CharacterBaseAttr baseAttr) : base(strategy, lv, baseAttr) { }
}

