/****************************************************
	文件：PlayerMove.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections;
using UnityEngine;

public class PlayerMove : View
{
    #region 属性
    public override string Name => StringDefine.V_PlayerMove;
    #endregion

    #region 字段
    [SerializeField] float moveSpeed = 20f;
    float moveXSpeed = 13f;
    private CharacterController m_Cc;
    private InputDirection m_InputDir = InputDirection.NULL;
    private bool activeInput = false;
    Vector3 m_MousePos = Vector3.zero;
    int m_nowIndex = 1;
    int m_TargetIndex = 1;
    float m_XDistance;

    #endregion

    #region 回调函数
    public override void HandleEvent(string name, object data)
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region Unity
    private void Awake()
    {
        m_Cc = GetComponent<CharacterController>();
    }

    private void Start()
    {
        StartCoroutine(UpdateAction());
    }

    #endregion

    #region 方法
    IEnumerator UpdateAction()
    {
        while (true)
        {
            m_Cc.Move(transform.forward * moveSpeed * Time.deltaTime);
            MoveToTargetPos();
            UpdateMoveDirection();
            yield return 0;
        }
    }

    private void GetInputDirection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            activeInput = true;
            m_MousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0) && activeInput)
        {
            Vector3 dir = Input.mousePosition - m_MousePos;
            if (dir.magnitude > 20)
            {
                if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y) && dir.x > 0)
                {
                    m_InputDir = InputDirection.RIGHT;
                }
                else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y) && dir.x < 0)
                {
                    m_InputDir = InputDirection.LEFT;
                }
                else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y) && dir.y > 0)
                {
                    m_InputDir = InputDirection.UP;
                }
                else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y) && dir.y < 0)
                {
                    m_InputDir = InputDirection.DOWN;
                }

                activeInput = false;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            activeInput = false;
        }

        /// 

        if (Input.GetKeyDown(KeyCode.W))
        {
            m_InputDir = InputDirection.UP;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            m_InputDir = InputDirection.DOWN;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            m_InputDir = InputDirection.LEFT;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            m_InputDir = InputDirection.RIGHT;
        }
    }
    void UpdateMoveDirection()
    {
        GetInputDirection();
        switch (m_InputDir)
        {
            case InputDirection.NULL:
                break;
            case InputDirection.RIGHT:
                if (m_TargetIndex < 2)
                {
                    m_TargetIndex++;
                    m_XDistance = 2;
                }
                break;
            case InputDirection.LEFT:
                if (m_TargetIndex > 0)
                {
                    m_TargetIndex--;
                    m_XDistance = -2;
                }
                break;
            case InputDirection.DOWN:
                break;
            case InputDirection.UP:
                break;
            default:
                break;
        }
        
    }

    void MoveToTargetPos()
    {
        if (m_TargetIndex != m_nowIndex)
        {
            float move = Mathf.Lerp(0, m_XDistance, moveXSpeed * Time.deltaTime);
            transform.position += new Vector3(move, 0, 0);
            m_XDistance -= move;
            if (Mathf.Abs(m_XDistance) < 0.05f)
            {
                m_XDistance = 0;
                m_nowIndex = m_TargetIndex;
                transform.position = new Vector3(-2f + 2 * m_nowIndex, transform.position.y, transform.position.z);
            }
        }
    }

    #endregion
}

