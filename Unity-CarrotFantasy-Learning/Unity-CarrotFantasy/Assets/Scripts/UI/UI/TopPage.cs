/****************************************************
    文件：TopPage.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class TopPage : MonoBehaviour 
{
    // 引用
    private Text txt_coin;
    private Text txt_roundCount;
    private Text txt_totalCount;
    private Image img_Btn_GameSpeed;
    private Image img_Btn_Pause;
    private GameObject emp_pauseGo;
    private GameObject emp_playingTextGo;
    private NormalModelPanel normalModelPanel;

    // Sprite数组
    public Sprite[] btn_gameSpeedSprites;
    public Sprite[] btn_pauseSprites;

    // 属性
    private bool isNormalSpeed;
    private bool isPause;


    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        txt_coin = transform.Find("Txt_Coin").GetComponent<Text>();
        txt_roundCount = transform.Find("Emp_PlayingText").Find("Txt_RoundText").GetComponent<Text>();
        txt_totalCount = transform.Find("Emp_PlayingText").Find("Txt_TotalCount").GetComponent<Text>();
        img_Btn_GameSpeed = transform.Find("Btn_GameSpeed").GetComponent<Image>();
        img_Btn_Pause = transform.Find("Btn_Pause").GetComponent<Image>();
        emp_playingTextGo = transform.Find("Emp_PlayingText").gameObject;
        emp_pauseGo = transform.Find("Emp_Pause").gameObject;
    }

    private void OnEnable()
    {
        UpdateCoinText();
        txt_totalCount.text = normalModelPanel.totalRound.ToString();
        img_Btn_GameSpeed.sprite = btn_gameSpeedSprites[0];
        img_Btn_Pause.sprite = btn_pauseSprites[0];
        isNormalSpeed = true;
        isPause = false;

        emp_pauseGo.SetActive(false);
        emp_playingTextGo.SetActive(true);
    }

    public void UpdateCoinText()
    {
        txt_coin.text = GameController.Instance.coin.ToString();
    }

    public void UpdateRoundText()
    {
        normalModelPanel.ShowRoundText(txt_roundCount);
    }

    // 改变游戏速度
    public void ChangeGameSpeed()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        isNormalSpeed = !isNormalSpeed;
        if (isNormalSpeed)
        {
            GameController.Instance.gameSpeed = 1;
            img_Btn_GameSpeed.sprite = btn_gameSpeedSprites[0];
        }
        else
        {
            GameController.Instance.gameSpeed = 2;
            img_Btn_GameSpeed.sprite = btn_gameSpeedSprites[1];
        }
    }

    // 游戏暂停
    public void PauseGame()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        isPause = !isPause;
        if (isPause)
        {
            GameController.Instance.isPause = true;
            img_Btn_Pause.sprite = btn_pauseSprites[1];
            emp_pauseGo.SetActive(true);
            emp_playingTextGo.SetActive(false);
        }
        else
        {
            GameController.Instance.isPause = false;
            img_Btn_Pause.sprite = btn_pauseSprites[0];
            emp_pauseGo.SetActive(false);
            emp_playingTextGo.SetActive(true);
        }
    }

    public void ShowMenu()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        normalModelPanel.ShowMenuPage();
    }
}