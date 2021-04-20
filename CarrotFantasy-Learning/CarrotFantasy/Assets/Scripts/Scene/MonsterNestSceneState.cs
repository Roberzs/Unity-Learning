/****************************************************
    文件：MonsterNestSceneState.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterNestSceneState : BaseSceneState
{
    public MonsterNestSceneState(UIFacade uIFacade) : base(uIFacade)
    {
    }

    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.GameLoadPanel);
        mUIFacade.AddPanelToDict(StringManager.MonsterNestPanel);
        base.EnterScene();
        GameManager.Instance.audioSourceManager.PlayBGMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("MonsterNest/BGMusic"));
    }

    public override void ExitScene()
    {
        
        SceneManager.LoadScene(1);
        base.ExitScene();
    }
}