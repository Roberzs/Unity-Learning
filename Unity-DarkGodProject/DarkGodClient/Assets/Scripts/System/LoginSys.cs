/****************************************************
    文件：LoginSys.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/9/8 22:33:10
	功能：登录模块
*****************************************************/

using PEProtocol;
using UnityEngine;

public class LoginSys : SystemRoot 
{
    public static LoginSys Instance = null;

    public LoginWnd loginWnd;
    public CreateWnd createWnd;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init LoginSys");
    }

    public void EnterLogin()
    {

        // 异步加载登陆场景
        resSvc.AsyncLoadScene(Constants.SceneLogin, () => 
            {
                // 加载完成打开登录界面
                loginWnd.SetWndState(true);
                audioSvc.PlayBGMusic(Constants.BGLogin);
            }
        );
    }

    public void RspLogin(GameMsg msg)
    {
        GameRoot.AddTips("登录成功");

        GameRoot.Instance.SetPlayerData(msg.rspLogin);
        if (msg.rspLogin.playerData.name == "")
        {
            // 打开角色创建界面
            createWnd.SetWndState();
        }
        else
        {
            MainCitySys.Instance.EnterMainCity();
        }
        
        // 关闭登录界面
        loginWnd.SetWndState(false);
    }

    public void RspRename(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerName(msg.rspRename);

        // 跳转到主城
        MainCitySys.Instance.EnterMainCity();
        // 关闭创建角色界面
        createWnd.SetWndState(false);
    }
}