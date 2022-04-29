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
    private const float rollAnimTimeLength = 0.733f;
    #endregion

    #region 字段
    private CharacterController m_Cc;
    private InputDirection m_InputDir = InputDirection.NULL;
    private bool activeInput = false;
    private Vector3 m_MousePos = Vector3.zero;
    private int m_nowIndex = 1;
    private int m_TargetIndex = 1;
    public float m_TargetX = 0;
    public float m_TargetY = 0;
    private bool isRolling = false;
    private bool isHiting = false;
    private float rollAnimTimer;
    private GameModel mGameModel;
    private float moveSpeed = 20f;
    private float CurrMoveSpeed;
    private float AcceleSpeed = 3;
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
        mGameModel = GetModel<GameModel>();
    }

    private void Start()
    {
        StartCoroutine(UpdateAction());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagDefine.SmallFance))
        {
            HitObstacles(other.gameObject);
        }
        if (other.CompareTag(TagDefine.MediumFance))
        {

        }
        if (other.CompareTag(TagDefine.BigFance))
        {
            if (isRolling)
                return;
            HitObstacles(other.gameObject);
        }
    }

    #endregion

    #region 方法

    #region 移动相关
    IEnumerator UpdateAction()
    {
        //Physics.autoSyncTransforms = true;
        while (true)
        {
            if (mGameModel != null && mGameModel.IsPlay && !mGameModel.IsPause)
            {
                if (!m_Cc.isGrounded)
                    m_TargetY -= m_Grivaty * Time.deltaTime;
                // YZ 轴移动
                m_Cc.Move((transform.forward * moveSpeed + new Vector3(0, m_TargetY, 0)) * Time.deltaTime);
                // X 轴移动
                MoveToXTargetPos();
                // 更新输入
                UpdateMoveDirection();
                UpdateRollState();
            }
            
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
                    m_TargetX += 2;
                    GameRoot.Instance.soundManager.PlayEffectAudio(StringDefine.S_Huadong);
                }
                break;
            case InputDirection.LEFT:
                if (m_TargetIndex > 0)
                {
                    m_TargetIndex--;
                    m_TargetX -= 2;
                    GameRoot.Instance.soundManager.PlayEffectAudio(StringDefine.S_Huadong);
                }
                break;
            case InputDirection.DOWN:
                isRolling = true;
                rollAnimTimer = rollAnimTimeLength;
                GameRoot.Instance.soundManager.PlayEffectAudio(StringDefine.S_Slide);
                break;
            case InputDirection.UP:
                if (m_Cc.isGrounded)
                {
                    m_TargetY = 5;
                    GameRoot.Instance.soundManager.PlayEffectAudio(StringDefine.S_Jump);
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

    void MoveToXTargetPos()
    {
        if (m_TargetIndex != m_nowIndex)
        {
            float targetPos = Mathf.Lerp(transform.position.x, m_TargetX, 1 / Mathf.Abs(transform.position.x - m_TargetX) * moveXYSpeed * Time.deltaTime);
            transform.position = new Vector3(targetPos, transform.position.y, transform.position.z);
            if (Mathf.Abs(transform.position.x - m_TargetX) < 0.05f)
            {
                m_nowIndex = m_TargetIndex;
                transform.position = new Vector3(m_TargetX, transform.position.y, transform.position.z);
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

    /// <summary>
    /// 撞到障碍物
    /// </summary>
    private void HitObstacles(GameObject obstacles)
    {
        obstacles.SendMessage("HitPlayer", SendMessageOptions.DontRequireReceiver);
        // 保护最大速度
        if (!isHiting)
        {
            isHiting = true;
            CurrMoveSpeed = moveSpeed;
            StartCoroutine(RecoverSpeed());
        }
        moveSpeed = 0.0f;   
    }

    IEnumerator RecoverSpeed()
    {
        while (moveSpeed <= CurrMoveSpeed)
        {
            moveSpeed += AcceleSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isHiting = false;
        moveSpeed = CurrMoveSpeed;
    }


    #endregion
}
