/****************************************************
    文件：SoldierCaptive.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/25 11:28:05
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierCaptive : ISoldier
{
    private IEnemy mEnemy;

    public SoldierCaptive(IEnemy enemy)
    {
        mEnemy = enemy;

        ICharacterAttr attr = new SoldierAttr(enemy.Attr.Strategy, 1, enemy.Attr.BaseAttr);
        this.Attr = attr;

        this.GameObject = mEnemy.GameObject;

        this.Weapon = mEnemy.Weapon;
    }

    protected override void PlayEffect()
    {
        mEnemy.PlayEffect();
    }

    protected override void PlaySound()
    {
        // Nothing
    }
}
