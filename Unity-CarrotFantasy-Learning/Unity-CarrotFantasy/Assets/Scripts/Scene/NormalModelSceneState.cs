/****************************************************
    文件：NormalModelSceneState.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class NormalModelSceneState : BaseSceneState
{
    public NormalModelSceneState(UIFacade uIFacade) : base(uIFacade)
    {
    }

    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.NormalModelPanel);
        mUIFacade.AddPanelToDict(StringManager.GameLoadPanel);
        base.EnterScene();
        GameManager.Instance.audioSourceManager.CloseBGMusic();
    }

    public override void ExitScene()
    {
        base.ExitScene();
        GameManager.Instance.audioSourceManager.OpenBGMusic();
    }
}