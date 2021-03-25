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

    // 用于测试
    public PlayerManager()
    {
        adventrueModelNum = 100;
        burriedLevelNum = 100;
        bossModelNum = 100;
        coin = 100;
        killMonsterNum = 100;
        clearItemNum = 100;
        unLockedNormalModelLevelNum = new List<int>()
        {
            2, 2, 2
        };
        unLockedNormalModelBigLevelList = new List<bool>()
        {
            true, true, true
        };

        unLockedNormalModelLevelList = new List<Stage>()
        {
            new Stage(10, 2, new int[]{1,2 },false, 1,1, 1,true,false),
            new Stage(10, 2, new int[]{2,3 },false, 0,2, 1,true,false),
            new Stage(10, 3, new int[]{5,2,6 },false, 0, 3, 1,true,false),
            new Stage(10, 2, new int[]{1,2 },false, 0,4, 1,false,false),
            new Stage(10, 2, new int[]{1,2 },false, 0,5, 1,false,true),
            new Stage(10, 2, new int[]{1,2 },false, 0,1, 2,true,false),
            new Stage(10, 2, new int[]{1,2 },false, 0,2, 2,true,false),
            new Stage(10, 2, new int[]{1,2 },false, 0,3, 2,true,false),
            new Stage(10, 2, new int[]{1,2 },false, 0,4, 2,true,false),
            new Stage(10, 2, new int[]{1,2 },false, 0,5, 2,false,true),
            new Stage(10, 2, new int[]{1,2 },false, 0,1, 3,true,false),
            new Stage(10, 2, new int[]{1,2 },false, 0,2, 3,true,false),
            new Stage(10, 2, new int[]{1,2 },false, 0,3, 3,true,false),
            new Stage(10, 2, new int[]{1,2 },false, 0,4, 3,true,false),
            new Stage(10, 2, new int[]{1,2 },false, 0,5, 3,false,true),
        };
    }
}
