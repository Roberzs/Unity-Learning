/****************************************************
	文件：UIBoard.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIBoard : View
{
    public override string Name => StringDefine.V_UIBoard;
    

    public override void HandleEvent(string name, object data)
    {
        switch (name)
        {
            case StringDefine.E_UpdateDis:
                DistanceArgs e0 = data as DistanceArgs;
                Distance = e0.distance;
                break;
            case StringDefine.E_UpdateCoin:
                CoinArgs e1 = data as CoinArgs;
                Coin += e1.coinCount;
                break;
            case StringDefine.E_AddTimer:
                CountDownTimer += m_AddTime;
                break;
            case StringDefine.E_HitGoalTrigger:
                break;
            default:
                break;
        }
    }

    public override void RegisterAttentionEvent()
    {
        AttentionList.Add(StringDefine.E_UpdateDis);
        AttentionList.Add(StringDefine.E_UpdateCoin);
        AttentionList.Add(StringDefine.E_AddTimer);
        AttentionList.Add(StringDefine.E_HitGoalTrigger);
    }

    #region Field
    int m_Coin = 0;
    int m_Distance = 0;
    int m_Goal = 0;

    float m_InitTime = 50f;             // 初始倒计时
    float m_AddTime = 20f;              // 倒计时增加变量

    float m_CountDownTimer = 0f;        // 倒计时
    float m_SkillTimer = 0;
    GameModel m_GameModel = null;

    public Text txtCoin;
    public Text txtDistance;

    public Text txtCountDownTimer;
    public Image imgCountDownTimer;

    public Text txtGizmoMangent;
    public Text txtGizmoMultiply;
    public Text txtGizmoInvinclble;

    public Slider sliTime;
    public Slider sliGoal;

    public Button btnMagnet;
    public Button btnMultiply;
    public Button btnInvincible;
    public Button btnGoal;

    private IEnumerator MultiplyCor = null;
    private IEnumerator MagnetCor = null;
    private IEnumerator InvincibleCor = null;
    #endregion

    // 略: 封装属性快捷键 Ctrl + R,E

    public int Coin { get => m_Coin; set { m_Coin = value; txtCoin.text = value.ToString(); } }
    public int Distance { get => m_Distance; set {  m_Distance = value; txtDistance.text = value.ToString() + "米"; } }

    public int Goal { get => m_Goal; set => m_Goal = value; }

    public float CountDownTimer 
    { 
        get => m_CountDownTimer; 
        set {
            if (value <= 0)
            {
                m_CountDownTimer = 0f;
                SendEvent(StringDefine.E_EndGame);
            }
            m_CountDownTimer = Mathf.Min(value, m_InitTime);
            txtCountDownTimer.text = m_CountDownTimer.ToString("f2") + "s";
            imgCountDownTimer.fillAmount = m_CountDownTimer / m_InitTime;
        } 
    }

    private void ResetData()
    {
        Coin = 0;
        Distance = 0;
        CountDownTimer = m_InitTime;
        m_SkillTimer = m_GameModel.SkillTime;
    }

    #region Mono

    private void Awake()
    {
        m_GameModel = GetModel<GameModel>();
        m_SkillTimer = m_GameModel.SkillTime;
        UpdateUI();
        ResetData();
    }

    private void Update()
    {
        if (m_GameModel != null && (m_GameModel.IsPlay && !m_GameModel.IsPause))
        {
            CountDownTimer -= Time.deltaTime;
        }
    }

    #endregion

    #region Func
    /// <summary>
    /// 更新按钮是否可用
    /// </summary>
    public void UpdateUI()
    {
        ShowOrHideBtn(m_GameModel.InvincibleCnt, btnInvincible);
        ShowOrHideBtn(m_GameModel.MagnetCnt, btnMagnet);
        ShowOrHideBtn(m_GameModel.MultiplyCnt, btnMultiply);
    }

    private void ShowOrHideBtn(float i, Button btn)
    {
        if (i > 0)
        {
            btn.interactable = true;
            btn.transform.Find("Mask").gameObject.SetActive(false);
        }
        else
        {
            btn.interactable = false;
            btn.transform.Find("Mask").gameObject.SetActive(true);
        }
    }

    public void OnClickPauseBtn()
    {
        PauseArgs e = new PauseArgs()
        {
            distance = Distance,
            score = Coin * 3 + Distance+ Goal * 15,
            coin = Coin
        };
        SendEvent(StringDefine.E_PauseGame, e);
    }

    public void HitMultiply()
    {
        if (MultiplyCor != null)
        {
            StopCoroutine(MultiplyCor);
        }
        MultiplyCor = MultiplyCoroutine();
        StartCoroutine(MultiplyCor);
    }

    IEnumerator MultiplyCoroutine()
    {
        float timer = m_SkillTimer;
        txtGizmoMultiply.transform.parent.gameObject.SetActive(true);
        while (timer > 0)
        {
            yield return new WaitForEndOfFrame();
            if (m_GameModel.IsPlay && !m_GameModel.IsPause)
            {
                timer -= Time.deltaTime;
                txtGizmoMultiply.text = GetTime(timer);
            }
        }
        txtGizmoMultiply.transform.parent.gameObject.SetActive(false);
    }

    public void HitMagnet()
    {
        if (MagnetCor != null)
        {
            StopCoroutine(MagnetCor);
        }
        MagnetCor = MagnetCoroutine();
        StartCoroutine(MagnetCor);
    }

    IEnumerator MagnetCoroutine()
    {
        float timer = m_SkillTimer;
        txtGizmoMangent.transform.parent.gameObject.SetActive(true);
        while (timer > 0)
        {
            yield return new WaitForEndOfFrame();
            if (m_GameModel.IsPlay && !m_GameModel.IsPause)
            {
                timer -= Time.deltaTime;
                txtGizmoMangent.text = GetTime(timer);
            }
        }
        txtGizmoMangent.transform.parent.gameObject.SetActive(false);
    }

    public void HitInvincible()
    {
        if (InvincibleCor != null)
        {
            StopCoroutine(InvincibleCor);
        }
        InvincibleCor = InvincibleCoroutine();
        StartCoroutine(InvincibleCor);
    }

    IEnumerator InvincibleCoroutine()
    {
        float timer = m_SkillTimer;
        txtGizmoInvinclble.transform.parent.gameObject.SetActive(true);
        while (timer > 0)
        {
            yield return new WaitForEndOfFrame();
            if (m_GameModel.IsPlay && !m_GameModel.IsPause)
            {
                timer -= Time.deltaTime;
                txtGizmoInvinclble.text = GetTime(timer);
            }
        }
        txtGizmoInvinclble.transform.parent.gameObject.SetActive(false);
    }

    private string GetTime(float time)
    {
        return time.ToString("%f0");
    }

    public void OnClickMagnetBtn()
    {
        UseSkill(ItemKind.ItemMagnet);
    }

    public void OnClickInvincibleBtn()
    {
        UseSkill(ItemKind.ItemInvincible);
    }

    public void OnClickMultiplyBtn()
    {
        UseSkill(ItemKind.ItemMultiply);
    }

    private void UseSkill(ItemKind item)
    {
        ItemArgs args = new ItemArgs
        {
            hitCount = 1,
            kind = item
        };
        SendEvent(StringDefine.E_HitItem, args);
    }

    private void ShowGoalUI()
    {

    }

    #endregion
}
