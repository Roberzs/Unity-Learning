/****************************************************
	文件：LoadingUI.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class LoadingUI : Window
{
    private LoadingPanel m_LoadingPanel;

    public override void Awake(params object[] paramList)
    {
        m_LoadingPanel = GameObject.GetComponent<LoadingPanel>();
    }

    public override void OnUpdate()
    {
        if (!m_LoadingPanel) 
            return;

        m_LoadingPanel.prgTxt.text = GameSceneManager.LoadingProgress.ToString("0") + "%";
        m_LoadingPanel.prgImg.fillAmount = GameSceneManager.LoadingProgress / 100.0f;
    }
}

