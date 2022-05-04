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
    public float MultiplyTime { get => m_MultiplyTime; set => m_MultiplyTime = value; }
    public float MagnetTime { get => m_MagnetTime; set => m_MagnetTime = value; }
    public float Invincible { get => m_Invincible; set => m_Invincible = value; }
    #endregion

    #region Field
    private bool m_IsPlay = true;
    private bool m_IsPause = false;
    private float m_MultiplyTime = 5f;
    private float m_MagnetTime = 3.5f;
    private float m_Invincible = 5.0f;
    #endregion

    #region Mono

    #endregion

    #region Func

    #endregion
}
