/****************************************************
    文件：ServerSession.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/12/1 23:21:33
	功能：服务器网络会话
*****************************************************/

using PENet;
using PEProtocol;

public class ServerSession : PESession<GameMsg>
{
    public int sessionID = 0;

    protected override void OnConnected()
    {
        sessionID = ServerRoot.Instance.GetSessionID();
        PECommon.Log("SessionID:" + sessionID + " Client Connect");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("SessionID:" + sessionID + " RvPack CMD:" + ((CMD)msg.cmd).ToString());
        NetSvc.Instance.AddMsgQue(this, msg);
    }

    protected override void OnDisConnected()
    {
        LoginSys.Instance.ClearOfflineData(this);
        PECommon.Log("SessionID:" + sessionID + " Client DisConnect");
    }
}
