/****************************************************
    文件：FubenSys.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/12/8 23:47:24
    功能：Nothing
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FubenSys : SystemRoot
{
    public static FubenSys Instance = null;

    public FubenWnd fubenWnd;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init FubenSys");
    }

    public void EnterFuben()
    {
        OpenFubenWnd();
    }

    public void OpenFubenWnd()
    {
        fubenWnd.SetWndState();
    }
}
