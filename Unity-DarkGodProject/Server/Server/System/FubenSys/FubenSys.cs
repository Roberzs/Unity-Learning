/****************************************************
    文件：FubenSys.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/12/13 20:34:07
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using PEProtocol;

public class FubenSys
{
    private static FubenSys instance = null;
    public static FubenSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FubenSys();
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

        PECommon.Log("FubenSys Init Done.");

    }

    public void ReqFBFight(MsgPack pack)
    {
        ReqFBFight data = pack.msg.reqFBFight;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspFBFight,
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        int power = cfgSvc.GetMapData(data.fbId).power;
        
        if (pd.fuben < data.fbId)
        {
            msg.err = (int)ErrorCode.ClientDataError;
        }
        else if(pd.power < power)
        {
            msg.err = (int)ErrorCode.LackPower;
        }
        else
        {
            pd.power -= power;
            if (cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                RspFBFight rspFBFight = new RspFBFight
                {
                    fbId = data.fbId,
                    power = pd.power
                };
                msg.rspFBFight = rspFBFight;
            }
            else
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
        }
        pack.session.SendMsg(msg);
    }
}
