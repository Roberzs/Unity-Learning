/****************************************************
    文件：Start.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLoadSceneState : BaseSceneState
{
    public StartLoadSceneState(UIFacade uIFacade) : base(uIFacade)
    {

    }

    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.StartLoadPanel);
        base.EnterScene();
    }

    public override void ExitScene()
    {
        base.ExitScene();
        SceneManager.LoadScene(1);
    }
}