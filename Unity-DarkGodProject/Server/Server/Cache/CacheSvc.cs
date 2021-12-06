/****************************************************
    文件：CacheSvc.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/12/1 23:21:33
	功能：数据缓存层
*****************************************************/

using System.Collections.Generic;
using PEProtocol;

public class CacheSvc
{
    public static CacheSvc instance = null;
    public static CacheSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CacheSvc();
            }
            return instance;

        }
    }

    private DBMgr dbMgr;

    private Dictionary<string, ServerSession> onLineAccDic = new Dictionary<string, ServerSession>();
    private Dictionary<ServerSession, PlayerData> onLineSessionDic = new Dictionary<ServerSession, PlayerData>();

    public void Init()
    {
        dbMgr = DBMgr.Instance;
        PECommon.Log("CacheSvc Init Done.");

    }

    public bool IsAcctOnLine(string acct)
    {
        return onLineAccDic.ContainsKey(acct);
    }

    // 获取所有在线客户端
    public List<ServerSession> GetOnlineServerSessions()
    {
        List<ServerSession> list = new List<ServerSession>();
        foreach (var item in onLineSessionDic)
        {
            list.Add(item.Key);
        }
        return list;
    }

    public Dictionary<ServerSession, PlayerData> GetOnlineCache()
    {
        return onLineSessionDic;
    }

    // 根据账号密码查找数据 密码错误返回null  账号不存在默认创建账号
    public PlayerData GetPlayerData(string acct, string pass)
    {
        // 在数据库中查找对应数据

        return dbMgr.QueryPlayerData(acct, pass);
    }

    // 账号上线 缓存数据
    public void AcctOnLine(string acct,ServerSession session,PlayerData playerData)
    {
        onLineAccDic.Add(acct, session);
        onLineSessionDic.Add(session, playerData);
    }

    public bool IsNameExist(string name)
    {
        return dbMgr.QueryNameData(name);
    }

    // 根据session查找用户playerdata
    public PlayerData GetPlayerDataBySession(ServerSession session)
    {
        if (onLineSessionDic.TryGetValue(session, out PlayerData playerData))
        {
            return playerData;
        }
        else
        {
            return null;
        }
    }

    // 更新数据
    public bool UpdatePlayerData(int id, PlayerData playerData)
    {
        return dbMgr.UpdatePlayerData(id, playerData);
    }

    public ServerSession GetOnlineServerSession(int ID)
    {
        ServerSession session = null;
        foreach (var item in onLineSessionDic)
        {
            if (item.Value.id == ID)
            {
                session = item.Key;
                break;
            }
        }
        return session;
    }

    // 下线处理
    public void AcctOfLine(ServerSession session)
    {
        foreach (var item in onLineAccDic)
        {
            if (item.Value == session)
            {
                onLineAccDic.Remove(item.Key);
                break;
            }
        }

        bool succ = onLineSessionDic.Remove(session);
        PECommon.Log("offline Result:" + succ + " SessionID:" + session.sessionID);
    }
}

