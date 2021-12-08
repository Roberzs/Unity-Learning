/****************************************************
    文件：FubenWnd.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/12/8 23:40:28
    功能：Nothing
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FubenWnd : WindowRoot
{
    protected override void InitWnd()
    {
        base.InitWnd();
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}
