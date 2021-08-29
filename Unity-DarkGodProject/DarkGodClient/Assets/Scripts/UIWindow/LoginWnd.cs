/****************************************************
    文件：LoginWnd.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/9/9 21:31:40
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
using PEProtocol;

public class LoginWnd : WindowRoot 
{
    public InputField iptAcct;
    public InputField iptPass;
    public Button btnEnter;
    public Button btnNotice;

    protected override void InitWnd()
    {
        base.InitWnd();

        if (PlayerPrefs.HasKey("Account") && PlayerPrefs.HasKey("Password"))
        {
            iptAcct.text = PlayerPrefs.GetString("Account");
            iptPass.text = PlayerPrefs.GetString("Password");
        }
        else
        {
            iptAcct.text = "";
            iptPass.text = "";
        }
    }

    public void ClickEnterBtn()
    {
        audioSvc.PlayUIAudio(Constants.UILoginBtn);

        string _acct = iptAcct.text;
        string _pass = iptPass.text;
        if (_acct != "" && _pass != "") {
            // 更新本地存储的账号密码
            PlayerPrefs.SetString("Account", _acct);
            PlayerPrefs.SetString("Password", _pass);

            // 发送网络请求，请求登录
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqLogin,
                reqLogin = new ReqLogin
                {
                    acct = _acct,
                    pass = _pass
                }
            };
            netSvc.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("账号或密码为空");
        }

    }

    public void ClickNoticeBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        GameRoot.AddTips("模块尚未开发");
    }
}