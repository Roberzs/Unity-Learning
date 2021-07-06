/****************************************************
    文件：Bullet.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/24 19:36:04
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public struct BulletProperty
{
    public float debuffTime;
    public float debuffValue;
}

public class Bullet : MonoBehaviour
{
    public Transform targetTrans;
    public int moveSpeed;
    public int attackValue;
    public int towerID;
    public int towerLevel;

    protected virtual void Update()
    {
        // 如果游戏结束 将子弹对象丢进对象池
        if (GameController.Instance.gameOver)
        {
            DestoryBullet();
        }

        if (GameController.Instance.isPause)
        {
            return;
        }

        // 如果目标敌人已死亡 也将子弹对象丢进对象池
        if (targetTrans == null || !targetTrans.gameObject.activeSelf)
        {
            DestoryBullet();
            return;
        }

        // 子弹的移动和转向
        if (targetTrans.gameObject.tag == "Item")
        {
            transform.position = Vector3.Lerp(
                transform.position, 
                targetTrans.position + new Vector3(0, 0, 3),
                1 / Vector3.Distance(transform.position, targetTrans.position + new Vector3(0, 0, 3)) * Time.deltaTime * moveSpeed * GameController.Instance.gameSpeed);
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
        }
        else if (targetTrans.gameObject.tag == "Monster")
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetTrans.position,
                1 / Vector3.Distance(transform.position, targetTrans.position) * Time.deltaTime * moveSpeed * GameController.Instance.gameSpeed);
            transform.LookAt(targetTrans.position);
        }

        // 角度纠正
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles += new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        }
    }

    protected virtual void DestoryBullet()
    {
        targetTrans = null;
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + towerID.ToString() + "/Bullect/" + towerLevel.ToString(), gameObject);
    }

    protected virtual void CreateEffect()
    {
        GameObject effectGo = GameController.Instance.GetGameObjectResource("Tower/ID" + towerID.ToString() + "/Effect/" + towerLevel.ToString());
        effectGo.transform.position = transform.position;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
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
                    DestoryBullet();
                }
            }
        }
    }
}
