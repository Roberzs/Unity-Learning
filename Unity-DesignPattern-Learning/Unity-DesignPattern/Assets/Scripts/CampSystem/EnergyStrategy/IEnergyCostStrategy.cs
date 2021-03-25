/****************************************************
    文件：IEnergyCostStrategy.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/21 11:48:09
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class IEnergyCostStrategy
{
    public abstract int GetCampUpgradeCost(SoldierType st, int lv);
    public abstract int GetWeaponUpgradeCost(WeaponType wt);
    public abstract int GetSoldierTrainCost(SoldierType st, int lv);
}
