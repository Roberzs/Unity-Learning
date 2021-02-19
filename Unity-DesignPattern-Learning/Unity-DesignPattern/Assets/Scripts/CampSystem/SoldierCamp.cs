/****************************************************
    文件：SoldierCamp.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/19 23:13:08
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierCamp : ICamp
{
    private const int MAX_LV = 4;       // 兵营最大等级
    private int mLv = 1;                // 兵营等级
    private WeaponType mWeaponType = WeaponType.Gun;

    public SoldierCamp(GameObject gameObject, string name, string iconSprite, SoldierType soldierType, Vector3 position, WeaponType weaponType, int lv) : base(gameObject, name, iconSprite, soldierType, position)
    {
        mLv = lv;
        mWeaponType = weaponType;
    }
}
