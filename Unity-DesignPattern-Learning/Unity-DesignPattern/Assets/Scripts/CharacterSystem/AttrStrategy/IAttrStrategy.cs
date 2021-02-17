using System;
using System.Collections.Generic;



public interface IAttrStrategy
{
    int GetExtraHPValue(int lv);        // 血量加成
    int GetDmgDescValue(int lv);        // 伤害减免
    int GetCritDmg(float critRate);     // 暴击伤害
}

