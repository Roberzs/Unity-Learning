/****************************************************
    文件：ButtonUpLevel.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class ButtonUpLevel : MonoBehaviour 
{
    private int price;
    private Button button;
    private Text text;
    private Image image;

    private Sprite canUpLevelSprite;
    private Sprite cantUpLevelSprite;
    private Sprite reachHighestLevel;

    private GameController gameController;

    private void OnEnable()
    {
        if (text == null) return;
        UpdateUIView();
    }

    private void Start()
    {
        gameController = GameController.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(UpLevel);
        canUpLevelSprite = gameController.GetSprite("NormalMordel/Game/Tower/Btn_CanUpLevel");
        cantUpLevelSprite = gameController.GetSprite("NormalMordel/Game/Tower/Btn_CantUpLevel");
        reachHighestLevel = gameController.GetSprite("NormalMordel/Game/Tower/Btn_ReachHighestLevel");
        text = transform.Find("Text").GetComponent<Text>();
        image = GetComponent<Image>();
    }

    private void UpdateUIView()
    {
        if (gameController.selectGrid.towerLevel >= 3)      // 塔已升至顶级
        {
            image.sprite = reachHighestLevel;
            button.interactable = false;
            text.enabled = false;
        }
        else
        {
            text.enabled = true;
            price = gameController.selectGrid.towerPersonalProperty.upLevelPrice;
            text.text = price.ToString();
            if (gameController.coin >= price)
            {
                image.sprite = canUpLevelSprite;
                button.interactable = true;
            }
            else
            {
                image.sprite = cantUpLevelSprite;
                button.interactable = false;
            }
        }
    }

    private void UpLevel()
    {
        // 给建造者赋值升级塔的属性
        gameController.towerBuilder.m_TowerID = gameController.selectGrid.tower.towerID;
        gameController.towerBuilder.m_TowerLevel = gameController.selectGrid.towerLevel + 1;

        // 销毁之前等级的塔
        gameController.selectGrid.towerPersonalProperty.UpLevelTower();

        // 生成升级塔以及后续处理
        GameObject towerGo = gameController.towerBuilder.GetProduct();
        towerGo.transform.SetParent(gameController.selectGrid.transform);
        towerGo.transform.position = gameController.selectGrid.transform.position;
        gameController.selectGrid.AfterBuild();
        gameController.selectGrid.HideGrid();
        gameController.selectGrid = null;
    }
}