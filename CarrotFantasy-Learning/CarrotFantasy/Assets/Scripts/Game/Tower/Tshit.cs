/****************************************************
    文件：Tshit.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/25 15:41:18
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class Tshit : TowerPersonalProperty
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (GameController.Instance.isPause || targetTrans == null || GameController.Instance.gameOver) return;

        // 攻击方法
        if (timeVal >= attackCD / GameController.Instance.gameSpeed)
        {
            timeVal = 0;
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
    }
}
