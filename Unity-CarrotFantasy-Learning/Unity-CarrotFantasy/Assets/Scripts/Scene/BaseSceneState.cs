/****************************************************
    文件：BaseSceneState.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/4 23:07:55
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneState : IBaseSceneState
{
    protected UIFacade mUIFacade;

    public BaseSceneState (UIFacade uIFacade)
    {
        mUIFacade = uIFacade;
    }

    public virtual void EnterScene()
    {
        mUIFacade.InitDict();
    }

    public virtual void ExitScene()
    {
        mUIFacade.ClearDict();
    }
}
