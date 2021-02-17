/****************************************************
    文件：AttrFactory.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/15 23:13:34
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class AttrFactory : IAttrFactory
{
    private Dictionary<Type, CharacterBaseAttr> mCharacterBaseAttrDict;

    private Dictionary<WeaponType, WeaponBaseAttr> mWeaponBaseAttrDict;

    public AttrFactory()
    {
        InitCharacterBaseAttr();
        InitWeaponBaseAttr();
    }

    private void InitCharacterBaseAttr()
    {
        mCharacterBaseAttrDict = new Dictionary<Type, CharacterBaseAttr>();
        mCharacterBaseAttrDict.Add(typeof(SoldierRookie), new CharacterBaseAttr("新人", 80, 2f, "RookieIcon", "Soldier2", 0));
        mCharacterBaseAttrDict.Add(typeof(SoldierSergeant), new CharacterBaseAttr("中士", 90, 2.5f, "SergeanIcon", "Soldier3", 0));
        mCharacterBaseAttrDict.Add(typeof(SoldierCaptain), new CharacterBaseAttr("上尉", 100, 3f, "CaptainIcon", "Soldier1", 0));
        mCharacterBaseAttrDict.Add(typeof(EnemyElf), new CharacterBaseAttr("精灵", 100, 3f, "ElfIcon", "Enemy1", 0.2f));
        mCharacterBaseAttrDict.Add(typeof(EnemyOgre), new CharacterBaseAttr("侏儒", 120, 4f, "OgreIcon", "Enemy2", 0.3f));
        mCharacterBaseAttrDict.Add(typeof(EnemyTroll), new CharacterBaseAttr("巨魔", 150, 1f, "TrollIcon", "Enemy3", 0.4f));
    }

    private void InitWeaponBaseAttr()
    {
        mWeaponBaseAttrDict = new Dictionary<WeaponType, WeaponBaseAttr>();
        mWeaponBaseAttrDict.Add(WeaponType.Gun, new WeaponBaseAttr("手枪", 20, 5, "WeaponGun"));
        mWeaponBaseAttrDict.Add(WeaponType.Rifle, new WeaponBaseAttr("长枪", 30, 7, "WeaponRifle"));
        mWeaponBaseAttrDict.Add(WeaponType.Rocket, new WeaponBaseAttr("火箭", 40, 8, "WeaponRocket"));
    }

    public CharacterBaseAttr GetCharacterBaseAttr(Type t)
    {
        if (mCharacterBaseAttrDict.ContainsKey(t) == false)
        {
            Debug.LogError("无法根据类型" + t + "得到角色基础属性");
            return null;
        }
        return mCharacterBaseAttrDict[t];
    }

    WeaponBaseAttr IAttrFactory.GetWeaponBaseAttr(WeaponType weaponType)
    {
        if (mWeaponBaseAttrDict.ContainsKey(weaponType) == false)
        {
            Debug.LogError("无法根据类型" + weaponType + "得到武器基础属性");
            return null;
        }
        return mWeaponBaseAttrDict[weaponType];
    }
}
