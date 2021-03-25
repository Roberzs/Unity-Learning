/****************************************************
    文件：MonsterBuilder.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/22 14:38:30
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBuilder : IBuilder<Monster>
{
    public int m_MonsterID;
    private GameObject monsterGo;

    public void GetData(Monster productClassGo)
    {
        productClassGo.monsterID = m_MonsterID;
        productClassGo.HP = m_MonsterID * 100;
        productClassGo.currentHP = productClassGo.HP;
        productClassGo.moveSpeed = m_MonsterID;
        productClassGo.initMoveSpeed = m_MonsterID;
        productClassGo.prize = m_MonsterID * 50;
    }

    public void GetOtherResource(Monster productClassGo)
    {
        productClassGo.GetMonsterProperty();
    }

    public GameObject GetProduct()
    {
        GameObject itemGo = GameController.Instance.GetGameObjectResource("MonsterPrefab");
        Monster monster = GetProductClass(itemGo);
        GetData(monster);
        GetOtherResource(monster);
        return itemGo;
    }

    public Monster GetProductClass(GameObject gameObject)
    {
        return gameObject.GetComponent<Monster>();
    }
}
