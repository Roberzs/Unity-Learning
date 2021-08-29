/****************************************************
    文件：PowerSys.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/4/15 14:38:34
    功能：Nothing
*****************************************************/

using PEProtocol;
using System;
using System.Collections.Generic;

public class PowerSys
{
    private static PowerSys instance = null;
    public static PowerSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PowerSys();
            }
            return instance;

        }
    }
    private CacheSvc cacheSvc = null;
    private TimerSvc timerSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        timerSvc = TimerSvc.Instance;

        TimerSvc.Instance.AddTimeTask(CalcPowerAdd, PECommon.PowerAddSpace, PETimeUnit.Minute, 0);
        PECommon.Log("PowerSys Init Done.");

    }

    private void CalcPowerAdd(int tid)
    {
        // 体力恢复
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.PshPower
        };
        msg.pshPower = new PshPower();

        // 对在线玩家推送体力恢复数据
        Dictionary<ServerSession, PlayerData> onlineDic = cacheSvc.GetOnlineCache();
        foreach (var item in onlineDic)
        {
            PlayerData pd = item.Value;
            ServerSession session = item.Key;

            int powerMax = PECommon.GetPowerLimit(pd.lv);
            if (pd.power >= powerMax)
            {
                continue;
            }
            else
            {
                pd.power = Math.Min(pd.power + PECommon.PowerAddCount, powerMax);
                pd.time = timerSvc.GetNowTime();
                if (!cacheSvc.UpdatePlayerData(pd.id, pd))
                {
                    msg.err = (int)ErrorCode.UpdateDBError;
                }
                else
                {
                    msg.pshPower.power = pd.power;
                    session.SendMsg(msg);
                }
            }
        }

    }
}
