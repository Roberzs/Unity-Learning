/****************************************************
    文件：WeaponFactory.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/10 17:13:09
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactory : IWeaponFactory
{
    public IWeapon CreateWeapon(WeaponType weaponType)
    {
        WeaponBaseAttr baseAttr = FactoryManager.AttrFactory.GetWeaponBaseAttr(weaponType);

        GameObject weaponGO = FactoryManager.AssetFactory.LoadWeapon(baseAttr.AssetName);

        IWeapon weapon = new WeaponGun(baseAttr, weaponGO);

        return weapon;
    }
}
