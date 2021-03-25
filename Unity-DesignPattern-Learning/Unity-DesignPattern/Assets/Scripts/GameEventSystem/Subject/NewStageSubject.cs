/****************************************************
    文件：NewStageSubject.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/24 9:46:22
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class NewStageSubject: IGameEventSubject
{
    private int mStageCount = 0;

    public int StageCount { get { return mStageCount; } }

    public override void Notify()
    {
        mStageCount++;
        base.Notify();
    }
}
