/****************************************************
    文件：GameNormalBigLevelPanel.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/13 13:53:55
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 *      笔记: 事件的赋值与执行顺序  -   Inspector > 外部赋值 > Awake > OnEnable > Start
 *            脚本对象的失活与激活不作用于Awake方法
 */

public class GameNormalBigLevelPanel : BasePanel
{
    public Transform bigLevelContentTrans;
    public int bigLevelPageCount = 3;
    private SlideScrollView slideScrollView;
    private PlayerManager playerManager;
    private Transform[] bigLevelPage;

    private bool hasRigisterEvent;      // 按钮是否已经注册事件

    protected override void Awake()
    {
        base.Awake();
        playerManager = mUIFacade.mPlayerManager;
        bigLevelPage = new Transform[bigLevelPageCount];
        slideScrollView = transform.Find("Scroll View").GetComponent<SlideScrollView>();

        for (int i = 0; i < bigLevelPageCount; i++)
        {
            bigLevelPage[i] = bigLevelContentTrans.GetChild(i);
            ShowBigLevelState(playerManager.unLockedNormalModelBigLevelList[i], playerManager.unLockedNormalModelLevelNum[i], 5, bigLevelPage[i], i + 1);
        }
        hasRigisterEvent = true;
    }

    public override void EnterPanel()
    {
        base.EnterPanel();
        slideScrollView.Init();
        gameObject.SetActive(true);

    }

    public override void ExitPanel()
    {
        base.ExitPanel();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {

        for (int i = 0; i < bigLevelPageCount; i++)
        {
            bigLevelPage[i] = bigLevelContentTrans.GetChild(i);
            ShowBigLevelState(playerManager.unLockedNormalModelBigLevelList[i], playerManager.unLockedNormalModelLevelNum[i], 5, bigLevelPage[i], i + 1);
        }
    }

    public void ShowBigLevelState(bool unLocked, int unLockedLevelNum, int totalNum, Transform theBigLevelButtonTrans, int bigLevelID)
    {
        

        if (unLocked)
        {
            // 解锁状态
            theBigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(false);
            theBigLevelButtonTrans.Find("Img_Page").gameObject.SetActive(true);
            theBigLevelButtonTrans.Find("Img_Page").Find("Txt_Page").GetComponent<Text>().text = unLockedLevelNum.ToString() + "/" + totalNum.ToString();
            Button theBigLevelButtonCom = theBigLevelButtonTrans.GetComponent<Button>();
            theBigLevelButtonCom.interactable = true;
            
            if (!hasRigisterEvent)
            {
                theBigLevelButtonCom.onClick.AddListener(() => {

                    mUIFacade.PlayButtonAudioClip();

                    // 离开大关卡选择页面
                    mUIFacade.currentScenePanelDict[StringManager.GameNormalBigLevelPanel].ExitPanel();
                    GameNormalOptionPanel gameNormalOptionPanel = mUIFacade.currentScenePanelDict[StringManager.GameNormalOptionPanel] as GameNormalOptionPanel;
                    gameNormalOptionPanel.isInBigLevelPanel = false;

                    // 进入小关卡选择页面
                    GameNormalLevelPanel gameNormalLevelPanel = mUIFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel] as GameNormalLevelPanel;
                    gameNormalLevelPanel.ToThisPanel(bigLevelID);
                    
                    
                });
            }
            
        }
        else
        {
            theBigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(true);
            theBigLevelButtonTrans.Find("Img_Page").gameObject.SetActive(false);
            theBigLevelButtonTrans.GetComponent<Button>().interactable = false;
        }
    }

    // 翻页事件
    public void ToNextPage()
    {
        mUIFacade.PlayButtonAudioClip();
        slideScrollView.ToNextPage();
    }

    public void ToLastPage()
    {
        mUIFacade.PlayButtonAudioClip();
        slideScrollView.ToLastPage();
    }
}
