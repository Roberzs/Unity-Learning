using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 水晶子弹(电击)
/// </summary>
public class CrystalBullet : Bullet {

    private float attackTimeVal;
    private bool canTakeDamage;

	
	protected override void Update () {

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
            //transform.position = Vector3.Lerp(
            //    transform.position,
            //    targetTrans.position + new Vector3(0, 0, 3),
            //    1 / Vector3.Distance(transform.position, targetTrans.position + new Vector3(0, 0, 3)) * Time.deltaTime * moveSpeed * GameController.Instance.gameSpeed);
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
        }
        else if (targetTrans.gameObject.tag == "Monster")
        {
            //transform.position = Vector3.Lerp(
                //transform.position,
                //targetTrans.position,
                //1 / Vector3.Distance(transform.position, targetTrans.position) * Time.deltaTime * moveSpeed * GameController.Instance.gameSpeed);
            transform.LookAt(targetTrans.position);
        }

        // 角度纠正
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles += new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        }

        if (!canTakeDamage)
        {
            attackTimeVal += Time.deltaTime;
            if (attackTimeVal>=0.5f-towerLevel*0.15f)
            {
                canTakeDamage = true;
                attackTimeVal = 0;
                DecreaseHP();
            }
        }
       
	}

    private void DecreaseHP()
    {
        if (!canTakeDamage||targetTrans==null)
        {
            return;
        }
        if (targetTrans.gameObject.activeSelf)
        {
            if (targetTrans.position==null||(targetTrans.tag=="Item"&&GameController.Instance.targetTrans==null))
            {
                return;
            }
            if (targetTrans.tag=="Monster"|| (targetTrans.tag == "Item" && GameController.Instance.targetTrans.gameObject.tag == "Item"))
            {
                targetTrans.SendMessage("TakeDamage",attackValue);
                CreateEffect();
                canTakeDamage = false;
            }
        }
    }

    protected override void CreateEffect()
    {
        if (targetTrans.position==null)
        {
            return;
        }
        GameObject effectGo = GameController.Instance.GetGameObjectResource("Tower/ID"+towerID.ToString
            ()+"/Effect/"+towerLevel.ToString());
        effectGo.transform.position = targetTrans.position;

    }
}
