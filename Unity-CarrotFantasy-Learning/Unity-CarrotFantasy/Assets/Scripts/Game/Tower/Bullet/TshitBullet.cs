/****************************************************
    文件：TshitBullet.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/25 15:48:03
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class TshitBullet : Bullet
{
    private BulletProperty bulletProperty;

    private void Start()
    {
        bulletProperty = new BulletProperty
        {
            debuffTime = towerLevel * 0.4f,
            debuffValue = towerLevel * 0.3f
        };
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.activeSelf) return;
        // 如果是敌人 对其套上Debuff
        if (collision.tag == "Monster")
        {
            collision.SendMessage("DecreaseSpeed",bulletProperty);
        }
        base.OnTriggerEnter2D(collision);
    }
}
