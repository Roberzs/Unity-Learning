/****************************************************
	文件：UIBoard.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

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
            default:
                break;
        }
    }

    public override void RegisterAttentionEvent()
    {
        AttentionList.Add(StringDefine.E_UpdateDis);
        AttentionList.Add(StringDefine.E_UpdateCoin);
        AttentionList.Add(StringDefine.E_AddTimer);
    }

    #region Field
    int m_Coin = 0;
    int m_Distance = 0;

    float m_InitTime = 50f;             // 初始倒计时
    float m_AddTime = 20f;              // 倒计时增加变量

    float m_CountDownTimer = 0f;        // 倒计时

    GameModel m_GameModel = null;

    public Text txtCoin;
    public Text txtDistance;

    public Text txtCountDownTimer;
    public Image imgCountDownTimer;

    #endregion

    // 略: 封装属性快捷键 Ctrl + R,E

    public int Coin { get => m_Coin; set { m_Coin = value; txtCoin.text = value.ToString(); } }
    public int Distance { get => m_Distance; set {  m_Distance = value; txtDistance.text = value.ToString() + "米"; } }

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
    }

    private void Awake()
    {
        m_GameModel = GetModel<GameModel>();

        ResetData();
    }

    private void Update()
    {
        if (m_GameModel != null && (m_GameModel.IsPlay && !m_GameModel.IsPause))
        {
            CountDownTimer -= Time.deltaTime;
        }
    }
}
