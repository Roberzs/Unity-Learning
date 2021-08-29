/****************************************************
    文件：ChatWnd.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/4/13 16:37:05
    功能：Nothing
*****************************************************/

using PEProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWnd : WindowRoot
{
    public Text txtChat;
    public InputField iptChat;

    public Image imgWorld;
    public Image imgGuild;
    public Image imgFriend;

    private int chatType;

    private List<string> chatList = new List<string>();

    private bool canSend = true;

    protected override void InitWnd()
    {
        base.InitWnd();
        chatType = 0;
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (chatType == 0)
        {
            string chatMsg = "";
            for (int i = 0; i < chatList.Count; i++)
            {
                chatMsg += chatList[i] + "\n";
            }
            SetText(txtChat, chatMsg);

            SetSprite(imgWorld, "ResImages/btntype1");
            SetSprite(imgGuild, "ResImages/btntype2");
            SetSprite(imgFriend, "ResImages/btntype2");
        }
        else if (chatType == 1)
        {
            SetText(txtChat, "系统: 尚未加入工会");

            SetSprite(imgWorld, "ResImages/btntype2");
            SetSprite(imgGuild, "ResImages/btntype1");
            SetSprite(imgFriend, "ResImages/btntype2");
        }
        else if (chatType == 2)
        {
            SetText(txtChat, "系统: 暂无好友");

            SetSprite(imgWorld, "ResImages/btntype2");
            SetSprite(imgGuild, "ResImages/btntype2");
            SetSprite(imgFriend, "ResImages/btntype1");
        }
    }

    public void ClickWorldBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;

        RefreshUI();
    }

    public void ClickGuildBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 1;

        RefreshUI();
    }

    public void ClickFriendBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 2;

        RefreshUI();
    }

    public void ClickSendBtn()
    {
        if (!canSend)
        {
            GameRoot.AddTips("发送信息频率过高！");
            return;
        }
        if (iptChat.text != null && iptChat.text != "")
        {
            if (iptChat.text.Length > 12)
            {
                GameRoot.AddTips("发送信息过长！");
            }
            else
            {
                // 发送信息到服务器
                GameMsg msg = new GameMsg
                {
                    cmd = (int)CMD.SndChat,
                    sndChat = new SndChat
                    {
                        chat = iptChat.text,
                    }
                };
                iptChat.text = "";
                //SetText(iptChat, "");
                netSvc.SendMsg(msg);
            }
        }
        else
        {
            GameRoot.AddTips("发送信息不能为空！");
            return;
        }
        canSend = false;

        timerSvc.AddTimeTask((int tid) =>
        {
            canSend = true;
        }, 5, PETimeUnit.Second);
        
    }

    public void AddChatMsg(string name, string chat)
    {
        chatList.Add(Constants.Color(name + ":", TxtColor.Blue) + chat);
        if (chatList.Count > 12)
        {
            chatList.RemoveAt(0);
        }
        if (gameObject.activeSelf == false) return;
        RefreshUI();
    }

    public void ClickClostBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        SetWndState(false);
    }
}
