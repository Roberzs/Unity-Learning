/****************************************************
    文件：NormalModelPanel.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/21 17:56:23
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NormalModelPanel : BasePanel
{
    // 页面物体
    private GameObject topPageGo;
    private GameObject gameOverPageGo;
    private GameObject gameWinPaeGo;
    private GameObject menuPageGo;
    private GameObject img_FinalWave;
    private GameObject img_StartGame;
    private GameObject prizePageGo;

    public int totalRound;

    // 引用
    public TopPage topPage;

    protected override void Awake()
    {
        base.Awake();
        transform.SetSiblingIndex(1);
        topPageGo = transform.Find("Img_TopPage").gameObject;
        gameOverPageGo = transform.Find("GameOverPage").gameObject;
        gameWinPaeGo = transform.Find("GameWinPage").gameObject;
        menuPageGo = transform.Find("MenuPage").gameObject;
        img_FinalWave = transform.Find("Img_FinalWave").gameObject;
        img_StartGame = transform.Find("StartUI").gameObject;
        prizePageGo = transform.Find("PrizePage").gameObject;
        topPage = topPageGo.GetComponent<TopPage>();
    }

    

    private void OnEnable()
    {
        
        InvokeRepeating("PlayAudio", 0.5f, 1);
        Invoke("StartGame", 3.5f);
    }

    // 开始游戏倒计时的声音
    private void PlayAudio()
    {
        img_StartGame.SetActive(true);
        GameController.Instance.PlayEffectMusic("NormalMordel/CountDown");
    }

#if GameRuning

    // 开始游戏
    private void StartGame()
    {
        GameController.Instance.PlayEffectMusic("NormalMordel/GO");
        GameController.Instance.StartGame();
        img_StartGame.SetActive(false);
        CancelInvoke();
    }
#endif
    /// <summary>
    /// 面板的各种操作
    /// </summary>
    public override void EnterPanel()
    {
        base.EnterPanel();

        totalRound = GameController.Instance.currentStage.mTotalRound;
        topPageGo.SetActive(true);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();
        topPage.UpdateRoundText();
        topPage.UpdateCoinText();
    }

    // 显示或隐藏奖励
    public void ShowPrizePage(bool isShow = true)
    {
        if (!isShow) mUIFacade.PlayButtonAudioClip();
        GameController.Instance.isPause = isShow;
        prizePageGo.SetActive(isShow);
    }

    // 显示或隐藏菜单
    public void ShowMenuPage(bool isShow = true)
    {
        if (isShow) mUIFacade.PlayButtonAudioClip(); ;
        GameController.Instance.isPause = isShow;
        menuPageGo.SetActive(isShow);
    }

    // 更新显示回合数的文本
    public void ShowRoundText(Text roundText)
    {
        int roundNum = GameController.Instance.level.currentRound + 1;
        string roundStr = "";
        if (roundNum < 10)
            roundStr = "0 " + roundNum.ToString();
        else
            roundStr = (roundNum / 10).ToString() + " " + (roundNum % 10).ToString();
        
        roundText.text = roundStr;
    }

    // 胜利页面显示
    public void ShowGameWinPage()
    {
        Stage stage =  GameManager.Instance.playerManager.unLockedNormalModelLevelList[(GameController.Instance.currentStage.mBigLevelID - 1) * 5 + GameController.Instance.currentStage.mLevelID - 1];
        // 道具徽章
        if (GameController.Instance.IfAllClear())
        {
            stage.mAllClear = true;
        }
        // 萝卜徽章
        int carrotState = GameController.Instance.GetCarrotState();

        if (carrotState != 0 && stage.mCarrotState != 0)
        {
            if (carrotState < stage.mCarrotState)
            {
                stage.mCarrotState = GameController.Instance.GetCarrotState();
            }
        }
        else
        {
            stage.mCarrotState = GameController.Instance.GetCarrotState();
        }

            

        // 解锁下一关卡 注:当前关卡不是最后一关且下一关不是隐藏关卡
        if (GameController.Instance.currentStage.mLevelID % 5 != 0 && ((GameController.Instance.currentStage.mBigLevelID - 1) * 5 + GameController.Instance.currentStage.mLevelID - 1) < GameManager.Instance.playerManager.unLockedNormalModelLevelList.Count)
        {
            GameManager.Instance.playerManager.unLockedNormalModelLevelList[(GameController.Instance.currentStage.mBigLevelID - 1) * 5 + GameController.Instance.currentStage.mLevelID].unLocked = true;

        }
        UpdatePlayerManagerData();
        gameWinPaeGo.SetActive(true);
        GameController.Instance.gameOver = false;
        GameManager.Instance.playerManager.adventrueModelNum++;

        GameController.Instance.PlayEffectMusic("NormalMordel/Perfect");
    }

    // 失败页面显示
    public void ShowGameOverPage()
    {
        UpdatePlayerManagerData();
        gameOverPageGo.SetActive(true);
        GameController.Instance.gameOver = false;

        GameController.Instance.PlayEffectMusic("NormalMordel/Lose");
    }

    // 最后一波怪物
    public void ShowFinalWaveUI()
    {
        GameController.Instance.PlayEffectMusic("NormalMordel/FinalWave");
        img_FinalWave.SetActive(true);
        Invoke("CloseFinalWaveUI", 0.8f);
    }

    private void CloseFinalWaveUI()
    {
        img_FinalWave.SetActive(false);
    }

    /// <summary>
    /// 关卡处理有关
    /// </summary>
    
    private void UpdatePlayerManagerData()
    {
        GameManager.Instance.playerManager.coin += GameController.Instance.coin;
        GameManager.Instance.playerManager.killMonsterNum += GameController.Instance.killMonsterTotalNum;
        GameManager.Instance.playerManager.clearItemNum += GameController.Instance.clearItemNum;
    }

    public void Replay()
    {
        gameObject.SetActive(false);

        mUIFacade.PlayButtonAudioClip();
        UpdatePlayerManagerData();
        mUIFacade.ChangeSceneState(new NormalModelSceneState(mUIFacade));
        //GameController.Instance.gameOver = false;
        Invoke("ResetGame", 1);
    }

    // 重置当前关卡
    private void ResetGame()
    {
        ResetUI();
        SceneManager.LoadScene(3);
        gameObject.SetActive(true);
    }

    // 重置页面UI的显示
    public void ResetUI()
    {
        gameOverPageGo.SetActive(false);
        gameWinPaeGo.SetActive(false);
        menuPageGo.SetActive(false);
        gameObject.SetActive(false);
    }

    // 选择其他关卡
    public void ChooseOtherLevel()
    {
        mUIFacade.PlayButtonAudioClip();

        //GameController.Instance.gameOver = false;
        UpdatePlayerManagerData();
        Invoke("ToOhterScene", 2);
        mUIFacade.ChangeSceneState(new NormalGameOptionSceneState(mUIFacade));
    }

    public void ToOhterScene()
    {
        GameController.Instance.gameOver = false;
        ResetUI();
        SceneManager.LoadScene(2);
    }
}
