/****************************************************
    文件：BuyWnd.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/4/13 18:58:25
    功能：Nothing
*****************************************************/

using PEProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyWnd : WindowRoot
{
    private int buyType;

    public Text txtInfo;
    public Button btnSure;

    protected override void InitWnd()
    {
        base.InitWnd();
        btnSure.interactable = true;
        RefreshUI();
    }

    public void SetBuyType(int type)
    {
        buyType = type;
    }

    private void RefreshUI()
    {
        switch (buyType)
        {
            case 0:
                // 购买体力
                txtInfo.text = "是否花费" + Constants.Color("10钻石", TxtColor.Red) + "购买" + Constants.Color("50体力", TxtColor.Green);
                break;
            case 1:
                // 购买金币
                txtInfo.text = "是否花费" + Constants.Color("10钻石", TxtColor.Red) + "购买" + Constants.Color("1000金币", TxtColor.Green);
                break;
        }
    }

    public void ClickSureBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        PlayerData pd = GameRoot.Instance.PlayerData;
        if (pd.diamond < 10)
        {
            GameRoot.AddTips("钻石不足");
            return;
        }

        // 网络消息
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ReqBuy,
            reqBuy = new ReqBuy
            {
                type = buyType,
                cost = 10
            }
        };

        netSvc.SendMsg(msg);
        btnSure.interactable = false;
    }

    public void ClickCloseWnd()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}
