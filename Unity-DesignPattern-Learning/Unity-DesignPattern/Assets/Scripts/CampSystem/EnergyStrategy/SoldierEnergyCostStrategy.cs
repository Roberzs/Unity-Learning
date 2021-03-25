/****************************************************
    文件：SoldierEnergyCostStrategy.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/21 11:52:33
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierEnergyCostStrategy : IEnergyCostStrategy
{
    public override int GetCampUpgradeCost(SoldierType st, int lv)
    {
        int energy = 0;

        switch (st)
        {
            case SoldierType.Rookie:
                energy = 60;
                break;
            case SoldierType.Sergeant:
                energy = 65;
                break;
            case SoldierType.Captain:
                energy = 70;
                break;
            default:
                Debug.LogError("无法获取战士类型为" + st + "的兵营升级所需能量值");
                break;
        }
        energy += (lv - 1) * 2;
        if (energy > 100) energy = 100;
        return energy;
    }

    public override int GetSoldierTrainCost(SoldierType st, int lv)
    {
        int energy = 0;
        switch (st)
        {
            case SoldierType.Rookie:
                energy = 10;
                break;
            case SoldierType.Sergeant:
                energy = 15;
                break;
            case SoldierType.Captain:
                energy = 20;
                break;
            case SoldierType.Captive:
                return 12;
            default:
                Debug.LogError("无法获取战士类型为" + st + "的战士训练所需能量值");
                break;
        }
        energy += (lv - 1) * 2;
        return energy;
    }

    public override int GetWeaponUpgradeCost(WeaponType wt)
    {
        int energy = 0;
        switch (wt)
        {
            case WeaponType.Gun:
                energy = 30;
                break;
            case WeaponType.Rifle:
                energy = 40;
                break;
            default:
                // Debug.LogError("无法获取武器类型为" + wt + "的武器升级所需能量值");
                break;
        }
        return energy;
    }
}
