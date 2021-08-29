﻿/****************************************************
    文件：GuideSys.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/4/11 14:07:08
    功能：任务引导业务系统
*****************************************************/

using PEProtocol;
using System;
using System.Collections.Generic;

public class GuideSys
{
    private static GuideSys instance = null;
    public static GuideSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GuideSys();
            }
            return instance;

        }
    }
    private CacheSvc cacheSvc = null;
    private CfgSvc cfgSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgSvc.Instance;
        PECommon.Log("GuideSys Init Done.");

    }

    public void ReqGuide(MsgPack pack)
    {
        ReqGuide data = pack.msg.reqGuide;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspGuide,
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        GuideCfg gc = cfgSvc.GetAutoGuideData(data.guideid);

        // 更新任务ID 更新玩家数据
        if (pd.guideid == data.guideid)
        {
            if (pd.guideid == 1001)
            {
                // 如果任务为智者任务 更新智者点拨任务进度
                TaskSys.Instance.CalcTaskPrgs(pd, 1);
            }

            pd.guideid += 1;

            pd.coin += gc.coin;
           PECommon.CalcExp(pd, gc.exp);

            if (!cacheSvc.UpdatePlayerData(pd.id, pd)) {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspGuide = new RspGuide
                {
                    guideid = pd.guideid,
                    coin = pd.coin,
                    lv = pd.lv,
                    exp = pd.exp
                };
            }
        }
        else
        {
            msg.err = (int)ErrorCode.ServerDataError;
        }

        pack.session.SendMsg(msg);
    }

    
}
