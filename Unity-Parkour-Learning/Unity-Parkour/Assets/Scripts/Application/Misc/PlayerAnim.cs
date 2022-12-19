/****************************************************
	文件：PlayerAnim.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System;
using UnityEngine;

public class PlayerAnim : View
{
	private Animation m_Anim;
	Action m_PlayedAnim;
    GameModel m_GameModel;
    public override string Name => StringDefine.V_PlayerAnim;

    private void Awake()
    {
		m_Anim = GetComponent<Animation>();
		m_PlayedAnim = PlayRun;
        m_GameModel = GetModel<GameModel>();

    }

    private void Update()
    {
        if (m_PlayedAnim != null)
        {
            if (m_GameModel != null && m_GameModel.IsPlay && !m_GameModel.IsPause)
                m_PlayedAnim();
            else
                m_Anim.Stop();
		}
    }

    private void PlayRun()
    {
		m_Anim.Play("run");
    }

	void PlayLeftJump()
    {
		m_Anim.Play("left_jump");
		if (m_Anim["left_jump"].normalizedTime > 0.95f)
        {
			m_PlayedAnim = PlayRun;
        }
	}

    void PlayRightJump()
    {
        m_Anim.Play("right_jump");
        if (m_Anim["right_jump"].normalizedTime > 0.95f)
        {
            m_PlayedAnim = PlayRun;
        }
    }

    void PlayRoll()
    {
        m_Anim.Play("roll");
        if (m_Anim["roll"].normalizedTime > 0.95f)
        {
            m_PlayedAnim = PlayRun;
        }
    }

    void PlayJump()
    {
        m_Anim.Play("jump");
        if (m_Anim["jump"].normalizedTime > 0.95f)
        {
            m_PlayedAnim = PlayRun;
        }
    }

    public void ChangeAnim(InputDirection dir)
    {
        switch (dir)
        {
            case InputDirection.NULL:
                break;
            case InputDirection.RIGHT:
                m_PlayedAnim = PlayRightJump;
                break;
            case InputDirection.LEFT:
                m_PlayedAnim = PlayLeftJump;
                break;
            case InputDirection.DOWN:
                m_PlayedAnim = PlayRoll;
                break;
            case InputDirection.UP:
                m_PlayedAnim = PlayJump;
                break;
        }
    }

    /// <summary>
    /// 射门动作的播放
    /// </summary>
    public void PlayGoalAnim()
    {
        m_PlayedAnim = PlayGoal;
    }

    private void PlayGoal()
    {
        m_Anim.Play("Shoot01");
        if (m_Anim["Shoot01"].normalizedTime > 0.95f)
        {
            m_PlayedAnim = PlayRun;
        }
    }

    public override void HandleEvent(string name, object data)
    {
        throw new NotImplementedException();
    }
}
