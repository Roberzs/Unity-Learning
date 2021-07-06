/****************************************************
    文件：WindmallBullet.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/25 16:53:47
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class WindmallBullet : Bullet
{
    private bool hasTarget;
    private float timeVal;

    private void OnEnable()
    {
        hasTarget = false;
        timeVal = 0;
    }

    protected override void Update()
    {
        if (GameController.Instance.gameOver || timeVal >= 2.5f)
        {
            DestoryBullet();
        }
        if (GameController.Instance.isPause) return;
        if (timeVal < 2.5)
        {
            timeVal += Time.deltaTime;
        }
        if (hasTarget)
        {
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            if (targetTrans != null && targetTrans.gameObject.activeSelf)
            {
                hasTarget = true;
                InitTarget();
            }
        }
        
    }

    private void InitTarget()
    {
        // 旋转以及旋转的纠正
        //transform.LookAt(targetTrans);
        if (targetTrans.gameObject.tag == "Item")
        {
            // 因为Item道具不与炮台处于同一Z轴坐标  所以要对其位置进行修正
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
        }
        else
        {
            transform.LookAt(targetTrans.position);
        }
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles += new Vector3(transform.eulerAngles.x, -90, transform.eulerAngles.z);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster" || collision.tag == "Item")
        {
            if (collision.gameObject.activeSelf)
            {
                if (targetTrans == null || (collision.tag == "Item" && GameController.Instance.targetTrans == null))
                {
                    return;
                }
                if (collision.tag == "Monster" || (collision.tag == "Item" && GameController.Instance.targetTrans == collision.transform))
                {
                    collision.SendMessage("TakeDamage", attackValue);
                    CreateEffect();
                }
            }
        }
    }

}
