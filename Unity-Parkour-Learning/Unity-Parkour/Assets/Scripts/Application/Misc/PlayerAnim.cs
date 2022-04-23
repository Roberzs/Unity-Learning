/****************************************************
	文件：PlayerAnim.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
	private Animation m_Anim;
	Action m_PlayedAnim;

    private void Awake()
    {
		m_Anim = GetComponent<Animation>();
		m_PlayedAnim = PlayRun;

	}

    private void Update()
    {
        if (m_PlayedAnim != null)
        {
			m_PlayedAnim();
			//m_PlayedAnim = null;

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
}
