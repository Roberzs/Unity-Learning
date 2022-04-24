/****************************************************
    文件：GameRoot.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/9/8 22:32:53
	功能：Nothing
*****************************************************/

using PEProtocol;
using UnityEngine;

public class GameRoot : MonoBehaviour 
{
    public static GameRoot Instance = null;

    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        ClearUIRoot();
        Init();
    }

    // -- 设置UI启用状态 --//
    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++) canvas.GetChild(i).gameObject.SetActive(false);
        dynamicWnd.SetWndState();
    }

    private void Init()
    {
        // 服务模块初始化
        NetSvc net = GetComponent<NetSvc>();
        net.InitSvc();
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();
        AudioSvc audio = GetComponent<AudioSvc>();
        audio.InitSvc();
        TimerSvc timer = GetComponent<TimerSvc>();
        timer.InitSvc();

        // 业务模块初始化
        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();

        MainCitySys maincitySys = GetComponent<MainCitySys>();
        maincitySys.InitSys();

        FubenSys fubenSys = GetComponent<FubenSys>();
        fubenSys.InitSys();

        BattleSys battleSys = GetComponent<BattleSys>();
        battleSys.InitSys();

        // 物理设置 (每当Transform组件更改时是否自动将变换更改与物理系统同步, 会有一定的性能消耗, 默认关闭)
        Physics.autoSyncTransforms = true;

        // 进入加载登录场景UI
        login.EnterLogin();
    }

    public static void AddTips(string tips)
    {
        Instance.dynamicWnd.AddTips(tips);
    }

    private PlayerData playerData = null;
    public PlayerData PlayerData
    {
        get
        {
            return playerData;
        }
    }
    public void SetPlayerData(RspLogin data)
    {
        playerData = data.playerData;
    }

    public void SetPlayerName (RspRename data)
    {
        playerData.name = data.name;
    }

    public void SetPlayerDataByGuide(RspGuide data)
    {
        playerData.coin = data.coin;
        playerData.lv = data.lv;
        playerData.exp = data.exp;
        playerData.guideid = data.guideid;
    }

    public void SetPlayerDataByStrong(RspStrong data)
    {
        playerData.coin = data.coin;
        playerData.crystal = data.crystal;
        playerData.hp = data.hp;
        playerData.ad = data.ad;
        playerData.ap = data.ap;
        playerData.addef = data.addef;
        playerData.apdef = data.apdef;
        playerData.strongArr = data.strongArr;
    }

    public void SetPlayerDataByBuy(RspBuy data)
    {
        playerData.coin = data.coin;
        playerData.diamond = data.diamond;
        playerData.power = data.power;
    }

    public void SetPlayerDataByPower(PshPower data)
    {
        playerData.power = data.power;
    }

    public void SetPlayerDataByTask(RspTakeTaskReward data)
    {
        playerData.coin = data.coin;
        playerData.lv = data.lv;
        playerData.exp = data.exp;
        playerData.taskArr = data.taskArr;
    }

    public void SetPlayerDataByTask(PshTaskPrgs data)
    {
        playerData.taskArr = data.taskArr;
    }

    #region 副本系统业务逻辑
    public void SetPlayerDataByFBStart(RspFBFight data)
    {
        playerData.power = data.power;
    }
    #endregion
}