/****************************************************
    文件：NormalGameOptionSceneState.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class NormalGameOptionSceneState : BaseSceneState
{
    public NormalGameOptionSceneState(UIFacade uIFacade) : base(uIFacade)
    {
    }

    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.GameNormalOptionPanel);
        mUIFacade.AddPanelToDict(StringManager.GameNormalBigLevelPanel);
        mUIFacade.AddPanelToDict(StringManager.GameNormalLevelPanel);
        mUIFacade.AddPanelToDict(StringManager.HelpPanel);
        mUIFacade.AddPanelToDict(StringManager.GameLoadPanel);
        base.EnterScene();
    }

    public override void ExitScene()
    {
        base.ExitScene();
    }
}