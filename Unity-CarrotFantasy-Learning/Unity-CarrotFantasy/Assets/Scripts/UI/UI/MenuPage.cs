/****************************************************
    文件：MenuPage.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class MenuPage : MonoBehaviour 
{
    private NormalModelPanel normalModelPanel;

    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
    }

    public void GoOn()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        GameController.Instance.isPause = false;
        transform.gameObject.SetActive(false);
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