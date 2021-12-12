/****************************************************
    文件：FubenWnd.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/12/8 23:40:28
    功能：Nothing
*****************************************************/

using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FubenWnd : WindowRoot
{
    public Button[] fbBtnArr;
    public Transform pointerTrans;

    private PlayerData pd;

    protected override void InitWnd()
    {
        base.InitWnd();
        pd = GameRoot.Instance.PlayerData;

        RefreshUI();
    }

    public void RefreshUI()
    {
        int fbId = pd.fuben;
        for (int i = 0; i < fbBtnArr.Length; i++)
        {
            if (i < fbId % 10000)
            {
                SetActive(fbBtnArr[i].gameObject);
                if (i == fbId % 10000 - 1)
                {
                    pointerTrans.SetParent(fbBtnArr[i].transform);
                    pointerTrans.localPosition = new Vector2(25, 100);
                }
            }
            else
            {
                SetActive(fbBtnArr[i].gameObject, false);
            }
        }
    }

    public void ClickTaskBtn(int fbId)
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        // 检查体力是否足够
        int power = resSvc.GetMapCfgData(fbId).power;
        if (power > pd.power)
        {
            GameRoot.AddTips("体力值不足");
        }
        else
        {
            // 请求战斗
            netSvc.SendMsg(new GameMsg
            {
                cmd = (int)CMD.ReqFBFight,
                reqFBFight = new ReqFBFight
                {
                    fbId = fbId
                }
            });
        }
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}
