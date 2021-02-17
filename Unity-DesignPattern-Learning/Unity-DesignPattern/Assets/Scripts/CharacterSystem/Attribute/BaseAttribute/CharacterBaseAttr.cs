/****************************************************
    文件：CharacterBaseAttr.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/15 23:07:36
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBaseAttr
{
    protected string mName;     // 角色名
    protected int mMaxHP;       // 角色最大生命值
    protected float mMoveSpeed;     // 角色移动速度
    protected string mIconSprite;       // 角色头像名
    protected string mPrefabName;       // 游戏物体名
    protected float mCritRate;      // 暴击率 0 - 1 (敌人拥有)

    public CharacterBaseAttr(string name, int maxHP, float moveSpeed, string iconSprite, string prefabName, float critRate)
    {
        mName = name;
        mMaxHP = maxHP;
        mMoveSpeed = moveSpeed;
        mIconSprite = iconSprite;
        mPrefabName = prefabName;
        mCritRate = critRate;
    }

    public string Name { get { return mName; } }
    public int MaxHP { get { return mMaxHP; } }
    public float MoveSpeed { get { return mMoveSpeed; } }
    public string IconSprite { get { return mIconSprite; } }
    public string PrefabName { get { return mPrefabName; } }
    public float CritRate { get { return mCritRate; } }

}
