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
            
            m_hotFixPanel.txt_Tip.text = "��ѹ��...";
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
            // �����޷�����
            GameRoot.OpenCommonConfirm("��ʾ", "��������ʧ�ܣ���������״̬��", () => { Application.Quit(); }, () => { Application.Quit(); });
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
                // ����
                var content = string.Format("��ǰ�汾Ϊ{0}����{1:F}M��С�ĸ��°����Ƿ�ʼ����", HotFixManager.Instance.CurVersion, HotFixManager.Instance.LoadSumSize / 1024f);
                GameRoot.OpenCommonConfirm("��ʾ", content, OnClickStartDownload, OnClickCancleDownload);
            }
            else
            {
                // ������Ϸ
                DownloadFinished();
            }
        });
    }

    private void OnClickStartDownload()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            var content = string.Format("��ǰ���ڷ�WiFI���绷�����Ƿ��������");
            GameRoot.OpenCommonConfirm("��ʾ", content, StartDownload, OnClickCancleDownload);
        }
        else
        {
            StartDownload();
        }
    }

    private void StartDownload()
    {
        m_hotFixPanel.txt_Tip.text = "������...";
        m_hotFixPanel.m_InfoPanel.gameObject.SetActive(true);
        m_hotFixPanel.txt_HotFixInfo.text = HotFixManager.Instance.CurrentPatches.Des;
        GameRoot.Instance.StartCoroutine(HotFixManager.Instance.StartDownloadAB(DownloadFinished));
    }

    /// <summary>
    /// ������ɻص�
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
        GameRoot.OpenCommonConfirm("��ʾ", "�������б��ȡʧ�ܣ���������״̬���Ƿ��������أ�", CheckVersion, () => { Application.Quit(); });
    }

    private void ItemError(string content)
    {
        GameRoot.OpenCommonConfirm("��ʾ", content + "��Դ����ʧ�ܡ��Ƿ��������أ�", NewDownload, () => { Application.Quit(); });
    }

    private void NewDownload()
    {
        HotFixManager.Instance.CheckVersion((bUpdate) =>
        {
            if (bUpdate)
            {
                // ����
                StartDownload();
            }
            else
            {
                // ������Ϸ
                DownloadFinished();
            }
        });
    }
}
