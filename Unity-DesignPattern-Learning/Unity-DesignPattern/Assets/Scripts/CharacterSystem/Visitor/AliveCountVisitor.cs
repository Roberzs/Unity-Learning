/****************************************************
    文件：AliveCountVisitor.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/24 22:34:51
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class AliveCountVisitor: ICharacterVisitor
{
    public int enemyCount { get; private set; }
    public int soldierCount { get; private set; }

    public void Reset()
    {
        enemyCount = 0;
        soldierCount = 0;
    }

    public override void VisitEnemy(IEnemy enemy)
    {
        if (enemy.IsKilled == false) enemyCount += 1;
    }

    public override void VisitSoldier(ISoldier soldier)
    {
        if (soldier.IsKilled == false) soldierCount += 1;
    }
}
