/****************************************************
    文件：MainCitySys.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/31 16:17:52
    功能：主城业务系统
*****************************************************/

using PEProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainCitySys : SystemRoot
{
    public static MainCitySys Instance = null;

    public MainCityWnd maincityWnd;
    public InfoWnd infoWnd;
    public GuideWnd guideWnd;
    public StrongWnd strongWnd;
    public ChatWnd chatWnd;
    public BuyWnd buyWnd;
    public TaskWnd taskWnd;

    private PlayerController playerCtrl;
    private Transform charCamTrans;

    private AutoGuideCfg curtTaskData;
    private Transform[] npcPosTrans;        // NPC位置
    private NavMeshAgent nav;               // 角色导航组件
    private CharacterController charCtrl;   // 角色CharacterController组件

    private bool isNavGuide = false;        // 角色是否处于导航状态

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init MainCitySys");
    }

    public void EnterMainCity()
    {
        MapCfg mapData = resSvc.GetMapCfgData(Constants.MainCityMapID);

        resSvc.AsyncLoadScene(mapData.sceneName, () =>
        {
            PECommon.Log("Enter MainCity...");

            // 加载游戏主角
            LoadPlayer(mapData);

            // 打开主场景UI
            maincityWnd.SetWndState();

            // 播放主城背景音乐
            audioSvc.PlayBGMusic(Constants.BGMainCity);

            // 获取NPC寻路位置
            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            MainCityMap mcm = map.GetComponent<MainCityMap>();
            npcPosTrans = mcm.NpcPosTrans;

            // 设置人物展示相机
            if(charCamTrans != null)
            {
                charCamTrans.gameObject.SetActive(false);
            }
        });
    }

    private void LoadPlayer(MapCfg mapData)
    {
        //Debug.Log(mapData.playerBornRote);
        GameObject player = resSvc.LoadPrefab(PathDefine.AssissnCityPlayerPrefab);
        // 人物加载后会下落 测试后是CharacterController的问题 具体问题不明 此处先将人物组件禁用 设置位置后再启用
        var ctrl = player.GetComponent<CharacterController>();
        ctrl.enabled = false;
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        ctrl.enabled = true;
        // 初始化相机
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        charCtrl = player.GetComponent<CharacterController>();
        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();

        nav = player.GetComponent<NavMeshAgent>();
    }

    public void SetMoveDir(Vector2 dir)
    {
        StopNavTask();
        if (dir == Vector2.zero)
        {
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
        else
        {
            playerCtrl.SetBlend(Constants.BlendWalk);
        }
        playerCtrl.Dir = dir;
    }

    public void OpenInfoWnd()
    {
        StopNavTask();
        if (charCamTrans == null)
        {
            charCamTrans = GameObject.FindGameObjectWithTag("CharShowCam").transform;
        }
        // 设置人物展示相机的相对位置
        charCamTrans.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 3.8f + new Vector3(0, 1.2f, 0);
        charCamTrans.localEulerAngles = new Vector3(0, 180 + playerCtrl.transform.localEulerAngles.y, 0);
        charCamTrans.localScale = Vector3.one;
        charCamTrans.gameObject.SetActive(true);
        infoWnd.SetWndState();
    }

    public void CloseInfoWnd()
    {
        if (charCamTrans != null)
        {
            charCamTrans.gameObject.SetActive(false);
        }
        infoWnd.SetWndState(false);
    }

    private float startRoate = 0;
    public void SetStartRoate()
    {
        startRoate = playerCtrl.transform.localEulerAngles.y;
    }

    public void SetPlayerRoate(float roate)
    {
        playerCtrl.transform.localEulerAngles = new Vector3(0, startRoate + roate, 0);
    }

    public void RunTask(AutoGuideCfg agc)
    {
        if (agc != null)
        {
            curtTaskData = agc;
        }

        // 解析任务数据 自动寻路
        nav.enabled = true;
        charCtrl.enabled = false;
        if (curtTaskData.npcID != -1)
        {
            float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[agc.npcID].position);
            if (dis < 0.5f)
            {
                isNavGuide = false;
                nav.isStopped = true;
                playerCtrl.SetBlend(Constants.BlendIdle);
                nav.enabled = false;
                charCtrl.enabled = true;
                OpenGuideWnd();
            }
            else
            {
                isNavGuide = true;
                //playerCtrl.enabled = false;
                nav.enabled = true;
                nav.speed = Constants.PlayerMoveSpeed;
                nav.SetDestination(npcPosTrans[agc.npcID].position);
                playerCtrl.SetBlend(Constants.BlendWalk);
            }
        }
        else
        {
            OpenGuideWnd();
        }
    }

    private void Update()
    {
        // 如果处于导航 对相机位置进行更新    
        if (isNavGuide)
        {
            IsArriveNavPos();
            playerCtrl.SetCam();
        }
    }

    private void StopNavTask()
    {
        if (isNavGuide)
        {
            isNavGuide = false;
            nav.isStopped = true;
            playerCtrl.SetBlend(Constants.BlendIdle);
            nav.enabled = false;
            charCtrl.enabled = true;
        }
    }

    private void IsArriveNavPos()
    {
        float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[curtTaskData.npcID].position);
        if (dis < 0.5f)
        {
            isNavGuide = false;
            nav.isStopped = true;
            playerCtrl.SetBlend(Constants.BlendIdle);
            nav.enabled = false;
            charCtrl.enabled = true;
            OpenGuideWnd();
        }
    }

    #region 任务引导

    // 打开对话界面
    private void OpenGuideWnd()
    {
        guideWnd.SetWndState();
    }

    public AutoGuideCfg GetCurtTaskData()
    {
        return curtTaskData;
    }

    public void RspGuide(GameMsg msg)
    {
        RspGuide data = msg.rspGuide;

        GameRoot.AddTips(Constants.Color( "任务奖励 金币:" + curtTaskData.coin + " 经验值:" + curtTaskData.exp, TxtColor.Blue));
        switch (curtTaskData.actID)
        {
            case 0:
                // 与智者的对话
                break;
            default:
                break;
        }

        GameRoot.Instance.SetPlayerDataByGuide(data);
        maincityWnd.RefreshUI();
    }

    #endregion

    #region 强化

    public void OpenStrongWnd()
    {
        strongWnd.SetWndState();
    }

    public void RspStrong(GameMsg msg)
    {
        int zhanliPre = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
        GameRoot.Instance.SetPlayerDataByStrong(msg.rspStrong);
        int zhanliNow = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
        GameRoot.AddTips(Constants.Color("战力提升 + " + (zhanliNow - zhanliPre), TxtColor.Green));

        strongWnd.UpdateUI();
        maincityWnd.RefreshUI();
    }

    #endregion

    #region 聊天
    public void OpenChatWnd()
    {
        chatWnd.SetWndState();
    }

    public void PshChat(GameMsg msg)
    {
        chatWnd.AddChatMsg(msg.pshChat.name, msg.pshChat.chat);
    }

    #endregion

    #region 商店

    public void OpenBuyWnd(int type)
    {
        buyWnd.SetBuyType(type);
        buyWnd.SetWndState();
    }

    public void RspBuy(GameMsg msg)
    {
        RspBuy data = msg.rspBuy;
        GameRoot.Instance.SetPlayerDataByBuy(data);
        GameRoot.AddTips("购买成功");

        maincityWnd.RefreshUI();
        buyWnd.SetWndState(false);
    }

    #endregion

    #region 能量恢复
    public void PshPower(GameMsg msg)
    {
        PshPower data = msg.pshPower;
        GameRoot.Instance.SetPlayerDataByPower(data);
        if (maincityWnd.gameObject.activeSelf)
        {
            maincityWnd.RefreshUI();
        }
        
    }
    #endregion

    #region 任务奖励
    public void OpenTaskRewardWnd()
    {
        taskWnd.SetWndState();
    }

    public void RspTakeTaskReward(GameMsg msg)
    {
        RspTakeTaskReward data = msg.rspTakeTaskReward;
        GameRoot.Instance.SetPlayerDataByTask(data);

        taskWnd.RefreshUI();
        maincityWnd.RefreshUI();
    }

    public void PshTaskPrgs(GameMsg msg)
    {
        PshTaskPrgs data = msg.pshTaskPrgs;
        GameRoot.Instance.SetPlayerDataByTask(data);


    }
    #endregion
}
