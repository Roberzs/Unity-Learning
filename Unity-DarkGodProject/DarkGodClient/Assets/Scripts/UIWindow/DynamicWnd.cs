/****************************************************
    文件：DynamicWnd.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/11/26 22:58:29
	功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWnd : WindowRoot 
{
    public Animation tipsAni;
    public Text txtTips;

    private Queue<string> tipsQue = new Queue<string>();
    private bool isTipsShow = false;

    protected override void InitWnd()
    {
        base.InitWnd();
        SetActive(txtTips, false);
    }

    public void AddTips(string tips)
    {
        lock (tipsQue)
        {
            tipsQue.Enqueue(tips);
        }
    }

    private void Update()
    {
        if (tipsQue.Count > 0 && isTipsShow == false)
        {
            lock (tipsQue)
            {
                string tips = tipsQue.Dequeue();
                isTipsShow = true;
                SetTips(tips);
            }
        }
    }

    private void SetTips (string tips)
    {
        SetActive(txtTips);
        SetText(txtTips, tips);

        AnimationClip clip = tipsAni.GetClip("TipsShowAnim");
        tipsAni.Play();

        // 延时关闭激活
        StartCoroutine(AniPlayDone(clip.length, ()=> {
            SetActive(txtTips, false);
            isTipsShow = false;
        }));
    }

    private IEnumerator AniPlayDone(float sec, Action cb)
    {
        yield return new WaitForSeconds(sec);
        cb?.Invoke();
    }
}