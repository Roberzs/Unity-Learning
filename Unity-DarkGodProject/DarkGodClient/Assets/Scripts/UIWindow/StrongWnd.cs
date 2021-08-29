/****************************************************
    文件：StrongWnd.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/4/12 12:11:22
    功能：Nothing
*****************************************************/

using PEProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StrongWnd : WindowRoot
{
    public Transform posBtnTrans;
    private Image[] imgs = new Image[6];
    private int currentIndex;
    private PlayerData pd;

    #region 相关UI
    public Image imgCurtPos;
    public Text txtStarLv;
    public Transform starTransGrp;
    public Text propHP1;
    public Text propHurt1;
    public Text propDef1;
    public Text propHP2;
    public Text propHurt2;
    public Text propDef2;
    public Image propArr1;
    public Image propArr2;
    public Image propArr3;

    public Transform costTransRoot;
    public Text txtNeedLv;
    public Text txtCostCoin;
    public Text txtCostCrystal;

    public Text txtCoin;

    #endregion

    private StrongCfg nextsd;

    protected override void InitWnd()
    {
        base.InitWnd();

        pd = GameRoot.Instance.PlayerData;

        RegClickEvts();
        ClickPosItem(0);
    }

    private void RegClickEvts()
    {
        for (int i = 0; i < posBtnTrans.childCount; i++)
        {
            Image img = posBtnTrans.GetChild(i).GetComponent<Image>();
            imgs[i] = img;
            OnClick(img.gameObject, (object args) =>
            {
                audioSvc.PlayUIAudio(Constants.UIClickBtn);
                ClickPosItem((int)args);
            },i);
        }
    }

    private void ClickPosItem(int index)
    {
        PECommon.Log("点击事件" + index);
        currentIndex = index;
        for (int i = 0; i< imgs.Length; i++)
        {
            Transform trans = imgs[i].transform;
            if (i == currentIndex)
            {
                // 当前点击图片
                SetSprite(imgs[i], PathDefine.ItemArrorBG);
                trans.localPosition = new Vector3(10, trans.localPosition.y, 0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 95);
            }
            else
            {
                SetSprite(imgs[i], PathDefine.ItemPlatBG);
                trans.localPosition = new Vector3(0, trans.localPosition.y, 0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(220, 85);
            }
        }
        RefreshItem();
    }

    private void RefreshItem()
    {
        SetText(txtCoin, pd.coin);

        switch (currentIndex)
        {
            case 0:
                SetSprite(imgCurtPos, PathDefine.ItemToukui);
                break;
            case 1:
                SetSprite(imgCurtPos, PathDefine.ItemBody);
                break;
            case 2:
                SetSprite(imgCurtPos, PathDefine.ItemYaobu);
                break;
            case 3:
                SetSprite(imgCurtPos, PathDefine.ItemHand);
                break;
            case 4:
                SetSprite(imgCurtPos, PathDefine.ItemLeg);
                break;
            case 5:
                SetSprite(imgCurtPos, PathDefine.ItemFoot);
                break;
        }
        SetText(txtStarLv, pd.strongArr[currentIndex] + "星级");
        int curtStarLv = pd.strongArr[currentIndex];
        for (int i = 0; i < starTransGrp.childCount; i++)
        {
            Image img = starTransGrp.GetChild(i).GetComponent<Image>();
            if (i < curtStarLv)
            {
                SetSprite(img, PathDefine.SpStar2);
            }
            else
            {
                SetSprite(img, PathDefine.SpStar1);
            }
        }

        int sumAddHp = resSvc.GetPropAddValPreLv(currentIndex, curtStarLv, 1);
        int sumAddHurt = resSvc.GetPropAddValPreLv(currentIndex, curtStarLv, 2);
        int sumAddDef = resSvc.GetPropAddValPreLv(currentIndex, curtStarLv, 3);

        SetText(propHP1, "生命 + " + sumAddHp);
        SetText(propHurt1, "伤害 + " + sumAddHurt);
        SetText(propDef1, "防御 + " + sumAddDef);

        int nextStartLv = curtStarLv + 1;
        nextsd = resSvc.GetStrongData(currentIndex, nextStartLv);
        if (nextsd != null)
        {
            SetActive(propHP2, true);
            SetActive(propHurt2, true);
            SetActive(propDef2, true);
            SetActive(costTransRoot, true);
            SetActive(propArr1, true);
            SetActive(propArr2, true);
            SetActive(propArr3, true);

            SetText(propHP2, "强化后 + " + nextsd.addhp);
            SetText(propHurt2, "+ " + nextsd.addhurt);
            SetText(propDef2, "+ " + nextsd.adddef);

            SetText(txtNeedLv, "需要等级："+nextsd.minlv);
            SetText(txtCostCoin, "需要消耗：      " + nextsd.coin);
            SetText(txtCostCrystal, nextsd.crystal + "/" + pd.crystal);

            SetText(txtCoin, pd.coin);
        }
        else
        {
            SetActive(propHP2, false);
            SetActive(propHurt2, false);
            SetActive(propDef2, false);
            SetActive(costTransRoot, false);
            SetActive(propArr1, false);
            SetActive(propArr2, false);
            SetActive(propArr3, false);
        }
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

    public void ClickStrongBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        // 数据校验
        if(pd.strongArr[currentIndex] < 10)
        {
            if (pd.lv < nextsd.minlv)
            {
                GameRoot.AddTips("等级不足");
                return;
            }
            if (pd.coin < nextsd.coin)
            {
                GameRoot.AddTips("金币不足");
                return;
            }
            if (pd.crystal < nextsd.crystal)
            {
                GameRoot.AddTips("水晶不足");
                return;
            }

            netSvc.SendMsg(new GameMsg
            {
                cmd = (int)CMD.ReqStrong,
                reqStrong = new ReqStrong
                {
                    pos = currentIndex
                }
            });
        }
        else
        {
            GameRoot.AddTips("星级已升至最顶级");
        }
    }

    public void UpdateUI()
    {
        audioSvc.PlayUIAudio(Constants.FBItem);
        RefreshItem();
    }
}
