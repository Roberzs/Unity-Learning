/****************************************************
	文件：GameModel.cs
	作者：Zhangying
	邮箱：zhy18125@gmail.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class GameModel : Model
{

    #region Attr
    public override string Name => StringDefine.M_GameModel;
    public bool IsPlay { get => m_IsPlay; set => m_IsPlay = value; }
    public bool IsPause { get => m_IsPause; set => m_IsPause = value; }
    public float MultiplyCnt { get => m_MultiplyCnt; set => m_MultiplyCnt = value; }
    public float MagnetCnt { get => m_MagnetCnt; set => m_MagnetCnt = value; }
    public float InvincibleCnt { get => m_InvincibleCnt; set => m_InvincibleCnt = value; }
    public float SkillTime { get => m_SkillTime; set => m_SkillTime = value; }
    #endregion

    #region Field
    private bool m_IsPlay = true;
    private bool m_IsPause = false;
    
    private float m_MultiplyCnt = 5f;
    private float m_MagnetCnt = 3.5f;
    private float m_InvincibleCnt = 5.0f;
    
    private float m_SkillTime = 5.0f;
    #endregion

    #region Mono

    #endregion

    #region Func
    public void Init()
    {
        m_MagnetCnt = 0f;
        m_MultiplyCnt = 0f;
        m_InvincibleCnt = 0f;
        m_SkillTime = 5f;
    }
    #endregion
}
