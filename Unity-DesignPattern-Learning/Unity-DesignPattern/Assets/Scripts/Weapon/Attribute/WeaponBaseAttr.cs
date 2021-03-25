/****************************************************
    文件：WeaponBaseAttr.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/16 0:19:40
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBaseAttr
{
    protected string mName;    // 武器名
    protected int mAtk;     // 攻击力
    protected float mAtkRange;      // 攻击范围
    protected string mAssetName;        // 武器模型资源名

    public WeaponBaseAttr(string name, int atk, float atkRange, string assetName)
    {
        mName = name;
        mAtk = atk;
        mAtkRange = atkRange;
        mAssetName = assetName;
    }

    public string Name { get { return mName; } }
    public int Atk { get { return mAtk; } }
    public float AtkRange { get { return AtkRange; } }
    public string AssetName { get { return mAssetName; } }
}
