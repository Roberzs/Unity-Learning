/****************************************************
    文件：GameNormalLevelPanel.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/13 23:04:32
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNormalLevelPanel : BasePanel
{
    public int currentBigLevelID;
    public int currentLevelID;

    private string filePath;            // 存储图片的根路径
    private string theSpritePath;
    private Transform levelContentTrans;
    private GameObject Img_LockBtnGo;
    private Transform emp_TowerTrans;
    private Image img_BGLeft;
    private Image img_BGRight;
    private Image img_Carrot;
    private Image img_AllClear;
    private Text txt_TotalWaves;

    private PlayerManager playerManager;
    private SlideScrollView slideScrollView;

    private List<GameObject> levelContentImageGos;
    private List<GameObject> towerContentImageGos;

    protected override void Awake()
    {
        base.Awake();
        filePath = "GameOption/Normal/Level/";
        playerManager = mUIFacade.mPlayerManager;
        levelContentImageGos = new List<GameObject>();
        towerContentImageGos = new List<GameObject>();
        levelContentTrans = transform.Find("Scroll View").Find("Viewport").Find("Content");
        Img_LockBtnGo = transform.Find("Img_LockBtn").gameObject;
        emp_TowerTrans = transform.Find("Emp_Tower");
        img_BGLeft = transform.Find("Img_BGLeft").GetComponent<Image>();
        img_BGRight = transform.Find("Img_BGRight").GetComponent<Image>();
        txt_TotalWaves = transform.Find("Img_Total").Find("Text").GetComponent<Text>();
        slideScrollView = transform.Find("Scroll View").GetComponent<SlideScrollView>();
        currentBigLevelID = 1;
        currentLevelID = 1;
    }

    // 加载资源
    private void LoadResource()
    {
        mUIFacade.GetSprite(filePath + "AllClear");
        mUIFacade.GetSprite(filePath + "Carrot_1");
        mUIFacade.GetSprite(filePath + "Carrot_2");
        mUIFacade.GetSprite(filePath + "Carrot_3");
        for (int i = 1; i <= 3; i++)
        {
            string spritePath = filePath + i.ToString() + "/";
            mUIFacade.GetSprite(spritePath + "BG_Left");
            mUIFacade.GetSprite(spritePath + "BG_Right");
            for (int j = 1; j <= 5; j++)
            {
                mUIFacade.GetSprite(spritePath + "Level_" + j.ToString());
            }
        }

        for (int i = 1; i <= 12; i++)
        {
            mUIFacade.GetSprite(filePath + "Tower/Tower_" + i.ToString());
        }

    }

    // 更新地图卡UI的方法
    public void UpdateMapUI(string spritePath)
    {
        img_BGLeft.sprite = mUIFacade.GetSprite(spritePath + "BG_Left");
        img_BGRight.sprite = mUIFacade.GetSprite(spritePath + "BG_Right");
        for (int i = 0; i < 5; i++)
        {
            levelContentImageGos.Add(CreateUIAndSetUIPosition("Img_Level", levelContentTrans));
            // 更换关卡显示图片
            levelContentImageGos[i].GetComponent<Image>().sprite = mUIFacade.GetSprite(spritePath + "Level_" + (i + 1).ToString());
            //Debug.Log(spritePath + "Level_" + (i + 1).ToString());

            Stage stage = playerManager.unLockedNormalModelLevelList[(currentBigLevelID - 1) * 5 + i];
            levelContentImageGos[i].transform.Find("Img_Carrot").gameObject.SetActive(false);
            levelContentImageGos[i].transform.Find("Img_AllClear").gameObject.SetActive(false);
            if (stage.unLocked)
            {
                // 关卡已解锁
                if (stage.mAllClear)
                {
                    levelContentImageGos[i].transform.Find("Img_AllClear").gameObject.SetActive(true);
                }
                if (stage.mCarrotState != 0)
                {
                    Image carrotImageGo = levelContentImageGos[i].transform.Find("Img_Carrot").GetComponent<Image>();
                    carrotImageGo.gameObject.SetActive(true);
                    carrotImageGo.sprite = mUIFacade.GetSprite(filePath + "Carrot_" + stage.mCarrotState);
                }

                levelContentImageGos[i].transform.Find("Img_Lock").gameObject.SetActive(false);
                levelContentImageGos[i].transform.Find("Img_BG").gameObject.SetActive(false);
            }
            else
            {
                // 关卡未解锁
                if (stage.mIsRewardLevel)
                {
                    // 隐藏关卡
                    levelContentImageGos[i].transform.Find("Img_Lock").gameObject.SetActive(false);
                    levelContentImageGos[i].transform.Find("Img_BG").gameObject.SetActive(true);
                    Image monsterPetImage = levelContentImageGos[i].transform.Find("Img_BG").Find("Img_Monster").GetComponent<Image>();
                    monsterPetImage.sprite = mUIFacade.GetSprite("MonsterNest/Monster/Baby/" + currentBigLevelID.ToString());
                    monsterPetImage.SetNativeSize();
                    monsterPetImage.transform.localScale = Vector3.one;
                }
                else
                {
                    // 不是隐藏关卡
                    levelContentImageGos[i].transform.Find("Img_Lock").gameObject.SetActive(true);
                    levelContentImageGos[i].transform.Find("Img_BG").gameObject.SetActive(false);
                }
            }
        }
        // 设置滚动视图的大小
        slideScrollView.SetContentLength(5);
        // 设置需要发送消息
        slideScrollView.needSeedMessage = true;
    }

    // 实例化UI
    public GameObject CreateUIAndSetUIPosition(string uiName, Transform parentTrans)
    {
        GameObject itemGo = mUIFacade.GetGameObjectResource(FactoryType.UIFactory, uiName);
        itemGo.transform.SetParent(parentTrans);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        return itemGo;
    }

    // 销毁地图卡
    private void DestoryMapUI()
    {
        if (levelContentImageGos.Count > 0)
        {
            for (int i =0; i < 5; i++)
            {
                mUIFacade.PushGameObjectToFactory(FactoryType.UIFactory, "Img_Level", levelContentImageGos[i]);
            }
        }
        levelContentImageGos.Clear();
    }

    // 更新静态UI
    public void UpdateLevelUI(string spritePaht)
    {
        if (towerContentImageGos.Count != 0)
        {
            for (int i = 0; i < towerContentImageGos.Count; i++)
            {
                towerContentImageGos[i].GetComponent<Image>().sprite = null;
                mUIFacade.PushGameObjectToFactory(FactoryType.UIFactory, "Img_Tower", towerContentImageGos[i]);
            }
            towerContentImageGos.Clear();
        }

        Stage stage = playerManager.unLockedNormalModelLevelList[(currentBigLevelID - 1) * 5 + currentLevelID - 1];
        if (stage.unLocked)
        {
            Img_LockBtnGo.SetActive(false);
        }
        else
        {
            Img_LockBtnGo.SetActive(true);
        }
        txt_TotalWaves.text = stage.mTotalRound.ToString();
        for (int i = 0; i < stage.mTowerIDListLength; i++)
        {
            towerContentImageGos.Add(CreateUIAndSetUIPosition("Img_Tower", emp_TowerTrans));
            towerContentImageGos[i].GetComponent<Image>().sprite = mUIFacade.GetSprite(filePath + "Tower/" + "Tower_" + stage.mTowerIDList[i].ToString());
        }

    }

    public void ToThisPanel(int currentBigLevel)
    {
        currentBigLevelID = currentBigLevel;
        EnterPanel();
    }

    public override void InitPanel()
    {
        base.InitPanel();
        gameObject.SetActive(false);
    }

    public override void EnterPanel()
    {
        base.EnterPanel();
        gameObject.SetActive(true);
        theSpritePath = filePath + currentBigLevelID.ToString() + "/";
        DestoryMapUI();
        UpdateMapUI(theSpritePath);
        UpdateLevelUI(theSpritePath);
        slideScrollView.Init();
        currentLevelID = 1;
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();
        UpdateLevelUI(theSpritePath);
    }

    public override void ExitPanel()
    {
        base.ExitPanel();
        gameObject.SetActive(false);
    }

    public void ToGamePanel()
    {
        GameManager.Instance.currentStage = playerManager.unLockedNormalModelLevelList[(currentBigLevelID - 1) * 5 + currentLevelID];
        mUIFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        mUIFacade.ChangeSceneState(new NormalModelSceneState(mUIFacade));
    }

    public void ToLastLevel()
    {
        
        currentLevelID--;
        UpdatePanel();
    }

    public void ToNextLevel()
    {
        currentLevelID++;
        UpdatePanel();
    }
}
