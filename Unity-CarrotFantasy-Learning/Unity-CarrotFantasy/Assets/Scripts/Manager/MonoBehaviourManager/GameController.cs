/****************************************************
    文件：GameController.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：游戏控制管理 负责控制整个游戏逻辑
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
    private static GameController _instance;
    public static GameController Instance { get => _instance; }

    private GameManager mGameManager;
    public Level level;
    public int[] mMonsterIDList;        // 当前波次的产怪列表
    public int mMonsterIDIndex;         // 当前产怪列表怪物的索引

    public Stage currentStage;
    public MapMaker mapMaker;
    public Transform targetTrans;       // 集火目标
    public GameObject targetSignal;     // 集火标志
    public GridPoint selectGrid;        // 当前选择的网格

    // 计数器
    public int killMonsterNum;          // 当前波次杀死敌人计数
    public int clearItemNum;            // 道具销毁数量计数
    public int killMonsterTotalNum;     // 杀怪总数

    // 属性
    public int carrotHp = 10;
    public int coin;
    public int gameSpeed;               // 游戏速度
    public bool isPause;
    
    public bool creatingMonster;        // 是否继续产生怪物
    public bool gameOver;               // 游戏是否结束
    

    // 建造者
    public MonsterBuilder monsterBuilder;
    public TowerBuilder towerBuilder;

    // 建塔价格表
    public Dictionary<int, int> towerPriceDict;
    // 建塔按钮列表
    public GameObject towerListGo;
    // 处理塔升级与买卖的画布
    public GameObject handleTowerCanvasGo;

    // 游戏中使用的UI面板
    public NormalModelPanel normalModelPanel;

    // 游戏资源
    public RuntimeAnimatorController[] controllers;

    private void Awake()
    {
#if GameRuning
        _instance = this;
        mGameManager = GameManager.Instance;
        // 注释 用以方便测试
        currentStage = mGameManager.currentStage;
        normalModelPanel = mGameManager.uIManager.mUIFacade.currentScenePanelDict[StringManager.NormalModelPanel] as NormalModelPanel;
        normalModelPanel.EnterPanel();
        //currentStage = new Stage(10, 5, new int[5] { 1, 2 ,3, 4, 5}, false,0 , 1, 1, true, false);
        

        mapMaker = GetComponent<MapMaker>();
        mapMaker.InitMapMaker();
        mapMaker.LoadMap(currentStage.mBigLevelID, currentStage.mLevelID);
        gameSpeed = 1;
        coin = 1000;
        monsterBuilder = new MonsterBuilder();
        towerBuilder = new TowerBuilder();

        // 建塔列表
        for (int i = 0; i < currentStage.mTowerIDList.Length; i++)
        {
            GameObject itemGo = mGameManager.GetGameObjectResource(FactoryType.UIFactory, "Btn_TowerBuild");
            itemGo.GetComponent<ButtonTower>().towerID = currentStage.mTowerIDList[i];
            itemGo.transform.SetParent(towerListGo.transform);
            itemGo.transform.localPosition = Vector3.zero;
            itemGo.transform.localScale = Vector3.one;
        }

        // 价格表
        towerPriceDict = new Dictionary<int, int>()
        {
            { 1,100},
            { 2,120},
            { 3,140},
            { 4,100},
            { 5,160},
        };

        controllers = new RuntimeAnimatorController[12];
        for(int i = 0; i < controllers.Length; i++)
        {
            controllers[i] = GetRuntimeAnimatorController("Monster/" + mapMaker.bigLevelID.ToString() + "/" + (i + 1).ToString());
        }

        level = new Level(mapMaker.roundInfoList.Count, mapMaker.roundInfoList);
        normalModelPanel.topPage.UpdateCoinText();
        normalModelPanel.topPage.UpdateRoundText();
        isPause = true;
        // level.HandleRound();
#endif
    }

    private void Update()
    {
#if GameRuning
        if (!isPause)
        {
            // 产怪逻辑
            if (killMonsterNum >= mMonsterIDList.Length)
            {
                if (level.currentRound >= level.totalRound)
                {
                    return;
                }
                // 回合数增加
                AddRoundNum();
            }
            else
            {
                if (!creatingMonster)
                {
                    CreateMonster();
                }
            }
        }
        else
        {
            // 暂停
            StopCreateMonster();
            creatingMonster = false;
        }
#endif
    }

    /// <summary>
    /// 玩家信息相关
    /// </summary>

    // 改变玩家金币数
    public void ChangeCoin(int coinNum)
    {
        coin += coinNum;
        // 更新UI
        normalModelPanel.UpdatePanel();
    }

    // 萝卜减血
    public void DecreaseHP()
    {
        PlayEffectMusic("NormalMordel/Carrot/Crash");

        carrotHp--;
        // 更新萝卜的UI
        mapMaker.carrot.UpdateCarrotUI();
    }

    // 判断当前道具是否全部清除
    public bool IfAllClear()
    {
        for (int x = 0; x < MapMaker.xColumn; x++)
        {
            for (int y = 0; y < MapMaker.yRow; y++)
            {
                if (mapMaker.gridPoints[x,y].gridState.hasItem == true)
                {
                    return false;
                }
            }
        }
        return true;
    }

    // 萝卜状态
    public int GetCarrotState()
    {
        int carrotState = 0;
        if (carrotHp >=8)
        {
            carrotState = 1;
        }
        else if (carrotHp >= 4)
        {
            carrotState = 2;
        }
        else
        {
            carrotState = 3;
        }
        return carrotState;
    }

    /// <summary>
    /// 产怪有关方法
    /// </summary>

    public void CreateMonster()
    {
        //Debug.Log("开始产怪");
        creatingMonster = true;
        // 重复延时调用
        InvokeRepeating("InstantiateMonster", (float)1 / gameSpeed, (float)1 / gameSpeed);
    }

    // 具体产怪逻辑
    private void InstantiateMonster()
    {
        if (mMonsterIDIndex >= mMonsterIDList.Length)
        {
            StopCreateMonster();
            return;
        }

        PlayEffectMusic("NormalMordel/Monster/Create");

        // 特效
        GameObject effectGo = GetGameObjectResource("CreateEffect");
        effectGo.transform.SetParent(transform);
        effectGo.transform.position = mapMaker.monsterPathPos[0];

        // 怪物
        if (mMonsterIDIndex < mMonsterIDList.Length)
        {
            // 当前关卡信息.产怪列表[当前波次].波次信息.怪物ID列表[第mMonsterIDIndex只怪物的ID]
            monsterBuilder.m_MonsterID = level.roudList[level.currentRound].roundInfo.mMonsterIDList[mMonsterIDIndex];
        }

        GameObject monsterGo = monsterBuilder.GetProduct();
        monsterGo.transform.SetParent(transform);
        monsterGo.transform.position = mapMaker.monsterPathPos[0];
        mMonsterIDIndex++;
        
    }

    public void StopCreateMonster()
    {
        CancelInvoke();
    }

    // 增加回合数 并让下一个回合的责任链处理产怪
    public void AddRoundNum()
    {
        mMonsterIDIndex = 0;
        killMonsterNum = 0;
        level.AddRoundNum();
        level.HandleRound();

        // 更新有关UI
        normalModelPanel.UpdatePanel();
    }

    /// <summary>
    /// 游戏逻辑有关方法
    /// </summary>
#if GameRuning
    public void HandleGrid(GridPoint grid)
    {
        if (grid.gridState.canBuild)
        {
            if (selectGrid == null)
            {
                // 没有上一个网格
                selectGrid = grid;
                selectGrid.ShowGrid();
                PlayEffectMusic("NormalMordel/Grid/GridSelect");
            }
            else if (grid == selectGrid)
            {
                // 选择网格与上一个相同
                grid.HideGrid();
                selectGrid = null;
                PlayEffectMusic("NormalMordel/Grid/GridDeSelect");
            }
            else if (grid != selectGrid)
            {
                // 选择网格与上一个不相同
                selectGrid.HideGrid();
                selectGrid = grid;
                selectGrid.ShowGrid();
                PlayEffectMusic("NormalMordel/Grid/GridSelect");
            }
        }
        else
        {
            grid.HideGrid();
            grid.ShowCantGrid();
            PlayEffectMusic("NormalMordel/Grid/SelectFault");
            if (selectGrid != null)
            {
                selectGrid.HideGrid();
            }
        }
    }

    // 开始游戏
    public void StartGame()
    {
        isPause = false;
        level.HandleRound();
    }

    // 打开礼物页面
    public void ShowPrizePage()
    {
        normalModelPanel.ShowPrizePage();
    }

    /// <summary>
    /// 与集火目标相关的方法
    /// </summary>

    public void ShowSignal()
    {
        PlayEffectMusic("NormalMordel/Tower/ShootSelect");
        targetSignal.transform.position = targetTrans.position + new Vector3(0, mapMaker.gridHeight / 2, 0);
        targetSignal.transform.SetParent(targetTrans);
        targetSignal.SetActive(true);
    }

    public void HideSignal()
    {
        targetSignal.gameObject.SetActive(false);
        targetTrans = null;
    }

#endif

    /// <summary>
    /// 各种资源的获取与操作
    /// </summary>
    public Sprite GetSprite(string resourcePath)
    {
        return mGameManager.GetSprite(resourcePath);
    }

    public AudioClip GetAudioClip(string resourcePath)
    {
        return mGameManager.GetAudioClip(resourcePath);
    }

    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return mGameManager.GetRuntimeAnimatorController(resourcePath);
    }

    public GameObject GetGameObjectResource(string resourcePaht)
    {
        return mGameManager.GetGameObjectResource(FactoryType.GameFactory, resourcePaht);
    }

    public void PushGameObjectToFactory(string resourcePath, GameObject itemGo)
    {
        mGameManager.PushGameObjectToFactory(FactoryType.GameFactory, resourcePath, itemGo);
    }

    public void PlayEffectMusic(string audioClipPath)
    {
        mGameManager.audioSourceManager.PlayEffectMusic(GetAudioClip(audioClipPath));
    }
}

public enum LevelType
{
    None,
    Boss
}