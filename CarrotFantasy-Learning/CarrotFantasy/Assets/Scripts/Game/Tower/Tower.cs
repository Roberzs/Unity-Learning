/****************************************************
    文件：Tower.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/23 22:26:48
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int towerID;
    private CircleCollider2D circleCollider2D;              // 攻击范围检测
    private TowerPersonalProperty towerPersonalProperty;    // 塔的特异属性功能
    private SpriteRenderer attackRangeSr;                   // 塔的攻击范围渲染器
    public bool isTarget;           // 有集火目标
    public bool hasTarget;          // 存在目标

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (isTarget)
        {
            if (towerPersonalProperty.targetTrans != GameController.Instance.targetTrans)
            {
                towerPersonalProperty.targetTrans = null;
                hasTarget = false;
                isTarget = false;
            }
        }
        if (hasTarget)
        {
            if (!towerPersonalProperty.targetTrans.gameObject.activeSelf)
            {
                towerPersonalProperty.targetTrans = null;
                hasTarget = false;
                isTarget = false;
            }
        }
    }

    // 初始化
    private void Init()
    {
        
        circleCollider2D = GetComponent<CircleCollider2D>();
        towerPersonalProperty = GetComponent<TowerPersonalProperty>();
        towerPersonalProperty.tower = this;
        attackRangeSr = transform.Find("attackRange").GetComponent<SpriteRenderer>();
        attackRangeSr.gameObject.SetActive(false);

        circleCollider2D.radius = 1.65f * (towerPersonalProperty.towerLevel);
        attackRangeSr.transform.localScale = new Vector3(1.5f * towerPersonalProperty.towerLevel, 1.5f * towerPersonalProperty.towerLevel, 1);
        isTarget = false;
        hasTarget = false;
    }

    public void GetTowerProperty()
    {

    }

    public void DestroyTower()
    {
        towerPersonalProperty.Init();
        Init();
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + towerID.ToString() + "/TowerSet/" + towerPersonalProperty.towerLevel, gameObject);
    }

    /// <summary>
    /// 触发器事件 (塔的攻击)
    /// </summary>

    private void OnTriggerEnter2D(Collider2D collision)
    {
        JudgeTarget(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        JudgeTarget(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (towerPersonalProperty.targetTrans == collision.transform)
        {
            towerPersonalProperty.targetTrans = null;
            hasTarget = false;
            isTarget = false;
        }
    }

    // 对范围内目标的判断
    private void JudgeTarget(Collider2D collision)
    {
        if (collision.tag != "Monster" && collision.tag != "Item" && isTarget) return;

        Transform targetTrans = GameController.Instance.targetTrans;    // 获取集火目标

        if (targetTrans != null)        // 有集火目标
        {
            if (!isTarget)      // 集火目标没有进入攻击范围
            {
                if (collision.tag == "Item" && collision.transform == targetTrans)      // 发现集火目标且集火目标是一个道具
                {
                    towerPersonalProperty.targetTrans = collision.transform;
                    isTarget = true;
                    hasTarget = true;
                }
                else if (collision.tag == "Monster")      // 一个怪物进入攻击范围
                {
                    if (collision.transform == targetTrans)  // 这个怪物是集火目标
                    {
                        towerPersonalProperty.targetTrans = collision.transform;
                        isTarget = true;
                        hasTarget = true;
                    }
                    else if (collision.transform != targetTrans && !hasTarget)    // 不是集火目标且当前没有攻击目标
                    {
                        towerPersonalProperty.targetTrans = collision.transform;
                        hasTarget = true;
                    }
                }
            }
        }
        else  // 没有集火目标
        {

            if (!hasTarget && collision.tag == "Monster")
            {
                towerPersonalProperty.targetTrans = collision.transform;
                hasTarget = true;
            }
        }
    }
}
