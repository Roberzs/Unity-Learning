/****************************************************
    文件：BaseData.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/4/5 21:06:17
    功能：配置数据类
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class StrongCfg : BaseData<StrongCfg>
{
    public int pos;
    public int starlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;
}

public class AutoGuideCfg: BaseData<AutoGuideCfg>
{
    public int npcID;       // 触发人物的NPCID
    public string dilogArr; // 对话
    public int actID;       // 任务ID
    public int coin;
    public int exp;
}

public class MapCfg: BaseData<MapCfg>
{
    public string mapName;
    public string sceneName;
    public Vector3 mainCamPos;
    public Vector3 mainCamRote;
    public Vector3 playerBornPos;
    public Vector3 playerBornRote;
}

public class TaskRewardCfg : BaseData<TaskRewardCfg>
{
    public string taskName;
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData : BaseData<TaskRewardData>
{
    public int prgs;
    public bool taked;
}

public class BaseData<T>
{
    public int ID;
}
