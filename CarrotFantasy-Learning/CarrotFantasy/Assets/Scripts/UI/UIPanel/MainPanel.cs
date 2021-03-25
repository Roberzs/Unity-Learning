/****************************************************
    文件：MainPanel.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/12 19:36:46
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainPanel : BasePanel
{
    private Animator carrotAnimator;
    private Transform monsterTrans;
    private Transform cloudTrans;
    private Tween[] mainPanelTween;      // 自身切换动画数组 0 - 右移  1 - 左移
    private Tween exitTween;            // 自身切换动画

    protected override void Awake()
    {
        base.Awake();
        transform.SetSiblingIndex(1);
        carrotAnimator = transform.Find("Emp_Carrot").GetComponent<Animator>();
        monsterTrans = transform.Find("Img_Monster");
        cloudTrans = transform.Find("Img_Cloud");

        mainPanelTween = new Tween[2];
        mainPanelTween[0] = transform.DOLocalMoveX(1024, 0.5f);
        mainPanelTween[0].SetAutoKill(false);
        mainPanelTween[0].Pause();
        mainPanelTween[1] = transform.DOLocalMoveX(-1024, 0.5f);
        mainPanelTween[1].SetAutoKill(false);
        mainPanelTween[1].Pause();

        PlayUITween();
    }

    public override void EnterPanel()
    {
        if (exitTween != null)
        {
            exitTween.PlayBackwards();
        }
        cloudTrans.gameObject.SetActive(true);
    }

    public override void ExitPanel()
    {
        exitTween.PlayForward();
        cloudTrans.gameObject.SetActive(false);
    }

    // UI动画播放 
    private void PlayUITween()
    {
        monsterTrans.DOLocalMoveY(200, 2f).SetLoops(-1, LoopType.Yoyo);
        cloudTrans.DOLocalMoveX(650, 8f).SetLoops(-1, LoopType.Restart);
    }

    public void MoveToRight()
    {
        exitTween = mainPanelTween[0];
        mUIFacade.currentScenePanelDict[StringManager.SetPanel].EnterPanel();
    }

    public void MoveToLeft()
    {
        exitTween = mainPanelTween[1];
        mUIFacade.currentScenePanelDict[StringManager.HelpPanel].EnterPanel();
    }

    /** 场景状态切换 */
    public void ToNormalModelScene()
    {
        mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(new NormalGameOptionSceneState(mUIFacade));
    }

    public void ToBossModelScene()
    {
        mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(new BosslGameOptionSceneState(mUIFacade));
    }

    public void ToMonsterNest()
    {
        mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(new MonsterNestSceneState(mUIFacade));
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
