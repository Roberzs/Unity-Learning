/****************************************************
    文件：CreateWnd.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/11/27 20:57:48
	功能：Nothing
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class CreateWnd : WindowRoot
{
    public InputField iptName;
    protected override void InitWnd()
    {
        base.InitWnd();

        iptName.text = resSvc.GetRdNameData();
    }

    public void ClickRandBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        iptName.text = resSvc.GetRdNameData(false);
    }

    public void ClickEnterBtn()
    {
        if (iptName.text != "")
        {
            // 发送请求到服务器，跳转到主城
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqRename,
                reqRename = new ReqRename
                {
                    name = iptName.text,
                },
            };
            netSvc.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("角色名不能为空");
        }
    }
}