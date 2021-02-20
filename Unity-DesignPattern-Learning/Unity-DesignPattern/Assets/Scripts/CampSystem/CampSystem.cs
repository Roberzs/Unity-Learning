/****************************************************
    文件：ICamp.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/19 23:12:37
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class CampSystem : IGameSystem
{
    private Dictionary<SoldierType, SoldierCamp> mSoldierCamps = new Dictionary<SoldierType, SoldierCamp>();

    public override void Init()
    {
        base.Init();

        InitCamp(SoldierType.Rookie);
        InitCamp(SoldierType.Sergeant);
        InitCamp(SoldierType.Captain);
    }

    public override void Update()
    {
        // 更新所有兵营状态
        foreach (var camp in mSoldierCamps.Values)
        {
            camp.Update();
        }
    }

    // 初始化兵营
    private void InitCamp(SoldierType soldierType)
    {
        string gameObjectName = null;
        string name = null;
        string iconSprite = null;
        float trainTime = 0f;
        switch (soldierType)
        {
            case SoldierType.Rookie:
                gameObjectName = "SoldierCamp_Rookie";
                name = "新手兵营";
                iconSprite = "RookieCamp";
                trainTime = 3f;
                break;
            case SoldierType.Sergeant:
                gameObjectName = "SoldierCamp_Sergeant";
                name = "中士兵营";
                iconSprite = "SergeantCamp";
                trainTime = 4f;
                break;
            case SoldierType.Captain:
                gameObjectName = "SoldierCamp_Captain";
                name = "上尉兵营";
                iconSprite = "CaptainCamp";
                trainTime = 5f;
                break;
            default:
                Debug.LogError("无法根据战士类型" + soldierType + "初始化兵营");
                break;
        }

        GameObject gameObject = GameObject.Find(gameObjectName);
        Vector3 position = UnityTool.FindChild(gameObject, "TrainPoint").transform.position;
        SoldierCamp camp = new SoldierCamp(gameObject, name, iconSprite, soldierType, position, trainTime);

        // 添加并设置点击脚本
        gameObject.AddComponent<CampOnClick>().Camp = camp;

        mSoldierCamps.Add(soldierType, camp);
    }
}
