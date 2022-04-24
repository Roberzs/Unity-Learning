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
    private const float m_Grivaty = 9.81f;
    private const float moveXYSpeed = 15f;
    private const float moveSpeed = 20f;
    private const float rollAnimTimeLength = 0.733f;
    #endregion

    #region 字段
    private CharacterController m_Cc;
    private InputDirection m_InputDir = InputDirection.NULL;
    private bool activeInput = false;
    private Vector3 m_MousePos = Vector3.zero;
    private int m_nowIndex = 1;
    private int m_TargetIndex = 1;
    private Vector2 m_XYTargetPos = new Vector2(0.0f, 0.0f);
    private bool isRolling = false;
    private float rollAnimTimer;
    #endregion

    #region 回调
    public override void HandleEvent(string name, object data)
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region Mono
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
        //Physics.autoSyncTransforms = true;
        while (true)
        {
            // 向Z轴移动
            //m_Cc.Move(Vector3.forward * moveSpeed * Time.deltaTime);
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
            // XY轴移动
            MoveToXYTargetPos();
            // 更新输入
            UpdateMoveDirection();
            UpdateRollState();
            yield return 0;
        }
    }

    /// <summary>
    /// 获取输入方向
    /// </summary>
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

    /// <summary>
    /// 根据输入方向更新目标点位置
    /// </summary>
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
                    m_XYTargetPos.x += 2;
                }
                break;
            case InputDirection.LEFT:
                if (m_TargetIndex > 0)
                {
                    m_TargetIndex--;
                    m_XYTargetPos.x -= 2;
                }
                break;
            case InputDirection.DOWN:
                isRolling = true;
                rollAnimTimer = rollAnimTimeLength;
                break;
            case InputDirection.UP:
                if (m_XYTargetPos.y == 0)
                {
                    m_XYTargetPos.y += 5;
                }
                break;
            default:
                break;
        }
        if (m_InputDir != InputDirection.NULL)
        {
            SendMessage("ChangeAnim", m_InputDir, SendMessageOptions.DontRequireReceiver);
        }
        m_InputDir = InputDirection.NULL;
    }

    void MoveToXYTargetPos()
    {
        if (m_TargetIndex != m_nowIndex || m_XYTargetPos.y != 0)
        {
            if (m_XYTargetPos.y != 0)
            {
                m_XYTargetPos.y -= m_Grivaty * Time.deltaTime;
            }
            Vector3 targetPos = new Vector3(m_XYTargetPos.x, m_XYTargetPos.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, moveXYSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            {
                m_nowIndex = m_TargetIndex;
                transform.position = targetPos;
            }
            if (m_XYTargetPos.y < -0.005f)
            {
                // 投机取巧 没想到好办法判断是跳起状态还是下落状态 但是当Y是负值时一定是下落
                m_XYTargetPos.y = 0;
                transform.position = targetPos;
            }
        }
    }

    /// <summary>
    /// 更新翻滚状态
    /// </summary>
    void UpdateRollState()
    {
        if (isRolling)
        {
            rollAnimTimer -= Time.deltaTime;
            if (Mathf.Abs(rollAnimTimer) < 0.05f)
            {
                rollAnimTimer = 0;
                isRolling = false;
            }
        }
    }

    #endregion
}

