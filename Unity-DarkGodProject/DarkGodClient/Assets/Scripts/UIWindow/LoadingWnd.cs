/****************************************************
    文件：LoadingWnd.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/9/8 23:9:49
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : WindowRoot 
{
    public Text txtTips;
    public Text txtPrg;
    public Image imgFG;
    public Image imgPoint;

    private float fgWidth;

    protected override void InitWnd()
    {
        base.InitWnd();

        fgWidth = imgFG.GetComponent<RectTransform>().sizeDelta.x;
        SetText(txtTips, "Tip:我是游戏提示");
        txtPrg.text = "0%";
        imgFG.fillAmount = 0;
        imgPoint.transform.localPosition = new Vector2(-fgWidth/2, 0);
    }

    public void SetProgress(float prg)
    {
        SetText(txtTips, (int)(prg * 100) + "%");
        imgFG.fillAmount = prg;
        imgPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(fgWidth * prg - fgWidth / 2, 0);
    }
}