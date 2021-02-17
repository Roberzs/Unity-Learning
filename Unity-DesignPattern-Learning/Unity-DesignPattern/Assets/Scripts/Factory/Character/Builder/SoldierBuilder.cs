/****************************************************
    文件：SoldierBuilder.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/10 23:23:50
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBuilder : ICharacterBuilder
{
    public SoldierBuilder(ICharacter character, System.Type t, WeaponType weaponType, Vector3 spawnPosition, int lv) : base(character, t, weaponType, spawnPosition, lv) { }

    public override void AddCharacterAttr()
    {

        CharacterBaseAttr baseAttr = FactoryManager.AttrFactory.GetCharacterBaseAttr(mT);

        mPrefabName = baseAttr.PrefabName;      // 角色模型

        ICharacterAttr attr = new SoldierAttr(new SoldierAttrStrategy(), mLv, baseAttr);
        mCharacter.Attr = attr;
    }

    public override void AddGameObject()
    {
        // 创建角色游戏物体（加载与实例化）
        GameObject characterGO = FactoryManager.AssetFactory.LodaSoldier(mPrefabName);   // 加载
        characterGO.transform.position = mSpawnPosition;
        mCharacter.GameObject = characterGO;
    }

    public override void AddWeapon()
    {
        // 添加武器
        IWeapon weaponGO = FactoryManager.WeaponFactory.CreateWeapon(mWeaponType);
        mCharacter.Weapon = weaponGO;
    }

    public override ICharacter GetResult()
    {
        return mCharacter;
    }
}
