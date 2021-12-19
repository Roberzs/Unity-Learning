/****************************************************
    文件：BattleSys.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/12/14 22:15:49
    功能：战斗业务系统
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSys : SystemRoot
{
    public static BattleSys Instance = null;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init BattleSys");
    }

    /// <summary>
    /// 开启一场战斗
    /// </summary>
    /// <param name="mapId"></param>
    public void StartBattle(int mapId)
    {
        GameObject go = new GameObject
        {
            name = "BattleRoot"
        };

        go.transform.SetParent(GameRoot.Instance.transform);
        BattleMgr battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.Init(mapId);
    }
}
