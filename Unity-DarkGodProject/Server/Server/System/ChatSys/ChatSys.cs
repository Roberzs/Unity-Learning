/****************************************************
    文件：ChatSys.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/4/13 17:19:57
    功能：聊天系统
*****************************************************/

using PEProtocol;
using System.Collections.Generic;

public class ChatSys
{
    private static ChatSys instance = null;
    public static ChatSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ChatSys();
            }
            return instance;

        }
    }
    private CacheSvc cacheSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("ChatSys Init Done.");

    }

    public void SndChat(MsgPack pack)
    {
        SndChat data = pack.msg.sndChat;
        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

        // 更新任务进度
        TaskSys.Instance.CalcTaskPrgs(pd, 6);

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.PshChat,
            pshChat = new PshChat
            {
                name = pd.name,
                chat = data.chat
            }
        };

        // 广播所有在线客户端
        List<ServerSession> list = cacheSvc.GetOnlineServerSessions();
        byte[] bytes = PENet.PETool.PackNetMsg(msg);    // 提前序列化
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SendMsg(bytes);
        }
    }
}
