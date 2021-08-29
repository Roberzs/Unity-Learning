/****************************************************
    文件：GameMsg.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/12/1 23:21:33
	功能：网络通信协议
*****************************************************/

using PENet;
using System;

namespace PEProtocol
{
    [Serializable]
    public class GameMsg: PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;

        public ReqRename reqRename;
        public RspRename rspRename;

        public ReqGuide reqGuide;
        public RspGuide rspGuide;

        public ReqStrong reqStrong;
        public RspStrong rspStrong;

        public SndChat sndChat;
        public PshChat pshChat;

        public ReqBuy reqBuy;
        public RspBuy rspBuy;

        public PshPower pshPower;

        public ReqTakeTaskReward reqTakeTaskReward;
        public RspTakeTaskReward rspTakeTaskReward;
    }

    #region 登录相关

    [Serializable]
    public class ReqLogin {
        public string acct;
        public string pass;
    }

    [Serializable]
    public class RspLogin
    {
        public PlayerData playerData;
    }

    [Serializable]
    public class ReqRename
    {
        public string name;
    }

    [Serializable]
    public class RspRename
    {
        public string name;
    }

    // 用户信息
    [Serializable]
    public class PlayerData
    {
        public int id;
        public string name;
        public int lv;
        public int exp;
        public int power;
        public int coin;
        public int diamond;
        public int crystal;

        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int dodge;//闪避概率
        public int pierce;//穿透比率
        public int critical;//暴击概率

        public int guideid;

        public int[] strongArr;

        public long time;

        public string[] taskArr;
    }

    #endregion

    #region 任务引导相关
    [Serializable]
    public class ReqGuide
    {
        public int guideid;
    }

    [Serializable]
    public class RspGuide
    {
        public int guideid;
        public int coin;
        public int lv;
        public int exp;
    }

    #endregion

    #region 强化相关
    [Serializable]
    public class ReqStrong
    {
        public int pos;
    }

    [Serializable]
    public class RspStrong
    {
        public int coin;
        public int crystal;
        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int[] strongArr;
    }
    #endregion

    #region 聊天相关
    [Serializable]
    public class SndChat
    {
        public string chat;
    }

    [Serializable]
    public class PshChat
    {
        public string name;
        public string chat;
    }
    #endregion

    #region 资源交易相关
    [Serializable]
    public class ReqBuy
    {
        public int type;
        public int cost;
    }

    [Serializable]
    public class RspBuy
    {
        public int type;
        public int diamond;
        public int coin;
        public int power;
    }
    #endregion

    #region 体力值恢复相关
    [Serializable]
    public class PshPower
    {
        public int power;
    }

    #endregion

    #region 任务相关
    [Serializable]
    public class ReqTakeTaskReward
    {
        public int rid;
    }

    [Serializable]
    public class RspTakeTaskReward
    {
        public int coin;
        public int lv;
        public int exp;
        public string[] taskArr;
    }

    #endregion

    // 错误码
    public enum ErrorCode
    {
        None = 0,

        ServerDataError,// 数据异常
        ClientDataError,// 客户端数据异常

        AcctIsOnLine,   // 账号在线
        WrongPass,      // 密码错误

        NameIsExist,    // 名字已存在
        UpdateDBError,  // 更新数据库失败

        LackLevel,
        LackCoin,
        LackCrystal,
        LackDiamond,
    }

    public enum CMD
    {
        None = 0,
        // 登录相关
        ReqLogin = 101,
        RspLogin = 102,

        ReqRename = 103,
        RspRename = 104,

        // 主城相关
        ReqGuide = 201,
        RspGuide = 202,

        ReqStrong = 203,
        RspStrong = 204,

        SndChat = 205,
        PshChat = 206,

        ReqBuy = 207,
        RspBuy = 208,

        PshPower = 209,

        ReqTakeTaskReward = 210,
        RspTakeTaskReward = 211,
    }

    public class SrvCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}
