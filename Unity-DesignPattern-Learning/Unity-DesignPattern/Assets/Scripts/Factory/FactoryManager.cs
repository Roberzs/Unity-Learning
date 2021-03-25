/****************************************************
    文件：FactoryManager.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/10 17:47:44
    功能：工厂管理类 对所有工厂进行统一管理
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public static class FactoryManager
{
    private static IAssetFactory mAssetFactory = null;
    private static ICharacterFactory mSoldierFactory = null;
    private static ICharacterFactory mEnemyFactory = null;
    private static IWeaponFactory mWeaponFactory = null;
    private static IAttrFactory mAttrFactory = null;

    public static IAttrFactory AttrFactory
    {
        get
        {
            if (mAttrFactory == null)
            {
                mAttrFactory = new AttrFactory();
            }
            return mAttrFactory;
        }
    }

    public static IAssetFactory AssetFactory
    {
        get
        {
            if (mAssetFactory == null)
            {
                // mAssetFactory = new ResourcesAssetFactory();
                mAssetFactory = new ResourcesAssetProxyFactory();
            }
            return mAssetFactory;
        }
    }

    public static ICharacterFactory SoldierFactory
    {
        get
        {
            if (mSoldierFactory == null)
            {
                mSoldierFactory = new SoldierFactory();
            }
            return mSoldierFactory;
        }
    }

    public static ICharacterFactory EnemyFactory
    {
        get
        {
            if (mEnemyFactory == null)
            {
                mEnemyFactory = new EnemyFactory();
            }
            return mEnemyFactory;
        }
    }

    public static IWeaponFactory WeaponFactory
    {
        get
        {
            if (mWeaponFactory == null)
            {
                mWeaponFactory = new WeaponFactory();
            }
            return mWeaponFactory;
        }
    }
}
