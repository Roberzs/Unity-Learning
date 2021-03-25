/****************************************************
    文件：GridPoint.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：网格脚本
*****************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class GridPoint : MonoBehaviour
{
    // 属性
    private SpriteRenderer spriteRenderer;
    public GridState gridState;
    public GridIndex gridIndex;
    public bool hasTower;

    // 资源
    private Sprite gridSprite;              // 网格图片
    private Sprite startSprite;             // 网格最开始显示的图片
    private Sprite cantBuildSprite;         // 禁止建塔图片

    private GameController gameController;
    private GameObject towerListGo;         // 建塔列表物体
    public GameObject handleTowerCanvasGo; // 有塔时需要使用的画布

    private Transform upLevelButtonTrans;   // 游戏场景升级按钮
    private Transform sellTowerButtonTrans; // 游戏场景出售按钮
    private Vector3 upLevelButtonInitPos;
    private Vector3 sellTowerButtonInitPos;

    // 有塔之后的属性
    public GameObject towerGo;
    public Tower tower;
    public TowerPersonalProperty towerPersonalProperty;
    public int towerLevel;
    private GameObject levelUpSignalGo;     // 升级信号标志


#if MapEditing
    private Sprite monsterPathSprite;       // 怪物路点图片
    public GameObject[] itemPrefabs;        // 道具数组
    public GameObject currentItem;          // 网格当前持有道具
#endif

    // 网格状态
    public struct GridState
    {
        public bool canBuild;
        public bool isMonsterPoint;
        public bool hasItem;
        public int itemID;
    }

    // 网格索引
    public struct GridIndex
    {
        public int xIndex;
        public int yIndex;
    }

    

    private void Awake()
    {
#if MapEditing
        gridSprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/Grid");
        monsterPathSprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/1/Monster/1-1");
        itemPrefabs = new GameObject[10];
        string prefabsPath = "Prefabs/Game/" +  MapMaker.Instrance.bigLevelID.ToString() + "/Item/";
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            itemPrefabs[i] = Resources.Load<GameObject>(prefabsPath + i.ToString());
            if (itemPrefabs[i] == null)
            {
                Debug.LogError("加载失败 路径" + prefabsPath + i.ToString());
            }
        }
#endif
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitGrid();

#if GameRuning
        gameController = GameController.Instance;
        gridSprite = gameController.GetSprite("NormalMordel/Game/Grid");
        startSprite = gameController.GetSprite("NormalMordel/Game/StartSprite");
        cantBuildSprite = gameController.GetSprite("NormalMordel/Game/cantBuild");
        spriteRenderer.sprite = startSprite;
        Tween tween = DOTween.To(
                    () =>
                    spriteRenderer.color,                // 我们想要改变的对象值
                    toColor                         // 经过DoTween计算得到的结果（当前值到目标值的插值）
                    => spriteRenderer.color = toColor,   // 将计算的结果赋值给想要改变的对象值
                    new Color(1, 1, 1, 0.2f),             // 想要得到的对象值
                    3.0f                              // 完成动画所需的时间
                    );
        tween.OnComplete(ChangeSpriteToGrid);
        towerListGo = gameController.towerListGo;
        handleTowerCanvasGo = gameController.handleTowerCanvasGo;
        upLevelButtonTrans = handleTowerCanvasGo.transform.Find("Btn_UpLevel");
        sellTowerButtonTrans = handleTowerCanvasGo.transform.Find("Btn_SellTower");
        upLevelButtonInitPos = upLevelButtonTrans.localPosition;
        sellTowerButtonInitPos = sellTowerButtonTrans.localPosition;
        levelUpSignalGo = transform.Find("UpLevelSignal").gameObject;
        levelUpSignalGo.SetActive(false);
#endif

    }

    private void Update()
    {
        if (levelUpSignalGo != null)
        {
            if (hasTower)
            {
                if (towerPersonalProperty.upLevelPrice <= gameController.coin && towerLevel < 3)
                {
                    levelUpSignalGo.SetActive(true);
                }
                else
                {
                    levelUpSignalGo.SetActive(false);
                }
            }
            else
            {
                if (levelUpSignalGo.activeSelf)
                {
                    levelUpSignalGo.SetActive(false);
                }
            }
        }
    }

    // 改回原来的Sprite
    private void ChangeSpriteToGrid()
    {
        spriteRenderer.enabled = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        if (gridState.canBuild)
        {
            spriteRenderer.sprite = gridSprite;
        }
        else
        {
            spriteRenderer.sprite = cantBuildSprite;
        }
    }

    public void InitGrid()
    {
        gridState.canBuild = true;
        gridState.isMonsterPoint = false;
        gridState.hasItem = false;
        spriteRenderer.enabled = true;
        gridState.itemID = -1;
#if MapEditing
        spriteRenderer.sprite = gridSprite;
        Destroy(currentItem);
#endif
#if GameRuning
        towerGo = null;
        towerPersonalProperty = null;
        hasTower = false;
#endif
    }


#if MapEditing

    private void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.P))
        {
            // 怪物路点
            gridState.canBuild = false;
            spriteRenderer.enabled = true;
            gridState.isMonsterPoint = !gridState.isMonsterPoint;
            if (gridState.isMonsterPoint)
            {
                // 是怪物路点
                MapMaker.Instrance.monsterPath.Add(gridIndex);
                spriteRenderer.sprite = monsterPathSprite;
            }
            else
            {
                MapMaker.Instrance.monsterPath.Remove(gridIndex);
                spriteRenderer.sprite = gridSprite;
                gridState.canBuild = true;
            }
        }
        else if (Input.GetKey(KeyCode.I))
        {
            // 道具
            gridState.itemID++;
            if (gridState.itemID == itemPrefabs.Length)
            {
                // 网格转换为没有道具
                gridState.itemID = -1;
                Destroy(currentItem);
                gridState.hasItem = false;
                return;
            }
            if (currentItem == null)
            {
                CreateItem();
            }
            else
            {
                Destroy(currentItem);
                CreateItem();
            }
            gridState.hasItem = true;
        }
        else if (!gridState.isMonsterPoint)
        {
            gridState.isMonsterPoint = false;
            gridState.canBuild = !gridState.canBuild;
            if (gridState.canBuild)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
    }



    // 生成道具
    private void CreateItem()
    {
        Vector2 createPos = transform.position;
        if (gridState.itemID <= 2)
        {
            createPos += new Vector2(MapMaker.Instrance.gridWidth, -MapMaker.Instrance.gridHeight) / 2;
        }
        else if(gridState.itemID <= 4 ) 
        {
            createPos += new Vector2(MapMaker.Instrance.gridWidth, 0) / 2;
        }
        GameObject itemGo = Instantiate(itemPrefabs[gridState.itemID], createPos, Quaternion.identity);
        itemGo.transform.SetParent(transform);
        currentItem = itemGo;
    }

    // 更新网格状态
    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            spriteRenderer.sprite = gridSprite;
            spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                CreateItem();
            }
        }
        else
        {
            if (gridState.isMonsterPoint)
            {
                spriteRenderer.sprite = monsterPathSprite;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
            
        }
    }

#endif

#if GameRuning
    // 更新网格状态
    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                CreateItem();
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    // 生成道具
    private void CreateItem()
    {
        GameObject itemGo = GameController.Instance.GetGameObjectResource(GameController.Instance.currentStage.mBigLevelID.ToString() + "/Item/" + gridState.itemID.ToString());
        itemGo.transform.SetParent(GameController.Instance.transform);

        // 对创建位置进行修正 要让道具在网格的上层
        Vector3 createPos = transform.position - new Vector3(0, 0, 3);
        if (gridState.itemID <= 2)
        {
            createPos += new Vector3(GameController.Instance.mapMaker.gridWidth , -GameController.Instance.mapMaker.gridHeight) / 2;
        }
        else if(gridState.itemID <= 4)
        {
            createPos += new Vector3(GameController.Instance.mapMaker.gridWidth, 0) / 2;
        }
        itemGo.transform.position = createPos;
        itemGo.GetComponent<Item>().gridPoint = this;
        itemGo.GetComponent<Item>().itemID = gridState.itemID;
    }

    // 有关网格处理的方法

    private void OnMouseDown()
    {
        // 如选择的是UI 不发生交互
        if (EventSystem.current.IsPointerOverGameObject()) return;

        gameController.HandleGrid(this);
    }

    public void ShowGrid()
    {
        
        if (!hasTower)
        {
            spriteRenderer.enabled = true;
            // 显示建塔列表
            towerListGo.transform.position = CorrectTowerListGoPosition();
            towerListGo.SetActive(true);
        }
        else
        {
            handleTowerCanvasGo.transform.position = transform.position;
            CorrectHandleTowerCanvasGoPosition();
            handleTowerCanvasGo.SetActive(true);
            // 显示塔的攻击范围
            towerGo.transform.Find("attackRange").gameObject.SetActive(true);
        }
    }

    public void HideGrid()
    {
        if (!hasTower)
        {
            // 隐藏建塔列表
            towerListGo.SetActive(false);
        }
        else
        {
            handleTowerCanvasGo.SetActive(false);
            // 隐藏塔的攻击范围
            towerGo.transform.Find("attackRange").gameObject.SetActive(false);

        }
        spriteRenderer.enabled = false;
    }

    public void ShowCantGrid()
    {
        spriteRenderer.enabled = true;
        Tween tween = DOTween.To(
                    () =>
                    spriteRenderer.color,                // 我们想要改变的对象值
                    toColor                         // 经过DoTween计算得到的结果（当前值到目标值的插值）
                    => spriteRenderer.color = toColor,   // 将计算的结果赋值给想要改变的对象值
                    new Color(1, 1, 1, 0f),             // 想要得到的对象值
                    2.0f                              // 完成动画所需的时间
                    );
        tween.OnComplete(
            () =>
            {
                spriteRenderer.enabled = false;
                spriteRenderer.color = new Color(1, 1, 1, 1);
            }
            );
    }

    // 纠正建塔列表的位置 将其显示有所偏移 使之能够显示完整
    private Vector3 CorrectTowerListGoPosition()
    {
        Vector3 correctPosition = Vector3.zero;
        if (gridIndex.xIndex <= 3 && gridIndex.xIndex >= 0)
        {
            correctPosition += new Vector3(gameController.mapMaker.gridWidth, 0, 0);
        }
        else if (gridIndex.xIndex <= 11 && gridIndex.xIndex >= 8)
        {
            correctPosition -= new Vector3(gameController.mapMaker.gridWidth, 0, 0);
        }
        
        if (gridIndex.yIndex <= 3 && gridIndex.yIndex >= 0)
        {
            correctPosition += new Vector3(0, gameController.mapMaker.gridHeight, 0);
        }
        else if (gridIndex.yIndex <= 7 && gridIndex.yIndex >= 4)
        {
            correctPosition -= new Vector3(0, gameController.mapMaker.gridHeight, 0);
        }
        correctPosition += transform.position;
        return correctPosition;
    }

    // 纠正升级出售画布的位置
    private void CorrectHandleTowerCanvasGoPosition()
    {
        upLevelButtonTrans.localPosition = Vector3.zero;
        sellTowerButtonTrans.localPosition = Vector3.zero;
        if (gridIndex.yIndex <= 0)
        {
            if (gridIndex.xIndex == 0)
            {
                sellTowerButtonTrans.position += new Vector3(gameController.mapMaker.gridWidth / 10 * 9, 0, 0);
            }
            else
            {
                sellTowerButtonTrans.position -= new Vector3(gameController.mapMaker.gridWidth / 10 * 9, 0, 0);
            }
            upLevelButtonTrans.localPosition = upLevelButtonInitPos;
        }
        else if (gridIndex.yIndex >= 6)
        {
            if (gridIndex.xIndex == 0)
            {
                upLevelButtonTrans.position += new Vector3(gameController.mapMaker.gridWidth / 10 * 9, 0, 0);
            }
            else
            {
                upLevelButtonTrans.position -= new Vector3(gameController.mapMaker.gridWidth / 10 * 9, 0, 0);
            }
            sellTowerButtonTrans.localPosition = sellTowerButtonInitPos;
        }
        else
        {
            upLevelButtonTrans.localPosition = upLevelButtonInitPos;
            sellTowerButtonTrans.localPosition = sellTowerButtonInitPos;
        }
    }

    // 建塔后的处理
    public void AfterBuild()
    {
        spriteRenderer.enabled = false;
        towerGo = transform.GetChild(1).gameObject;
        tower = towerGo.GetComponent<Tower>();
        towerPersonalProperty = towerGo.GetComponent<TowerPersonalProperty>();
        towerLevel = towerPersonalProperty.towerLevel;
    }

#endif
}