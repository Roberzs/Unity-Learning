/****************************************************
    文件：GameOverPage.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class GameOverPage : MonoBehaviour 
{
    private Text txt_RoundCount;
    private Text txt_TotalCount;
    private Text txt_CurrentLevel;
    private NormalModelPanel normalModelPanel;

    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        txt_RoundCount = transform.Find("Txt_RoundCount").GetComponent<Text>();
        txt_TotalCount = transform.Find("Txt_TotalCount").GetComponent<Text>();
        txt_CurrentLevel = transform.Find("Txt_CurrentLevel").GetComponent<Text>();
    }

    private void OnEnable()
    {
        txt_TotalCount.text = normalModelPanel.totalRound.ToString();
        txt_CurrentLevel.text = ((GameController.Instance.currentStage.mBigLevelID - 1) * 5 + GameController.Instance.currentStage.mLevelID).ToString();
        normalModelPanel.ShowRoundText(txt_RoundCount);
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