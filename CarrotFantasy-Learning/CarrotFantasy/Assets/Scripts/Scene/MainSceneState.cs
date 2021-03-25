/****************************************************
    文件：MainSceneState.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneState : BaseSceneState
{
    public MainSceneState(UIFacade uIFacade) : base(uIFacade)
    {
    }

    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.MainPanel);
        mUIFacade.AddPanelToDict(StringManager.SetPanel);
        mUIFacade.AddPanelToDict(StringManager.HelpPanel);
        mUIFacade.AddPanelToDict(StringManager.GameLoadPanel);
        base.EnterScene();
    }

    public override void ExitScene()
    {
        base.ExitScene();
        // 判断要跳转的场景
        if (mUIFacade.currentSceneState.GetType() == typeof(NormalGameOptionSceneState))
        {
            SceneManager.LoadScene(2);
        }
        else if (mUIFacade.currentSceneState.GetType() == typeof(BosslGameOptionSceneState))
        {
            SceneManager.LoadScene(4);
        }
        else if (mUIFacade.currentSceneState.GetType() == typeof(MonsterNestSceneState))
        {
            SceneManager.LoadScene(6);
        }
    }
}