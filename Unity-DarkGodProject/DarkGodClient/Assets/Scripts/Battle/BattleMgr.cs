/****************************************************
    文件：BattleMgr.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/12/14 22:30:49
    功能：战斗管理器
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr : MonoBehaviour
{
    private ResSvc resSvc;
    private AudioSvc audioSvc;

    private StateMgr stateMgr;
    private SkillMgr skillMgr;
    private MapMgr mapMgr;

    public void Init(int mapId)
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;

        // 初始化所有管理器
        stateMgr = gameObject.AddComponent<StateMgr>();
        stateMgr.Init();
        skillMgr = gameObject.AddComponent<SkillMgr>();
        skillMgr.Init();

        // 加载地图
        MapCfg mapData = resSvc.GetMapCfgData(mapId);
        resSvc.AsyncLoadScene(mapData.sceneName, () =>
        {
            // 初始化地图数据
            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            map.transform.localPosition = Vector3.zero;
            map.transform.localScale = Vector3.one;
            mapMgr = map.GetComponent<MapMgr>();
            mapMgr.Init();

            Camera.main.transform.position = mapData.mainCamPos;
            Camera.main.transform.localEulerAngles = mapData.mainCamRote;

            LoadPlayer(mapData);

            audioSvc.PlayBGMusic(Constants.BGHuangYe);
        });
    }

    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssissnBattlePlayerPrefab);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = Vector3.one;
    }


}
