/****************************************************
    文件：NetSvc.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/12/1 23:15:46
	功能：网络服务
*****************************************************/

using PENet;
using PEProtocol;
using UnityEngine;
using System.Collections.Generic;

public class NetSvc : MonoBehaviour 
{
    public static NetSvc Instance = null;
    PESocket<ClientSession, GameMsg> client = null;

    private static readonly string obj = "lock";
    private Queue<GameMsg> msgQue = new Queue<GameMsg>();

    public void InitSvc()
    {
        Instance = this;

        client = new PESocket<ClientSession, GameMsg>();
        // 设置日志接口
        client.SetLog(true, (string msg, int lv) =>
        {
            switch (lv)
            {
                case 0:
                    msg = "Log" + msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "Warn" + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "Error" + msg;
                    Debug.LogError(msg);
                    break;
                case 3:
                    msg = "Info" + msg;
                    Debug.Log(msg);
                    break;
            }
        });
        client.StartAsClient(SrvCfg.srvIP, SrvCfg.srvPort);
        PECommon.Log("Init NetSvc");
    }

    public void SendMsg(GameMsg msg)
    {
        if (client.session != null)
        {
            client.session.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("服务器未连接");
            InitSvc();
        }
    }

    public void AddNetPkg(GameMsg msg)
    {
        lock (obj)
        {
             msgQue.Enqueue(msg);
        }
    }

    private void Update()
    {
        if (msgQue.Count > 0)
        {
            lock (obj)
            {
                GameMsg msg = msgQue.Dequeue();
                ProcessMsg(msg);
            }
        }
    }

    private void ProcessMsg(GameMsg msg)
    {
        // 如果返回错误码  弹出提示
        if(msg.err != (int)ErrorCode.None)
        {
            switch ((ErrorCode)msg.err){
                case ErrorCode.ServerDataError:
                    PECommon.Log("服务器数据异常", LogType.Error);
                    GameRoot.AddTips("客户端数据异常");
                    break;
                case ErrorCode.AcctIsOnLine:
                    GameRoot.AddTips("当前账号已在线");
                    break;
                case ErrorCode.WrongPass:
                    GameRoot.AddTips("密码错误");
                    break;

                case ErrorCode.NameIsExist:
                    GameRoot.AddTips("角色名重复");
                    break;
                case ErrorCode.UpdateDBError:
                    PECommon.Log("数据库更新异常", LogType.Error);
                    GameRoot.AddTips("网络不稳定");
                    break;
                case ErrorCode.LackLevel:
                    GameRoot.AddTips("等级不足");
                    break;
                case ErrorCode.LackCoin:
                    GameRoot.AddTips("金币不足");
                    break;
                case ErrorCode.LackCrystal:
                    GameRoot.AddTips("水晶不足");
                    break;
                case ErrorCode.LackDiamond:
                    GameRoot.AddTips("钻石不足");
                    break;
            }
            return;
        }

        switch ((CMD)msg.cmd)
        {
            case CMD.RspLogin:
                LoginSys.Instance.RspLogin(msg);
                break;
            case CMD.RspRename:
                LoginSys.Instance.RspRename(msg);
                break;
            case CMD.RspGuide:
                MainCitySys.Instance.RspGuide(msg);
                break;
            case CMD.RspStrong:
                MainCitySys.Instance.RspStrong(msg);
                break;
            case CMD.PshChat:
                MainCitySys.Instance.PshChat(msg);
                break;
            case CMD.RspBuy:
                MainCitySys.Instance.RspBuy(msg);
                break;
            case CMD.PshPower:
                MainCitySys.Instance.PshPower(msg);
                break;
        }
    }
}