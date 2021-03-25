/****************************************************
    文件：TowerBuilder.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/23 22:25:38
    功能：塔的建造者
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : IBuilder<Tower>
{
    public int m_TowerID;
    private GameObject towerGo;
    public int m_TowerLevel;

    public void GetData(Tower productClassGo)
    {
        productClassGo.towerID = m_TowerID;
    }

    public void GetOtherResource(Tower productClassGo)
    {
        productClassGo.GetTowerProperty();
    }

    public GameObject GetProduct()
    {
        GameObject gameObject = GameController.Instance.GetGameObjectResource("Tower/ID" + m_TowerID.ToString() + "/TowerSet/" + m_TowerLevel.ToString());
        Tower tower = GetProductClass(gameObject);
        GetData(tower);
        GetOtherResource(tower);
        return gameObject;
    }

    public Tower GetProductClass(GameObject gameObject)
    {
        return gameObject.GetComponent<Tower>();
    }
}
