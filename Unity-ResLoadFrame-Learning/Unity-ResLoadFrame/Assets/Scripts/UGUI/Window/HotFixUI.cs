using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotFixUI : Window
{
    private HotFixPanel m_hotFixPanel;

    private float m_SumTime;

    public override void Awake(params object[] paramList)
    {
        base.Awake(paramList);
        m_SumTime = 0.0f;
        m_hotFixPanel = GameObject.GetComponent<HotFixPanel>();
        m_hotFixPanel.txt_Tip.text = "";
        m_hotFixPanel.img_Prg.fillAmount = 0.0f;

        HotFixManager.Instance.ServerInfoError += ServerInfoError;
        HotFixManager.Instance.ItemError += ItemError;

        //HotFix();

        if (HotFixManager.Instance.ComputeUnPackPath())
        {
            
            m_hotFixPanel.txt_Tip.text = "解压中...";
            HotFixManager.Instance.StartUnpackFile(() => 
            {
                m_SumTime = 0;
                HotFix();
            });
        }
        else
        {
            HotFix();
        }
    }

    private void HotFix()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            // 网络无法连接
            GameRoot.OpenCommonConfirm("提示", "网络连接失败，请检查网络状态。", () => { Application.Quit(); }, () => { Application.Quit(); });
        }
        else
        {
            CheckVersion();
        }
    }

    private void CheckVersion()
    {
        HotFixManager.Instance.CheckVersion((bUpdate) => 
        {
            if (bUpdate)
            {
                // 更新
                var content = string.Format("当前版本为{0}，有{1:F}M大小的更新包，是否开始下载", HotFixManager.Instance.CurVersion, HotFixManager.Instance.LoadSumSize / 1024f);
                GameRoot.OpenCommonConfirm("提示", content, OnClickStartDownload, OnClickCancleDownload);
            }
            else
            {
                // 进入游戏
                DownloadFinished();
            }
        });
    }

    private void OnClickStartDownload()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var content = string.Format("当前处于非WiFI网络环境，是否继续下载");
            GameRoot.OpenCommonConfirm("提示", content, StartDownload, OnClickCancleDownload);
        }
        else
        {
            StartDownload();
        }
    }

    private void StartDownload()
    {
        m_hotFixPanel.txt_Tip.text = "下载中...";
        m_hotFixPanel.m_InfoPanel.gameObject.SetActive(true);
        m_hotFixPanel.txt_HotFixInfo.text = HotFixManager.Instance.CurrentPatches.Des;
        GameRoot.Instance.StartCoroutine(HotFixManager.Instance.StartDownloadAB(DownloadFinished));
    }

    /// <summary>
    /// 下载完成回调
    /// </summary>
    private void DownloadFinished()
    {
        GameRoot.Instance.StartCoroutine(OnFinished());
    }

    IEnumerator OnFinished()
    {
        yield return GameRoot.Instance.StartCoroutine(GameRoot.Instance.StartGame(m_hotFixPanel.img_Prg, m_hotFixPanel.txt_Tip));
        UIManager.Instance.CloseWnd(this);
    }


    public override void OnUpdate()
    {
        if (HotFixManager.Instance.IsStartDownLoad)
        {
            m_SumTime += Time.deltaTime;
            m_hotFixPanel.img_Prg.fillAmount = HotFixManager.Instance.GetProgress();
            float speed = HotFixManager.Instance.GetLoadSize() / 1024.0f / m_SumTime;
            m_hotFixPanel.txt_Speed.text = String.Format("{0:F}M/s", speed);
        }
        else if (HotFixManager.Instance.IsStartUnPack)
        {
            m_SumTime += Time.deltaTime;
            m_hotFixPanel.img_Prg.fillAmount = HotFixManager.Instance.GetUnpackProgress();
            float speed = HotFixManager.Instance.AlreadyUnPackSize / 1024.0f / m_SumTime;
            m_hotFixPanel.txt_Speed.text = String.Format("{0:F}M/s", speed);
        }

    }

    private void OnClickCancleDownload()
    {
        Application.Quit();
    }

    public override void OnClose()
    {
        base.OnClose();

        HotFixManager.Instance.ServerInfoError -= ServerInfoError;
        HotFixManager.Instance.ItemError -= ItemError;

        GameSceneManager.Instance.LoadSceen("MenuScene",
        () =>
            {
                UIManager.Instance.PopUpWnd("LoadingPanel.prefab", true);
            },
            () =>
            {
                //UIManager.Instance.OnUpdate
                UIManager.Instance.CloseWnd("LoadingPanel.prefab");
                UIManager.Instance.PopUpWnd("MenuPanel.prefab");
            });
    }

    private void ServerInfoError()
    {
        GameRoot.OpenCommonConfirm("提示", "服务器列表获取失败，请检查网络状态。是否重新下载？", CheckVersion, () => { Application.Quit(); });
    }

    private void ItemError(string content)
    {
        GameRoot.OpenCommonConfirm("提示", content + "资源下载失败。是否重新下载？", NewDownload, () => { Application.Quit(); });
    }

    private void NewDownload()
    {
        HotFixManager.Instance.CheckVersion((bUpdate) =>
        {
            if (bUpdate)
            {
                // 更新
                StartDownload();
            }
            else
            {
                // 进入游戏
                DownloadFinished();
            }
        });
    }
}
