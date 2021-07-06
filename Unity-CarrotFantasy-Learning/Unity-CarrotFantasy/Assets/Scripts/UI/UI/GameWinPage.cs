/****************************************************
    文件：GameWinPage.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class GameWinPage : MonoBehaviour 
{
    private NormalModelPanel normalModelPanel;
    private Text txt_RoundCount;
    private Text txt_TotalCount;
    private Text txt_CurrentLevel;
    private Image img_Carrot;
    public Sprite[] carrotSprites;

    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        txt_RoundCount = transform.Find("Txt_RoundCount").GetComponent<Text>();
        txt_TotalCount = transform.Find("Txt_TotalCount").GetComponent<Text>();
        txt_CurrentLevel = transform.Find("Txt_CurrentLevel").GetComponent<Text>();
        img_Carrot = transform.Find("Img_Carrot").GetComponent<Image>();
    }

    private void OnEnable()
    {
        txt_TotalCount.text = normalModelPanel.totalRound.ToString();
        txt_CurrentLevel.text = ((GameController.Instance.currentStage.mBigLevelID - 1) * 5 + GameController.Instance.currentStage.mLevelID).ToString();
        normalModelPanel.ShowRoundText(txt_RoundCount);
        if(GameController.Instance.carrotHp >= 8)
        {
            img_Carrot.sprite = carrotSprites[0];
        }
        else if (GameController.Instance.carrotHp >= 4)
        {
            img_Carrot.sprite = carrotSprites[1];
        }
        else
        {
            img_Carrot.sprite = carrotSprites[2];
        }
    }

    public void Replay()
    {
        normalModelPanel.Replay();
    }

    public void ChooseOtherLevel()
    {
        normalModelPanel.ChooseOtherLevel();
    }
}