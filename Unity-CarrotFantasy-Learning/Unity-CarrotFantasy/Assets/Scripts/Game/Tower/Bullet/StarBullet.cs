/****************************************************
    文件：StarBullet.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/25 16:20:35
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class StarBullet : MonoBehaviour
{
    public int attackValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.activeSelf) return;

        if (collision.tag == "Monster" || collision.tag == "Item")
        {
            collision.SendMessage("TakeDamage", attackValue);
        }
    }
}
