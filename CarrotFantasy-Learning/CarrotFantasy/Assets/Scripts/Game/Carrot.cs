/****************************************************
    文件：Carrot.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/20 16:23:47
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carrot : MonoBehaviour
{
    // 萝卜不同状态的sprite
    private Sprite[] sprites;
    private Animator animator;
    private float timeVal;
    private SpriteRenderer sr;
    private Text hpText;

    private void Start()
    {
        sprites = new Sprite[7];
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i] = GameController.Instance.GetSprite("NormalMordel/Game/Carrot/" + i.ToString());
        }
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        hpText = transform.Find("HpCanvas").Find("Text").GetComponent<Text>();

    }

    private void Update()
    {
        if (GameController.Instance.carrotHp < 10)
        {
            animator.enabled = false;
            return;
        }

        if (timeVal >= 10)
        {
            animator.Play("Idle");
            timeVal = 0;
        }
        else
        {
            timeVal += Time.deltaTime;
        }
    }

    private void OnMouseDown()
    {
        if (GameController.Instance.carrotHp >= 10)
        {
            animator.Play("Touch");
        }
    }

    public void UpdateCarrotUI()
    {
        int hp = GameController.Instance.carrotHp;
        hpText.text = hp.ToString();
        if (hp>=7 && hp < 10)
        {
            sr.sprite = sprites[6];
        }
        else if (hp < 7 && hp > 0)
        {
            sr.sprite = sprites[hp - 1];
        }
        else
        {
            // 游戏结束
        }
    }
}
