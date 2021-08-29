/****************************************************
    文件：LoginSys.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/12/1 23:21:33
	功能：登录业务系统
*****************************************************/

using PEProtocol;
using System;

public class LoginSys
{
    public static LoginSys instance = null;
    public static LoginSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LoginSys();
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
        PECommon.Log("LoginSys Init Done.");
        
    }

    public void ReqLogin(MsgPack pack)
    {
        /**
         *  1 缓存层查找账号 若已存在说明已上线 发送错误码
         *  2 若未上线 通过缓存层获取账号信息 若返回空 说明密码错误 发送错误码
         *  3 账号密码正确 将账号信息发送给客户端 并将账号以及账号信息添加到缓存层
         */
        ReqLogin data = pack.msg.reqLogin;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspLogin,
        };

        if (cacheSvc.IsAcctOnLine(data.acct))
        {
            // 账号已上线 返回错误信息
            msg.err = (int)ErrorCode.AcctIsOnLine;
        }
        else
        {
            // 账号未上线
            PlayerData pd = cacheSvc.GetPlayerData(data.acct, data.pass);
            if (pd == null)
            {
                // 密码错误
                msg.err = (int)ErrorCode.WrongPass;
                PECommon.Log("密码错误", LogType.Error);
            }
            else
            {
                // 离线体力恢复
                int power = pd.power;
                long now = timerSvc.GetNowTime();
                long milliseconds = now - pd.time;
                int addPower = (int)(milliseconds / (1000 * 60 * PECommon.PowerAddSpace)) * PECommon.PowerAddCount;
                if (addPower > 0)
                {
                    int powerMax = PECommon.GetPowerLimit(pd.lv);
                    pd.power = Math.Min(power + addPower, powerMax);
                }
                if (power != pd.power)
                {
                    cacheSvc.UpdatePlayerData(pd.id, pd);
                }

                msg.rspLogin = new RspLogin
                {
                    playerData = pd
                };
                // 缓存账号数据
                cacheSvc.AcctOnLine(data.acct, pack.session, pd);
            }
        }
        // 回应客户端
        pack.session.SendMsg(msg);
    }

    // 判断名字是否合理
    public void ReqRename(MsgPack pack)
    {
        ReqRename data = pack.msg.reqRename;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspRename,
        };
        if (!cacheSvc.IsNameExist(data.name))
        {
            // 不存在
            // 更新缓存、数据库 然后返回客户端
            PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.session);
            playerData.name = data.name;
            if(cacheSvc.UpdatePlayerData(playerData.id, playerData))
            {
                // 更新成功
                msg.rspRename = new RspRename
                {
                    name = playerData.name,
                };
            }
            else
            {
                // 更新失败 
                msg.err = (int)ErrorCode.UpdateDBError;
            }

        }
        else
        {
            // 存在 返回错误码
            msg.err = (int)ErrorCode.NameIsExist;
        }
        // 返回消息
        pack.session.SendMsg(msg);
    }

    // 下线处理
    public void ClearOfflineData(ServerSession session)
    {
        // 下线时间修改
        PlayerData pd = cacheSvc.GetPlayerDataBySession(session);
        if (pd != null)
        {
            pd.time = timerSvc.GetNowTime();
            if (!cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                PECommon.Log("数据更新失败",LogType.Error);
            }
        }
        cacheSvc.AcctOfLine(session);
    }
}
