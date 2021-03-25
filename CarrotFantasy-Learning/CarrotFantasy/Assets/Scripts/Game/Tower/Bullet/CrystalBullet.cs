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
        base.Update();
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
