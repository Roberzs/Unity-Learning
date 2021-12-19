/****************************************************
    文件：PlayerController.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/4/5 17:6:32
	功能：角色控制器
*****************************************************/

using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    private Transform camTrans;
    private Vector3 camOffset;
    public Animator ani;
    public CharacterController ctrl;

    private Vector2 dir = Vector2.zero;
    public Vector2 Dir 
    { 
        get => dir;

        set 
        {
            if (value == Vector2.zero)
            {
                isMove = false;
                
            }
            else
            {
                isMove = true;
            }
            dir = value;
        }
    }

    private bool isMove = false;

    private float targetBlend;
    private float currentBlend;

    public void Init()
    {
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;
    }

    private void Update()
    {
 
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 _dir = new Vector2(h, v).normalized;
        if (_dir != Vector2.zero)
        {
            Dir = _dir;
            SetBlend(Constants.BlendWalk);
        }
        else
        {
            Dir = Vector2.zero;
            SetBlend(Constants.BlendIdle);
        }


        if (currentBlend != targetBlend)
        {
            UpdateMixBlend();
        }

        if (isMove)
        {
            // 设置移动方向
            SetDir();
            // 移动
            SetMove();
            // 相机跟随
            SetCam();
        }

    }

    private void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1));// + camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }

    public void SetCam()
    {
        if (camTrans != null)
        {
            camTrans.position = transform.position - camOffset;
        }
    }

    public void SetBlend(float blend)
    {
        targetBlend = blend;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currentBlend - targetBlend) < Constants.AccelerSpeed * Time.deltaTime)
        {
            currentBlend = targetBlend;
        }
        else if (currentBlend > targetBlend)
        {
            currentBlend -= Constants.AccelerSpeed * Time.deltaTime;
        }
        else
        {
            currentBlend += Constants.AccelerSpeed * Time.deltaTime;
        }
        ani.SetFloat("Blend", currentBlend);
    }
}