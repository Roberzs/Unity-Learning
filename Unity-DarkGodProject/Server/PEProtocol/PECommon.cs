/****************************************************
    文件：PECommon.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/12/1 23:21:33
	功能：服务器与客户端公用工具类
*****************************************************/

using PENet;
using PEProtocol;

public enum LogType
{
    Log = 0,
    Error = 1,
    Warning = 2,
    Info = 3
}

public class PECommon
{
    public static void Log (string msg = "", LogType tp = LogType.Log)
    {
        LogLevel lv = (LogLevel)tp;
        PETool.LogMsg(msg, lv);
    }

    // 返回角色战斗力
    public static int GetFightByProps(PlayerData pd)
    {
        return pd.lv * 100 + pd.ad + pd.ap + pd.addef + pd.apdef;
    }

    // 返回体力值上限
    public static int GetPowerLimit(int lv)
    {
        return ((lv - 1) / 10) * 150 + 150;
    }

    // 返回升级所需经验
    public static int GetExpUpValByLv(int lv)
    {
        return 100 * lv * lv;
    }

    // 人物升级的计算
    public static void CalcExp(PlayerData pd, int addExp)
    {
        int curtLv = pd.lv;
        int curtExp = pd.exp;
        int addRestExp = addExp;

        while (true)
        {
            int upNeedExp = PECommon.GetExpUpValByLv(curtLv) - curtExp;
            if (addRestExp >= upNeedExp)
            {
                curtLv += 1;
                curtExp = 0;
                addRestExp -= upNeedExp;
            }
            else
            {
                pd.lv = curtLv;
                pd.exp = curtExp + addRestExp;
                break;
            }
        }
    }

    public const int PowerAddSpace = 5;
    public const int PowerAddCount = 5;
}
