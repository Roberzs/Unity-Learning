using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ServerRoot
{
    private static ServerRoot instance = null;
    public static ServerRoot Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ServerRoot();
            }
            return instance;

        }
    }

    public void Init()
    {
        // 数据层
        DBMgr.Instance.Init();

        // 服务层
        CacheSvc.Instance.Init();
        CfgSvc.Instance.Init();
        TimerSvc.Instance.Init();
        NetSvc.Instance.Init();

        // 业务系统层
        LoginSys.Instance.Init();
        GuideSys.Instance.Init();
        StrongSys.Instance.Init();
        ChatSys.Instance.Init();
        BuySys.Instance.Init();
        PowerSys.Instance.Init();
        TaskSys.Instance.Init();
    }

    public void Update()
    {
        NetSvc.Instance.Update();
        TimerSvc.Instance.Update();
    }

    private int sessionID = 0;
    public int GetSessionID()
    {
        if (sessionID == int.MaxValue)
            sessionID = 0;
        return sessionID += 1;
    }
}
