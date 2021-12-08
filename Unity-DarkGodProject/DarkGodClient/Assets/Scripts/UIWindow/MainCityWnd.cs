/****************************************************
    文件：MainCityWnd.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/31 16:16:32
	功能：主城UI界面
*****************************************************/

using UnityEngine;
using PEProtocol;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainCityWnd : WindowRoot 
{
    public Image ImgTouch;
    public Image ImgDirBg;
    public Image ImgDirPoint;

    public Animation menuAni;
    public Button btnMenu;

    public Text txtFight;
    public Text txtPower;
    public Image ImgPowerPrg;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;
    public Transform expPrgTrans;

    public Button btnGuide;


    private bool menuState = true;
    private float pointDis;
    private Vector2 startPos = Vector2.zero;

    private AutoGuideCfg curtTaskData;

    protected override void InitWnd()
    {
        base.InitWnd();

        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;

        SetActive(ImgDirPoint, false);

        RegisterTouchEvts();

        RefreshUI();
    }

    public void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtFight, PECommon.GetFightByProps(pd));
        SetText(txtPower, "体力:" + pd.power + "/" + PECommon.GetPowerLimit(pd.lv));
        ImgPowerPrg.fillAmount = pd.power * 1.0f / PECommon.GetPowerLimit(pd.lv);
        SetText(txtLevel, pd.lv);
        SetText(txtName, pd.name);

        // 经验条自适应
        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        float globalRate = 1.0f * Constants.ScreenStandardHeight / Screen.height;       // 适应宽度
        float screenWidth = Screen.width * globalRate;
        float width = (screenWidth - 180) / 10;
        grid.cellSize = new Vector2(width, 8);

        int expPrgVal = (int)((pd.exp * 1.0f / PECommon.GetExpUpValByLv(pd.lv)) * 100);
        SetText(txtExpPrg, expPrgVal + "%");

        int index = expPrgVal / 10;
        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;
            }
            else if (i == index){
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            }
            else
            {
                img.fillAmount = 0;
            }
        }

        // 设置自动任务图标
        curtTaskData = resSvc.GetAutoGuideData(pd.guideid);
        if (curtTaskData != null)
        {
            SetGuideBtnIcon(curtTaskData.npcID);
        }
        else
        {
            SetGuideBtnIcon(-1);
        }
        
    }

    private void SetGuideBtnIcon(int npcID)
    {
        string spPath = "";
        Image img = btnGuide.GetComponent<Image>();
        switch (npcID)
        {
            case Constants.NPCWiseMan:
                spPath = PathDefine.WiseManHead;
                break;
            case Constants.NPCGeneral:
                spPath = PathDefine.GeneralHead;
                break;
            case Constants.NPCArtisan:
                spPath = PathDefine.ArtisanHead;
                break;
            case Constants.NPCTrader:
                spPath = PathDefine.TraderHead;
                break;
            default:
                spPath = PathDefine.TaskHead;
                break;
        }
        SetSprite(img, spPath);
    }

    #region 按钮点击事件

    public void ClickGuideBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        if (curtTaskData != null)
        {
            MainCitySys.Instance.RunTask(curtTaskData);
        }
        else
        {
            GameRoot.AddTips("更多引导任务，敬请期待……");
        }
    }

    public void ClickMenuBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIExtenBtn);

        menuState = !menuState;
        AnimationClip clip = null;
        if (menuState)
        {
            clip = menuAni.GetClip("OpenMCMenu");
        }
        else
        {
            clip = menuAni.GetClip("CloseMCMenu");
        }
        menuAni.Play(clip.name);
    }

    public void ClickTaskBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenTaskRewardWnd();

    }

    public void ClickFubenBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.EnterFuben();
    }

    public void ClickHeadBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenInfoWnd();
        
    }

    public void ClickStrongBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenStrongWnd();

    }

    public void ClickChatBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenChatWnd();

    }

    public void ClickBuyPowerBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenBuyWnd(0);
    }

    public void ClickMkCoinBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenBuyWnd(1);
    }

    public void RegisterTouchEvts()
    {
        OnClickDown(ImgTouch.gameObject, (PointerEventData evt) =>
        {
            startPos = evt.position;
            SetActive(ImgDirPoint);
            ImgDirBg.transform.position = evt.position;
        });
        OnClickUp(ImgTouch.gameObject, (PointerEventData evt) =>
        {
            ImgDirBg.transform.localPosition = Vector2.zero;
            ImgDirPoint.transform.localPosition = Vector2.zero;
            SetActive(ImgDirPoint, false);

            MainCitySys.Instance.SetMoveDir(Vector3.zero);
        });
        OnDrag(ImgTouch.gameObject, (PointerEventData evt) =>
        {
            Vector2 dir = evt.position - (Vector2)(ImgDirBg.transform.position);
            float len = dir.magnitude;
            if (len > pointDis)
            {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                ImgDirPoint.transform.position = (Vector2)(ImgDirBg.transform.position) + clampDir;
            }
            else
            {
                ImgDirPoint.transform.position = evt.position;
            }
            MainCitySys.Instance.SetMoveDir(dir);
        });
    }
    #endregion
}