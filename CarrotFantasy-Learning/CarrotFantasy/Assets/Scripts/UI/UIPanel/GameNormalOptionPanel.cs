/****************************************************
    文件：GameNormalOptionPanel.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/13 13:45:15
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class GameNormalOptionPanel : BasePanel
{
    [HideInInspector]
    public bool isInBigLevelPanel = true;

    public void ReturnToLastPanel()
    {
        

        if (isInBigLevelPanel)
        {
            mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
            mUIFacade.ChangeSceneState(new MainSceneState(mUIFacade));
        }
        else
        {
            mUIFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel].ExitPanel();
            mUIFacade.currentScenePanelDict[StringManager.GameNormalBigLevelPanel].EnterPanel();
        }
        isInBigLevelPanel = true;
    }

    public void ToHelpPanel()
    {
        mUIFacade.currentScenePanelDict[StringManager.HelpPanel].EnterPanel();
    }
}
