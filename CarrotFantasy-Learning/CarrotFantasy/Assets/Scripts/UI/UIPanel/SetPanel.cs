/****************************************************
    文件：SetPanel.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SetPanel : BasePanel 
{
    private GameObject optionPageGo;
    private GameObject statisticsPageGo;
    private GameObject producerPageGo;
    private GameObject panel_ResetGo;
    private Tween setPanelTween;
    private bool playBGMusic = true;
    private bool playEffectMusic = true;
    public Sprite[] btnSprites;         // 0 - 音效开 1 - 音效关  2 - 音乐开  3 - 音乐关
    private Image img_Btn_EffectAudio;
    private Image img_Btn_BGAudio;
    public Text[] statisticesTexts;

    protected override void Awake()
    {
        base.Awake();

        setPanelTween = transform.DOLocalMoveX(0, 0.5f);
        setPanelTween.SetAutoKill(false);
        setPanelTween.Pause();

        optionPageGo = transform.Find("OptionPage").gameObject;
        statisticsPageGo = transform.Find("StatisticsPage").gameObject;
        producerPageGo = transform.Find("ProducerPage").gameObject;
        img_Btn_EffectAudio = optionPageGo.transform.Find("Btn_EffectAudio").GetComponent<Image>();
        img_Btn_BGAudio = optionPageGo.transform.Find("Btn_BGAudio").GetComponent<Image>();
        panel_ResetGo = optionPageGo.transform.Find("ResetPage").gameObject;
        panel_ResetGo.SetActive(false);
    }

    public override void InitPanel()
    {
        transform.localPosition = new Vector3(-1024, 0, 0);
        transform.SetSiblingIndex(3);
    }

    public override void EnterPanel()
    {
        ShowOptionPage();
        MoveToCenter();
    }

    public override void ExitPanel()
    {
        setPanelTween.PlayBackwards();
        mUIFacade.currentScenePanelDict[StringManager.MainPanel].EnterPanel();
        InitPanel();
    }

    private void MoveToCenter()
    {
        setPanelTween.PlayForward();
    }

    /** 页面的显示 */
    public void ShowOptionPage()
    {
        optionPageGo.SetActive(true);
        statisticsPageGo.SetActive(false);
        producerPageGo.SetActive(false);
    }

    public void ShowStatisicsPage()
    {
        optionPageGo.SetActive(false);
        statisticsPageGo.SetActive(true);
        producerPageGo.SetActive(false);
        UpdateStatistices();
    }

    public void ShowProducerPage()
    {
        optionPageGo.SetActive(false);
        statisticsPageGo.SetActive(false);
        producerPageGo.SetActive(true);
    }

    /** 声音的打开或关闭 */

    public void CloseOrOpenEffectMusic()
    {
        playEffectMusic = !playEffectMusic;
        mUIFacade.CloseOrOpenEffectMusic();
        if (playEffectMusic)
        {
            img_Btn_EffectAudio.sprite = btnSprites[0];
        }
        else
        {
            img_Btn_EffectAudio.sprite = btnSprites[1];
        }
    }

    public void CloseOrOpenBGMusic()
    {
        playBGMusic = !playBGMusic;
        mUIFacade.CloseOrOpenBGMusic();
        if (playBGMusic)
        {
            img_Btn_BGAudio.sprite = btnSprites[2];
        }
        else
        {
            img_Btn_BGAudio.sprite = btnSprites[3];
        }
    }

    // 更新显示数据
    public void UpdateStatistices()
    {
        PlayerManager playerManager = mUIFacade.mPlayerManager;

        statisticesTexts[0].text = playerManager.adventrueModelNum.ToString();
        statisticesTexts[1].text = playerManager.burriedLevelNum.ToString();
        statisticesTexts[2].text = playerManager.bossModelNum.ToString();
        statisticesTexts[3].text = playerManager.coin.ToString();
        statisticesTexts[4].text = playerManager.killMonsterNum.ToString();
        statisticesTexts[5].text = playerManager.killBossNum.ToString();
        statisticesTexts[6].text = playerManager.clearItemNum.ToString();
    }

    // 游戏重置
    public void ResetGame()
    {

    }

    public void ShowResetPanel()
    {
        panel_ResetGo.SetActive(true);
    }

    public void CloseResetPanel()
    {
        panel_ResetGo.SetActive(false);
    }
}