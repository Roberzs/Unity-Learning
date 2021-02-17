using System;
using System.Collections.Generic;

public class SoldierAttrStrategy : IAttrStrategy
{
    public int GetCritDmg(float critRate)
    {
        return 0;
    }

    public int GetDmgDescValue(int lv)
    {
        return (lv - 1) * 5;
    }

    public int GetExtraHPValue(int lv)
    {
        return (lv - 1) * 10;
    }
}

