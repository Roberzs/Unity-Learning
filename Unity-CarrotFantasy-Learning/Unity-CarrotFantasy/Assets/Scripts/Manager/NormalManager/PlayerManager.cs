/****************************************************
    文件：PlayerManager.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/4 11:15:52
    功能：玩家管理 负责存储、加载各种游戏信息
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    // 游戏数据统计
    public int adventrueModelNum;
    public int burriedLevelNum;
    public int bossModelNum;
    public int coin;
    public int killMonsterNum;
    public int killBossNum;
    public int clearItemNum;

    public List<bool> unLockedNormalModelBigLevelList;
    public List<Stage> unLockedNormalModelLevelList;
    public List<int> unLockedNormalModelLevelNum;

    // 怪物窝
    public int cookies;
    public int milk;
    public int nest;
    public int diamands;
    public List<MonsterPetData> monsterPetDataList;     // 宠物的喂养信息


    // 用于测试
    //public PlayerManager()
    //{
    //    adventrueModelNum = 0;
    //    burriedLevelNum = 0;
    //    bossModelNum = 0;
    //    coin = 0;
    //    killMonsterNum = 0;
    //    killBossNum = 0;
    //    clearItemNum = 0;
    //    cookies = 100;
    //    milk = 100;
    //    nest = 1;
    //    diamands = 1000;
    //    unLockedNormalModelLevelNum = new List<int>()
    //        {
    //            1,0,0
    //        };
    //    unLockedNormalModelBigLevelList = new List<bool>()
    //        {
    //            true,false,false
    //        };
    //    unLockedNormalModelLevelList = new List<Stage>()
    //        {
    //               new Stage(10,1,new int[]{ 1},false,0,1,1,true,false),
    //               new Stage(9,1,new int[]{ 2},false,0,2,1,false,false),
    //               new Stage(8,2,new int[]{ 1,2},false,0,3,1,false,false),
    //               new Stage(10,1,new int[]{ 3},false,0,4,1,false,false),
    //               new Stage(9,3,new int[]{ 1,2,3},false,0,5,1,false,true),
    //               new Stage(8,2,new int[]{ 2,3},false,0,1,2,true,false),
    //               new Stage(10,2,new int[]{ 1,3},false,0,2,2,false,false),
    //               new Stage(9,1,new int[]{ 4},false,0,3,2,false,false),
    //               new Stage(8,2,new int[]{ 1,4},false,0,4,2,false,false),
    //               new Stage(10,2,new int[]{ 2,4},false,0,5,2,false,true),
    //               new Stage(9,2,new int[]{ 3,4},false,0,1,3,false,false),
    //               new Stage(8,1,new int[]{ 5},false,0,2,3,false,false),
    //               new Stage(7,2,new int[]{ 4,5},false,0,3,3,false,false),
    //               new Stage(10,3,new int[]{ 1,3,5},false,0,4,3,false,false),
    //               new Stage(10,3,new int[]{ 1,4,5},false,0,5,3,false,true)
    //        };
    //    monsterPetDataList = new List<MonsterPetData>()
    //        {
    //            new MonsterPetData()
    //            {
    //                monsterID=1,
    //                monsterLevel=1,
    //                remainCookies=0,
    //                remainMilk=0
    //            },
    //            new MonsterPetData()
    //            {
    //                monsterID=2,
    //                monsterLevel=1,
    //                remainCookies=0,
    //                remainMilk=0
    //            },
    //            new MonsterPetData()
    //            {
    //                monsterID=3,
    //                monsterLevel=1,
    //                remainCookies=0,
    //                remainMilk=0
    //            },
    //        };
    //}

    public void LoadInitData()
    {
        //adventrueModelNum = 0;
        //burriedLevelNum = 0;
        //bossModelNum = 0;
        //coin = 0;
        //killMonsterNum = 0;
        //killBossNum = 0;
        //clearItemNum = 0;
        //cookies = 100;
        //milk = 100;
        //nest = 1;
        //diamands = 1000;
        //unLockedNormalModelLevelNum = new List<int>()
        //        {
        //            1,0,0
        //        };
        //unLockedNormalModelBigLevelList = new List<bool>()
        //        {
        //            true,false,false
        //        };
        //unLockedNormalModelLevelList = new List<Stage>()
        //        {
        //               new Stage(10,1,new int[]{ 1},false,0,1,1,true,false),
        //               new Stage(9,1,new int[]{ 2},false,0,2,1,false,false),
        //               new Stage(8,2,new int[]{ 1,2},false,0,3,1,false,false),
        //               new Stage(10,1,new int[]{ 3},false,0,4,1,false,false),
        //               new Stage(9,3,new int[]{ 1,2,3},false,0,5,1,false,true),
        //               new Stage(8,2,new int[]{ 2,3},false,0,1,2,true,false),
        //               new Stage(10,2,new int[]{ 1,3},false,0,2,2,false,false),
        //               new Stage(9,1,new int[]{ 4},false,0,3,2,false,false),
        //               new Stage(8,2,new int[]{ 1,4},false,0,4,2,false,false),
        //               new Stage(10,2,new int[]{ 2,4},false,0,5,2,false,true),
        //               new Stage(9,2,new int[]{ 3,4},false,0,1,3,false,false),
        //               new Stage(8,1,new int[]{ 5},false,0,2,3,false,false),
        //               new Stage(7,2,new int[]{ 4,5},false,0,3,3,false,false),
        //               new Stage(10,3,new int[]{ 1,3,5},false,0,4,3,false,false),
        //               new Stage(10,3,new int[]{ 1,4,5},false,0,5,3,false,true)
        //        };
        //monsterPetDataList = new List<MonsterPetData>()
        //        {
        //            new MonsterPetData()
        //            {
        //                monsterID=1,
        //                monsterLevel=1,
        //                remainCookies=0,
        //                remainMilk=0
        //            },
        //            new MonsterPetData()
        //            {
        //                monsterID=2,
        //                monsterLevel=1,
        //                remainCookies=0,
        //                remainMilk=0
        //            },
        //            new MonsterPetData()
        //            {
        //                monsterID=3,
        //                monsterLevel=1,
        //                remainCookies=0,
        //                remainMilk=0
        //            },
        //        };
    }

    public void SaveData()
    {
        Memento memento = new Memento();
        memento.SaveByJson();
    }

    public void ReadData()
    {
        Memento memento = new Memento();
        PlayerManager playerManager = memento.LoadByJson();
        //数据信息
        adventrueModelNum = playerManager.adventrueModelNum;
        burriedLevelNum = playerManager.burriedLevelNum;
        bossModelNum = playerManager.bossModelNum;
        coin = playerManager.coin;
        killMonsterNum = playerManager.killMonsterNum;
        killBossNum = playerManager.killBossNum;
        clearItemNum = playerManager.clearItemNum;
        cookies = playerManager.cookies;
        milk = playerManager.milk;
        nest = playerManager.nest;
        diamands = playerManager.diamands;
        //列表
        unLockedNormalModelBigLevelList = playerManager.unLockedNormalModelBigLevelList;
        unLockedNormalModelLevelList = playerManager.unLockedNormalModelLevelList;
        unLockedNormalModelLevelNum = playerManager.unLockedNormalModelLevelNum;
        monsterPetDataList = playerManager.monsterPetDataList;


    }
}
