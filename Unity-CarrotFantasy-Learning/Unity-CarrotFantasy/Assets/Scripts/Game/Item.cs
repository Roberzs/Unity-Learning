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
using UnityEngine.EventSystems;

public class Item : MonoBehaviour
{
    public GridPoint gridPoint;
    public int itemID = -1;

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
#if MapEditing
        GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("Mask").GetComponent<BoxCollider>().enabled = false;
#endif

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

    // 初始化
    private void InitItem()
    {
        prize = 1000 - 100 * itemID;
        HP = 1500 - 100 * itemID;
        currentHP = HP;
        timeVal = 3;
    }

#if GameRuning

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
        if (GameController.Instance.targetTrans = transform)
        {
            GameController.Instance.HideSignal();
        }

        // 奖励
        GameObject coinGo = GameController.Instance.GetGameObjectResource("CoinCanvas");
        coinGo.transform.Find("Emp_Coin").GetComponent<CoinMove>().prize = prize;
        coinGo.transform.SetParent(GameController.Instance.transform);
        coinGo.transform.position = transform.position;
        // 增加玩家金币数
        GameController.Instance.ChangeCoin(prize);
        // 特效
        GameObject effectGo = GameController.Instance.GetGameObjectResource("DestoryEffect");
        effectGo.transform.SetParent(GameController.Instance.transform);
        effectGo.transform.position = transform.position;

        GameController.Instance.PushGameObjectToFactory(GameController.Instance.mapMaker.bigLevelID.ToString() + "/Item/" + itemID, gameObject);
        gridPoint.gridState.hasItem = false;
        InitItem();

        GameController.Instance.PlayEffectMusic("NormalMordel/Item");
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

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
