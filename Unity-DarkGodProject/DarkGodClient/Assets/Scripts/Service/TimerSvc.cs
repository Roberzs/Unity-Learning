/****************************************************
    文件：TimerSvc.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/4/15 12:56:28
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerSvc : SystemRoot
{
    public static TimerSvc Instance = null;

    private PETimer pt;

    public void InitSvc()
    {
        Instance = this;
        pt = new PETimer();
        pt.SetLog((string info) => {
            PECommon.Log(info);
        });

        PECommon.Log("Init TimerSvc");
    }

    public void Update()
    {
        pt.Update();
    }

    public int AddTimeTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond,int count = 1)
    {
        return pt.AddTimeTask(callback, delay, timeUnit, count);
    }
}
