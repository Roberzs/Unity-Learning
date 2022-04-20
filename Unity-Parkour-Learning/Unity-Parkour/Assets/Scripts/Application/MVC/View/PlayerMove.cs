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
    private CharacterController m_Cc;
    private InputDirection m_InputDir = InputDirection.NULL;
    private bool activeInput = false;
    Vector3 m_MousePos = Vector3.zero;
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
            GetInputDirection();
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
    }
    #endregion
}

