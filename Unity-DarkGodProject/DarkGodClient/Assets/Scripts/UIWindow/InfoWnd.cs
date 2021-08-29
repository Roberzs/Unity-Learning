/****************************************************
    文件：InfoWnd.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/4/6 17:5:42
	功能：角色信息展示界面
*****************************************************/


using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoWnd : WindowRoot 
{
    #region UI组件
    public RawImage imgChar;

    public Text txtInfo;
    public Text txtExp;
    public Image imgExpPrg;
    public Text txtPower;
    public Image imgPowerPrg;

    public Text txtJob;
    public Text txtFight;
    public Text txtHp;
    public Text txtHurt;
    public Text txtDef;

    public Transform transDetail;

    public Button btnClose;
    public Button btnCloseDetail;
    public Button btnDetail;

    public Text dtxhp;
    public Text dtxad;
    public Text dtxap;
    public Text dtxaddef;
    public Text dtxapdef;
    public Text dtxdodge;
    public Text dtxpierce;
    public Text dtxcritical;

    #endregion

    private Vector2 startPos;

    protected override void InitWnd()
    {
        base.InitWnd();

        SetActive(transDetail, false);

        RegTouchEvts();
        RefreshUI();
    }

    private void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtInfo, pd.name + " LV." + pd.lv);
        SetText(txtExp, pd.exp + "/" + PECommon.GetExpUpValByLv(pd.lv));
        imgExpPrg.fillAmount = pd.exp * 1.0f / PECommon.GetExpUpValByLv(pd.lv);
        SetText(txtPower, pd.power + "/" + PECommon.GetPowerLimit(pd.lv));
        imgPowerPrg.fillAmount = pd.power * 1.0f / PECommon.GetPowerLimit(pd.lv);

        SetText(txtJob, "职业     暗夜刺客");
        SetText(txtFight, "战力     " + PECommon.GetFightByProps(pd));
        SetText(txtHp, "血量     " + pd.hp);
        SetText(txtHurt, "伤害     " + (pd.ad + pd.ap));
        SetText(txtDef, "防御     " + (pd.addef + pd.apdef));

        // 详细属性的赋值
        SetText(dtxhp, pd.hp);
        SetText(dtxad, pd.ad);
        SetText(dtxap, pd.ap);
        SetText(dtxaddef, pd.addef);
        SetText(dtxapdef, pd.apdef);
        SetText(dtxdodge, pd.dodge);
        SetText(dtxpierce, pd.pierce + "%");
        SetText(dtxcritical, pd.critical + "%");
    }

    public void ClickDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetActive(transDetail);
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.Instance.CloseInfoWnd();
    }

    public void ClickCloseDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetActive(transDetail, false);
    }

    private void RegTouchEvts()
    {
        OnClickDown(imgChar.gameObject, (PointerEventData evt) =>
        {
            MainCitySys.Instance.SetStartRoate();
            startPos = evt.position;
        });
        OnDrag(imgChar.gameObject, (PointerEventData evt) =>
        {
            float roate = -(evt.position.x - startPos.x) * 0.4f;
            MainCitySys.Instance.SetPlayerRoate(roate);
        });
    }
}