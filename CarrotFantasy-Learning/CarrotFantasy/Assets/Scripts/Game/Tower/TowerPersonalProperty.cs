/****************************************************
    文件：TowerPersonalProperty.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/24 17:39:30
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerPersonalProperty : MonoBehaviour
{
    // 属性
    //private Transform towerTrans;                             // 要跟随目标旋转的塔
    public int towerLevel;
    protected float timeVal;
    public float attackCD;

    public int price;
    [HideInInspector]
    public int sellPrice;
    [HideInInspector]
    public int upLevelPrice;

    // 引用
    public Transform targetTrans;       // 攻击目标
    public Tower tower;                 // 自身Tower
    public Animator animator;

    // 资源
    protected GameObject bulletGo;      // 子弹

    protected virtual void Start()
    {
        upLevelPrice = (int)(price * 1.5f);
        sellPrice = price / 2;
        animator = transform.Find("tower").GetComponent<Animator>();
        timeVal = attackCD;
    }

    protected virtual void Update()
    {
        // 游戏暂停或丢失目标
        if (GameController.Instance.isPause || targetTrans == null || GameController.Instance.gameOver) return;
        // 目标已失活（已死亡）
        if (!targetTrans.gameObject.activeSelf)
        {
            targetTrans = null;
            return;
        }
        // 攻击方法
        if (timeVal >= attackCD / GameController.Instance.gameSpeed)
        {
            timeVal = 0;
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }

        // 旋转以及旋转的纠正
        //transform.LookAt(targetTrans);
        if(targetTrans.gameObject.tag == "Item")
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

    public void Init()
    {
        tower = null;
    }

    public void SellTower()
    {
        GameController.Instance.PlayEffectMusic("NormalMordel/Tower/TowerSell");

        GameController.Instance.ChangeCoin(sellPrice);
        GameObject itemGo = GameController.Instance.GetGameObjectResource("BuildEffect");
        itemGo.transform.position = transform.position;
        DestoryTower();
    }

    public void UpLevelTower()
    {
        GameController.Instance.PlayEffectMusic("NormalMordel/Tower/TowerUpdata");


        GameObject effectGo = GameController.Instance.GetGameObjectResource("UpLevelEffect");
        effectGo.transform.position = GameController.Instance.selectGrid.transform.position;
        GameController.Instance.ChangeCoin(-sellPrice);
        DestoryTower();
    }

    protected virtual void DestoryTower()
    {
        tower.DestroyTower();
    }

    protected virtual void Attack()
    {
        if (targetTrans == null) return;

        GameController.Instance.PlayEffectMusic("NormalMordel/Tower/Attack/" + tower.towerID.ToString());

        animator.Play("Attack");
        bulletGo = GameController.Instance.GetGameObjectResource("Tower/ID" + tower.towerID.ToString() + "/Bullect/" + towerLevel.ToString());
        bulletGo.transform.position = transform.position;
        bulletGo.GetComponent<Bullet>().targetTrans = targetTrans;
    }
}
