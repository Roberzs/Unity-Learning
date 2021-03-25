/****************************************************
    文件：Item.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/21 18:46:19
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public GridPoint gridPoint;
    public int itemID = -1;

    private GameController gameController;

    private int prize;
    private int HP;
    private int currentHP;
    private Slider slider;

    private float timeVal;  // 计时器 显示隐藏血条
    private bool showHP;

    private void OnEnable()
    {
        if (itemID != -1)
        {
            InitItem();
        }
    }

    private void Start()
    {
        gameController = GameController.Instance;
        slider = transform.Find("ItemCanvas").Find("HpSlider").GetComponent<Slider>();
        slider.gameObject.SetActive(false);
        GetComponent<Rigidbody2D>().sleepMode = RigidbodySleepMode2D.NeverSleep;
        InitItem();
    }

    private void Update()
    {
        if (showHP)
        {
            if (timeVal <= 0 )
            {
                slider.gameObject.SetActive(false);
                showHP = false;
                timeVal = 3;
            }
            else
            {
                timeVal -= Time.deltaTime;
            }
        }
    }

#if GameRuning

    // 初始化
    private void InitItem()
    {
        prize = 1000 - 100 * itemID;
        HP = 1500 - 100 * itemID;
        currentHP = HP;
        timeVal = 3;
    }

    private void TakeDamage(int attackValue)
    {
        slider.gameObject.SetActive(true);
        currentHP = Mathf.Max(currentHP - attackValue, 0);
        if (currentHP <= 0)
        {
            DestroyItem();
            return;

        }
        slider.value = (float)currentHP / HP;
        showHP = true;
        timeVal = 3;
    }

    private void DestroyItem()
    {
        if (gameController.targetTrans = transform)
        {
            gameController.HideSignal();
        }

        // 奖励
        GameObject coinGo = gameController.GetGameObjectResource("CoinCanvas");
        coinGo.transform.Find("Emp_Coin").GetComponent<CoinMove>().prize = prize;
        coinGo.transform.SetParent(gameController.transform);
        coinGo.transform.position = transform.position;
        // 增加玩家金币数
        gameController.ChangeCoin(prize);
        // 特效
        GameObject effectGo = gameController.GetGameObjectResource("DestoryEffect");
        effectGo.transform.SetParent(gameController.transform);
        effectGo.transform.position = transform.position;

        gameController.PushGameObjectToFactory(gameController.mapMaker.bigLevelID.ToString() + "/Item/" + itemID, gameObject);
        gridPoint.gridState.hasItem = false;
        InitItem();
    }

    private void OnMouseDown()
    {
        if (gameController.targetTrans == null)
        {
            gameController.targetTrans = transform;
            gameController.ShowSignal();
        }
        else if (gameController.targetTrans != transform)
        {
            // 更换目标
            gameController.HideSignal();
            gameController.targetTrans = transform;
            gameController.ShowSignal();
        }
        else if (gameController.targetTrans == transform)
        {
            // 取消目标
            gameController.HideSignal();
        }
    }
#endif
}
