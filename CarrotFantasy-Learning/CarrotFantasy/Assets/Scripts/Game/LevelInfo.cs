/****************************************************
    文件：LevelInfo.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/17 12:42:13
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo
{
    public int bigLevelID;
    public int levelID;

    public List<GridPoint.GridState> gridPoints;

    public List<GridPoint.GridIndex> monsterPath;

    public List<Round.RoundInfo> roundInfo;
}
