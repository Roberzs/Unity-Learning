/****************************************************
    文件：AchievementMemento.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/24 14:15:23
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementMemento
{
    public int enemykilledCount { get; set; }
    public int soldierKilledCount { get; set; }
    public int maxStageLv { get; set; }

    public void SaveData()
    {
        PlayerPrefs.SetInt("EnemykilledCount", enemykilledCount);
        PlayerPrefs.SetInt("SoldierKilledCount", soldierKilledCount);
        PlayerPrefs.SetInt("MaxStageLv", maxStageLv);
    }

    public void LoadData()
    {
        enemykilledCount = PlayerPrefs.GetInt("EnemykilledCount");
        soldierKilledCount = PlayerPrefs.GetInt("SoldierKilledCount");
        maxStageLv = PlayerPrefs.GetInt("MaxStageLv");
    }

}
