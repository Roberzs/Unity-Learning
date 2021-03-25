/****************************************************
    文件：ICharacterBuilder.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/10 23:20:00
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICharacterBuilder
{
    protected ICharacter mCharacter;

    protected System.Type mT;
    protected WeaponType mWeaponType;
    protected Vector3 mSpawnPosition;
    protected int mLv;

    protected string mPrefabName = "";      // 存储要创建的物体名(有瑕疵)

    public ICharacterBuilder(ICharacter character, System.Type t, WeaponType weaponType, Vector3 spawnPosition, int lv)
    {
        mCharacter = character;
        mT = t;
        mWeaponType = weaponType;
        mSpawnPosition = spawnPosition;
        mLv = lv;
    }

    public abstract void AddCharacterAttr();
    public abstract void AddGameObject();
    public abstract void AddWeapon();

    public abstract ICharacter GetResult();
}
