/****************************************************
    文件：MapMaker.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class MapMaker : MonoBehaviour 
{
#if MapEditing
    private static MapMaker _instrance;
    public bool drawLine;       // 是否绘制线条
    public static MapMaker Instrance { get => _instrance;  }
#endif
    // 地图长宽
    private float mapWidth;
    private float mapHeight;
    // 网格长宽
    public float gridWidth;
    public float gridHeight;
    // 行列个数
    public const int yRow = 8;
    public const int xColumn = 12;
    // 全部网格对象
    public GridPoint[,] gridPoints;

    // 怪物路径点
    public List<GridPoint.GridIndex> monsterPath;
    public List<Vector3> monsterPathPos;        // 怪物路径点的具体位置


    private SpriteRenderer bgSR;
    private SpriteRenderer roadSR;

    // 每一波次产生的怪物列表
    public List<Round.RoundInfo> roundInfoList;

    // 当前关卡索引
    public int bigLevelID;
    public int levelID;

    // 游戏物体
    public GameObject gridGo;

    public Carrot carrot;
    

    private void Awake()
    {
#if MapEditing
        _instrance = this;
        InitMapMaker();
#endif
    }

    // 地图编辑器初始化
    public void InitMapMaker()
    {
        CalculateSize();
        gridPoints = new GridPoint[xColumn, yRow];
        monsterPath = new List<GridPoint.GridIndex>();
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
#if MapEditing
                GameObject itemGo = Instantiate(gridGo, transform.position, transform.rotation);
#endif

#if GameRuning
                GameObject itemGo = GameController.Instance.GetGameObjectResource("Grid");
#endif
                Vector2 pos = new Vector2(-mapWidth / 2 + gridWidth / 2, -mapHeight / 2 + gridHeight / 2);  // 起始位置
                itemGo.transform.position = new Vector3(x * gridWidth + pos.x, y * gridHeight + pos.y);

                itemGo.transform.SetParent(transform);

                gridPoints[x, y] = itemGo.GetComponent<GridPoint>();
                gridPoints[x, y].gridIndex.xIndex = x;
                gridPoints[x, y].gridIndex.yIndex = y;
            }
        }

        bgSR = transform.Find("BG").GetComponent<SpriteRenderer>();
        roadSR = transform.Find("Road").GetComponent<SpriteRenderer>();

        bgSR.sprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + bigLevelID.ToString() + "/" + "BG" + (levelID / 3).ToString());
        roadSR.sprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + bigLevelID.ToString() + "/" + "Road" + levelID.ToString());
    }

    // 地图初始化
    public void InitMap()
    {
#if MapEditing
        RecoverTowerPoint();
#endif
        bigLevelID = 0;
        levelID = 0;
        roundInfoList.Clear();
        bgSR.sprite = null;
        roadSR.sprite = null;
    }

    // 计算网格宽高
    private void CalculateSize()
    {
        Vector3 leftDown = new Vector3(0, 0);
        Vector3 rightUp = new Vector3(1, 1);

        Vector3 posOne = Camera.main.ViewportToWorldPoint(leftDown);
        Vector3 posTwo = Camera.main.ViewportToWorldPoint(rightUp);

        mapWidth = posTwo.x - posOne.x;
        mapHeight = posTwo.y - posOne.y;

        gridWidth = mapWidth / xColumn;
        gridHeight = mapHeight / yRow;
    }

#if MapEditing

    // 绘制辅助网格
    private void OnDrawGizmos()
    {
        if (drawLine)
        {

            CalculateSize();
            Gizmos.color = Color.green;

            // 画行
            for (int y = 0; y <= yRow; y++)
            {
                Vector3 startPos = new Vector3(-mapWidth / 2, -mapHeight / 2 + gridHeight * y);
                Vector3 endPos = new Vector3(mapWidth / 2, -mapHeight / 2 + gridHeight * y);
                Gizmos.DrawLine(startPos, endPos);

                //Debug.Log(startPos + "     " + endPos);
            }

            // 画列
            for (int x = 0; x <= xColumn; x++)
            {
                Vector3 startPos = new Vector3(-mapWidth / 2 + gridWidth * x, -mapHeight / 2);
                Vector3 endPos = new Vector3(-mapWidth / 2 + gridWidth * x, mapHeight / 2);
                Gizmos.DrawLine(startPos, endPos);
            }
        }
    }

    // 清除怪物路点
    public void ClearMonsterPath()
    {
        monsterPath.Clear();
    }

    // 恢复地图编辑默认状态
    public void RecoverTowerPoint()
    {
        ClearMonsterPath();
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                gridPoints[x, y].InitGrid();
            }
        }
    }

    // 生成LevelInfo对象用来保存文件
    private LevelInfo CreateLevelInfoGo()
    {
        LevelInfo levelInfo = new LevelInfo
        {
            bigLevelID = this.bigLevelID,
            levelID = this.levelID,
        };

        levelInfo.gridPoints = new List<GridPoint.GridState>();
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                levelInfo.gridPoints.Add(gridPoints[x, y].gridState);
            }
        }
        levelInfo.monsterPath = new List<GridPoint.GridIndex>();
        for (int i = 0; i < monsterPath.Count; i++)
        {
            levelInfo.monsterPath.Add(monsterPath[i]);
        }
        levelInfo.roundInfo = new List<Round.RoundInfo>();
        for (int i = 0; i < roundInfoList.Count; i++)
        {
            levelInfo.roundInfo.Add(roundInfoList[i]);
        }
        Debug.Log("数据构建成功");
        return levelInfo;
    }

    // 保存当前关卡的数据文件
    public void SaveLevelFileByJson()
    {
        LevelInfo levelInfoGo = CreateLevelInfoGo();
        string filePath = Application.dataPath + "/Resources/Json/Level/" + "Level" +  bigLevelID.ToString() + "_" + levelID.ToString() + ".json";
        string saveJsonStr = JsonMapper.ToJson(levelInfoGo);
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
    }

#endif


    // 读取关卡数据文件并转换为LevelInfo对象
    public LevelInfo LoadLevelInfoFile(string fileName)
    {
        LevelInfo levelInfo = new LevelInfo();

        string filePath = Application.dataPath + "/Resources/Json/Level/" + fileName + ".json";
        if (File.Exists(filePath)){
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            levelInfo = JsonMapper.ToObject<LevelInfo>(jsonStr);
        }
        else
        {
            Debug.LogError("文件加载失败 路径:" + filePath);
        }
        return levelInfo;
    }

    // 根据LevelInfo对象加载地图
    public void LoadLevelFile(LevelInfo levelInfo)
    {
        bigLevelID = levelInfo.bigLevelID;
        levelID = levelInfo.levelID;
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                gridPoints[x, y].gridState = levelInfo.gridPoints[yRow * x + y];
                // 更新网格状态
                gridPoints[x, y].UpdateGrid();
            }
        }

        monsterPath.Clear();
        for (int i = 0; i < levelInfo.monsterPath.Count; i++)
        {
            monsterPath.Add(levelInfo.monsterPath[i]);
        }
        roundInfoList = new List<Round.RoundInfo>();
        for (int i = 0; i < levelInfo.roundInfo.Count; i++)
        {
            roundInfoList.Add(levelInfo.roundInfo[i]);
        }
        bgSR.sprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + bigLevelID.ToString() + "/" +"BG" + (levelID / 3).ToString());
        roadSR.sprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + bigLevelID.ToString() + "/" + "Road" + levelID.ToString());
    }

#if GameRuning
    // 加载地图
    public void LoadMap(int bigLevel, int level)
    {
        bigLevelID = bigLevel;
        levelID = level;
        LoadLevelFile(LoadLevelInfoFile("Level"+ bigLevelID.ToString() + "_" + levelID.ToString()));
        monsterPathPos = new List<Vector3>();
        for (int i = 0; i < monsterPath.Count; i++)
        {
            monsterPathPos.Add(gridPoints[monsterPath[i].xIndex, monsterPath[i].yIndex].transform.position);
        }

        // 起始点与终止点
        GameObject startPointGo = GameController.Instance.GetGameObjectResource("startPoint");
        startPointGo.transform.position = monsterPathPos[0];
        startPointGo.transform.SetParent(transform);

        GameObject endPointGo = GameController.Instance.GetGameObjectResource("Carrot");
        endPointGo.transform.position = monsterPathPos[monsterPathPos.Count - 1];
        endPointGo.transform.SetParent(transform);
        carrot = endPointGo.GetComponent<Carrot>();
        
    }
#endif
}