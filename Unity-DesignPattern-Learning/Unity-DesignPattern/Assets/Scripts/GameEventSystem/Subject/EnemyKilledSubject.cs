/****************************************************
    文件：EnemyKilledSubject.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/24 9:43:28
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKilledSubject: IGameEventSubject
{
    private int mKilledCount = 0;

    public int KilledCount { get { return mKilledCount; } }

    public override void Notify()
    {
        
        mKilledCount++;
        base.Notify();
    }
}
