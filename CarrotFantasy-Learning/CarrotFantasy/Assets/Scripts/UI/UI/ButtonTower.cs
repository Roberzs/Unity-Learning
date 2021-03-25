/****************************************************
    文件：ButtonTower.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/23 13:03:38
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTower : MonoBehaviour
{
    public int towerID;
    public int price;
    private Button button;
    private Sprite canClickSprite;
    private Sprite cantClickSprite;
    private Image image;
    private GameController gameController;

    private void Start()
    {
        gameController = GameController.Instance;

        image = GetComponent<Image>();
        button = GetComponent<Button>();

        canClickSprite = gameController.GetSprite("NormalMordel/Game/Tower/" + towerID.ToString() + "/CanClick1");
        cantClickSprite = gameController.GetSprite("NormalMordel/Game/Tower/" + towerID.ToString() + "/CanClick0");

        price = gameController.towerPriceDict[towerID];

        UpdateIcon();

        button.onClick.AddListener(BuildTower);
    }

    private void OnEnable()
    {
        if (price == 0)
        {
            return;
        }
        UpdateIcon();
    }

    // 更新图标显示
    private void UpdateIcon()
    {
        if (gameController.coin > price)
        {
            image.sprite = canClickSprite;
            button.interactable = true;
        }
        else
        {
            image.sprite = cantClickSprite;
            button.interactable = false;
        }
    }

    // 建塔
    private void BuildTower()
    {
        gameController.towerBuilder.m_TowerID = towerID;
        gameController.towerBuilder.m_TowerLevel = 1;
        GameObject towerGo = gameController.towerBuilder.GetProduct();
        towerGo.transform.SetParent(gameController.selectGrid.transform);
        towerGo.transform.position = gameController.selectGrid.transform.position;

        // 特效
        GameObject effectGo = gameController.GetGameObjectResource("BuildEffect");
        effectGo.transform.SetParent(gameController.transform);
        effectGo.transform.position = gameController.selectGrid.transform.position;

        // 建塔完成后 网格的后续操作
        gameController.selectGrid.AfterBuild();
        gameController.selectGrid.HideGrid();
        gameController.selectGrid.hasTower = true;
        gameController.ChangeCoin(-price);

        gameController.selectGrid = null;
        gameController.handleTowerCanvasGo.SetActive(false);

    }
}
