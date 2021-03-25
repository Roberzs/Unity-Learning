/****************************************************
    文件：HelpPanel.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/13 8:52:03
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class HelpPanel : BasePanel
{
    private GameObject helpPageGo;
    private GameObject monsterPageGo;
    private GameObject towerPageGo;
    private SlideScrollView slideScrollView;
    private SlideCanCoverSrollView slideCanCoverSrollView;
    private Tween helpPanelTween;

    protected override void Awake()
    {
        base.Awake();
        helpPageGo = transform.Find("HelpPage").gameObject;
        monsterPageGo = transform.Find("MonsterPage").gameObject;
        towerPageGo = transform.Find("TowerPage").gameObject;
        slideCanCoverSrollView = helpPageGo.transform.Find("Scroll View").GetComponent<SlideCanCoverSrollView>();
        slideScrollView = towerPageGo.transform.Find("Scroll View").GetComponent<SlideScrollView>();

        helpPanelTween = transform.DOLocalMoveX(0, 0.5f);
        helpPanelTween.SetAutoKill(false);
        helpPanelTween.Pause();
    }

    public override void InitPanel()
    {
        base.InitPanel();
        transform.localPosition = new Vector3(1024, 0, 0);
        transform.SetSiblingIndex(5);

        slideScrollView.Init();
        slideCanCoverSrollView.Init();

        ShowHelpPage();

        
    }

    public override void EnterPanel()
    {
        base.EnterPanel();
        gameObject.SetActive(true);

        slideScrollView.Init();
        slideCanCoverSrollView.Init();

        helpPanelTween.PlayForward();

        
    }

    public override void ExitPanel()
    {
        base.ExitPanel();

        if (mUIFacade.currentSceneState.GetType() == typeof(NormalGameOptionSceneState))
        {
            // 先将面板隐藏倒放一下 否则将无法执行正播事件
            gameObject.SetActive(false);
            helpPanelTween.PlayBackwards();

            // 冒险模式选择场景
            mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
            mUIFacade.ChangeSceneState(new MainSceneState(mUIFacade));
            SceneManager.LoadScene(1);
        }
        else
        {
            // 主场景操作
            helpPanelTween.PlayBackwards();
            mUIFacade.currentScenePanelDict[StringManager.MainPanel].EnterPanel();
        }
    }

    // 页面切换
    public void ShowHelpPage()
    {
        helpPageGo.SetActive(true);
        monsterPageGo.SetActive(false);
        towerPageGo.SetActive(false);
    }

    public void ShowMonsterPage()
    {
        helpPageGo.SetActive(false);
        monsterPageGo.SetActive(true);
        towerPageGo.SetActive(false);
    }

    public void ShowTowerPage()
    {
        helpPageGo.SetActive(false);
        monsterPageGo.SetActive(false);
        towerPageGo.SetActive(true);
    }
}
