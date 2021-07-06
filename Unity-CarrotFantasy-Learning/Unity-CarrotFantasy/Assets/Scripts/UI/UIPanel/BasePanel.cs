/****************************************************
    文件：BasePanel.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/12 9:55:45
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour, IBasePanel
{
    protected UIFacade mUIFacade;

    protected virtual void Awake()
    {
        mUIFacade = GameManager.Instance.uIManager.mUIFacade;
    }

    public virtual void EnterPanel()
    {
        
    }

    public virtual void ExitPanel()
    {

    }

    public virtual void InitPanel()
    {
    
    }

    public virtual void UpdatePanel()
    {
    
    }
}
