/****************************************************
    文件：ClientSession.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/12/1 23:21:33
	功能：客户端网络会话
*****************************************************/

using UnityEngine;
using PEProtocol;

public class ClientSession : PENet.PESession<GameMsg>
{
    protected override void OnConnected()
    {
        GameRoot.AddTips("服务器连接成功");
        PECommon.Log("Server Connect succeed.");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("RcvPack CMD:"+ ((int)msg.cmd).ToString());
        NetSvc.Instance.AddNetPkg(msg);
    }

    protected override void OnDisConnected()
    {
        GameRoot.AddTips("服务器断开连接");
        PECommon.Log("DisConnect To Server");
    }
}
