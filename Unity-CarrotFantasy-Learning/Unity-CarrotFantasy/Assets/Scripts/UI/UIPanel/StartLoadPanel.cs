/****************************************************
    文件：StartLoadPanel.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/12 18:57:13
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class StartLoadPanel : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
        Invoke("LoadNextScene", 2);
    }

    private void LoadNextScene()
    {
        mUIFacade.ChangeSceneState(new MainSceneState(mUIFacade));
    }
}
