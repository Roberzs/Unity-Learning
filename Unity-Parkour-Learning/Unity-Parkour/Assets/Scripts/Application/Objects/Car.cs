/****************************************************
	文件：Car.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class Car : Obstacles
{
    public bool IsCanMove = false;
    private bool isHitTrigger = false;
    private float moveSpeed = 12.0f;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void HitPlayer()
    {
        base.HitPlayer();
    }

    /// <summary>
    /// 激活车辆进行移动
    /// </summary>
    public void HitTrigger()
    {
        if (IsCanMove)
        {
            isHitTrigger = true;
        }
    }

    private void Update()
    {
        if (isHitTrigger)
        {
            transform.Translate(-transform.forward * moveSpeed * Time.deltaTime);
        }
    }
}
