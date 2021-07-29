﻿/****************************************************
    文件：Monster.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/22 14:36:25
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{

    // 属性
    public int monsterID;
    public int HP;
    public int currentHP;

    
    public float initMoveSpeed;     // 初始速度与当前速度
    public float moveSpeed;

    public int prize;

    // 组件
    private Animator animator;
    private Slider slider;          // 用于显示血条
    public GameObject TshitGo;      // 便便组件 
    private List<Vector3> monsterPointList;

    // 计数器与开关
    private int roadPointIndex = 1; // 当前要到达点的索引
    private bool reachCarrot;       // 是否到达终点
    private bool hasDecreasSpeed;   // 是否被减速

    private float decreaseSpeedTimeVal;     // 减速计时器
    private float decreaseTime;             // 减速效果的持续时间

    // 相关资源
    public AudioClip dieAudioClip;      // 死亡音效
    private RuntimeAnimatorController runtimeAnimatorController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        slider = transform.Find("MonsterCanvas").Find("HpSlider").GetComponent<Slider>();
        slider.gameObject.SetActive(false);
        monsterPointList = GameController.Instance.mapMaker.monsterPathPos;
        TshitGo = transform.Find("TShit").gameObject;
    }

    private void OnEnable()
    {
        // 怪物的转向
        if (roadPointIndex + 1 < monsterPointList.Count)
        {
            float xOffset = monsterPointList[0].x - monsterPointList[1].x;
            if (xOffset < 0)
            {
                // 右转
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                // 左转
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            slider.gameObject.transform.eulerAngles = Vector3.zero;
        }
    }

    // 获取特异性属性
    public void GetMonsterProperty()
    {
        runtimeAnimatorController = GameController.Instance.controllers[monsterID - 1];
        animator.runtimeAnimatorController = runtimeAnimatorController;
    }

#if GameRuning

    private void Update()
    {
        if (GameController.Instance.isPause || GameController.Instance.gameOver)
        {
            return;
        }

        // 没有到达终点与到达终点
        if (!reachCarrot)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                monsterPointList[roadPointIndex], 
                1.0f / Vector3.Distance(transform.position, monsterPointList[roadPointIndex]) * Time.deltaTime * moveSpeed * GameController.Instance.gameSpeed);
            if (Vector3.Distance(transform.position, monsterPointList[roadPointIndex]) <= 0.01f)
            {
                // 怪物的转向
                if (roadPointIndex + 1 < monsterPointList.Count)
                {
                    float xOffset = monsterPointList[roadPointIndex].x - monsterPointList[roadPointIndex + 1].x;
                    if (xOffset < 0)
                    {
                        // 右转
                        transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        // 左转
                        transform.eulerAngles = new Vector3(0, 180, 0);
                    }
                    slider.gameObject.transform.eulerAngles = Vector3.zero;
                }
                roadPointIndex++;
                if (roadPointIndex >= monsterPointList.Count)
                {
                    reachCarrot = true;
                }
            }
        }
        else
        {
            DestoryMonster();
            // 萝卜减血
            GameController.Instance.DecreaseHP();
        }

        if (hasDecreasSpeed)
        {
            decreaseSpeedTimeVal += Time.deltaTime;
        }
        if (decreaseSpeedTimeVal >= decreaseTime)
        {
            CancelDecreaseDebuff();
            decreaseSpeedTimeVal = 0;
        }
    }

    // 初始化方法
    private void InitMonsterGo()
    {
        monsterID = 0;
        HP = 0;
        currentHP = 0;
        moveSpeed = 0;
        roadPointIndex = 1;
        dieAudioClip = null;
        reachCarrot = false;
        slider.value = 1;
        slider.gameObject.SetActive(false);
        prize = 0;
        transform.eulerAngles = new Vector3(0, 0, 0);
        decreaseSpeedTimeVal = 0;
        decreaseTime = 0;
        CancelDecreaseDebuff();
    }

    // 受伤减血
    private void TakeDamage(int attackValue)
    {
        slider.gameObject.SetActive(true);
        currentHP = Mathf.Max(currentHP - attackValue, 0);
        if (currentHP <= 0)
        {
            // 播放音效
            GameController.Instance.PlayEffectMusic("NormalMordel/Monster/"+ GameController.Instance.currentStage.mBigLevelID.ToString()+ "/" + monsterID.ToString());

            DestoryMonster();
            return;
        }
        slider.value = (float)currentHP / HP;
    }



    // 销毁
    private void DestoryMonster()
    {
        if (GameController.Instance.targetTrans == transform)
        {
            GameController.Instance.HideSignal();
        }

        if (!reachCarrot)
        {
            // 没有到达终点 即被玩家杀死

            // 生成金币
            GameObject coinGo = GameController.Instance.GetGameObjectResource("CoinCanvas");
            coinGo.transform.Find("Emp_Coin").GetComponent<CoinMove>().prize = prize;
            coinGo.transform.SetParent(GameController.Instance.transform);
            coinGo.transform.position = transform.position;
            // 增加玩家金币数
            GameController.Instance.ChangeCoin(prize);
            // 随机奖励的掉落
            int randomNum = UnityEngine.Random.Range(0, 100);
            if (randomNum < 10)
            {
                GameObject prizeGo = GameController.Instance.GetGameObjectResource("Prize");
                prizeGo.transform.position = transform.position;
                GameController.Instance.PlayEffectMusic("NormalMordel/GiftCreate");
            }
        }

        // 销毁特效
        GameObject effectGo = GameController.Instance.GetGameObjectResource("DestoryEffect");
        effectGo.transform.SetParent(GameController.Instance.transform);
        effectGo.transform.position = transform.position;

        GameController.Instance.killMonsterNum++;
        GameController.Instance.killMonsterTotalNum++;

        InitMonsterGo();
        GameController.Instance.PushGameObjectToFactory("MonsterPrefab", gameObject);
    }

    // 减速buff
    private void DecreaseSpeed(BulletProperty bulletProperty)
    {
        if (!hasDecreasSpeed)
        {
            moveSpeed -= bulletProperty.debuffValue;
            TshitGo.SetActive(true);
        }
        decreaseSpeedTimeVal = 0;
        hasDecreasSpeed = true;
        decreaseTime = bulletProperty.debuffTime;

    }

    // 取消自身Debuff
    private void CancelDecreaseDebuff()
    {
        hasDecreasSpeed = false;
        moveSpeed = initMoveSpeed;
        TshitGo.SetActive(false);
    }

    


    private void OnMouseDown()
    {
        if (GameController.Instance.targetTrans == null)
        {
            GameController.Instance.targetTrans = transform;
            GameController.Instance.ShowSignal();
        }
        else if (GameController.Instance.targetTrans != transform)
        {
            // 更换目标
            GameController.Instance.HideSignal();
            GameController.Instance.targetTrans = transform;
            GameController.Instance.ShowSignal();
        }
        else if (GameController.Instance.targetTrans == transform)
        {
            // 取消目标
            GameController.Instance.HideSignal();
        }
    }
#endif

}