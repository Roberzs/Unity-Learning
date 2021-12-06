/****************************************************
    文件：StrongSys.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/4/13 15:01:35
    功能：强化系统
*****************************************************/

using PEProtocol;
using System;
using System.Collections.Generic;

public class StrongSys
{
    public static StrongSys instance = null;
    public static StrongSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StrongSys();
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
        PECommon.Log("StrongSys Init Done.");

    }

    public void ReqStrong(MsgPack pack)
    {
        ReqStrong data = pack.msg.reqStrong;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspStrong,
        };
        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        int curtStarLv = pd.strongArr[data.pos];
        StrongCfg nextsd = CfgSvc.Instance.GetStrongData(data.pos, curtStarLv + 1);
        // 数据校验
        if (pd.lv < nextsd.minlv)
        {
            msg.err = (int)ErrorCode.LackLevel;
        }
        else if (pd.coin < nextsd.coin)
        {
            msg.err = (int)ErrorCode.LackCoin;
        }
        else if (pd.crystal < nextsd.crystal)
        {
            msg.err = (int)ErrorCode.LackCrystal;
        }
        else
        {
            // 扣除资源
            pd.coin -= nextsd.coin;
            pd.crystal -= nextsd.crystal;

            pd.strongArr[data.pos] += 1;

            // 属性强化
            pd.hp += nextsd.addhp;
            pd.ad += nextsd.addhurt;
            pd.ap += nextsd.addhurt;
            pd.addef += nextsd.adddef;
            pd.apdef += nextsd.adddef;

            // 更新任务进度
            TaskSys.Instance.CalcTaskPrgs(pd, 3);
        }

        // 更新数据库
        if (!cacheSvc.UpdatePlayerData(pd.id, pd))
        {
            msg.err = (int)ErrorCode.UpdateDBError;
        }
        else
        {
            msg.rspStrong = new RspStrong
            {
                coin = pd.coin,
                crystal = pd.crystal,
                hp = pd.hp,
                ad = pd.ad,
                ap = pd.ap,
                addef = pd.addef,
                apdef = pd.apdef,
                strongArr = pd.strongArr
            };
        }

        pack.session.SendMsg(msg);
    }
}
