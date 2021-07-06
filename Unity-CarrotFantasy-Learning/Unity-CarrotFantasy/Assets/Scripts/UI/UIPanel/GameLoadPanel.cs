/****************************************************
    文件：GameLoadPanel.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/13 10:20:17
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadPanel : BasePanel
{
    public override void InitPanel()
    {
        base.InitPanel();
        gameObject.SetActive(false);
    }

    public override void EnterPanel()
    {
        base.EnterPanel();
        gameObject.SetActive(true);
        transform.SetSiblingIndex(8);

    }
}
