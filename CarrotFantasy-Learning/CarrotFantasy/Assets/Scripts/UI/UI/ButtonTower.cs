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
#if GameRuning

    public int towerID;
    public int price;
    private Button button;
    private Sprite canClickSprite;
    private Sprite cantClickSprite;
    private Image image;

    private void Start()
    {

        image = GetComponent<Image>();
        button = GetComponent<Button>();

        canClickSprite = GameController.Instance.GetSprite("NormalMordel/Game/Tower/" + towerID.ToString() + "/CanClick1");
        cantClickSprite = GameController.Instance.GetSprite("NormalMordel/Game/Tower/" + towerID.ToString() + "/CanClick0");

        price = GameController.Instance.towerPriceDict[towerID];

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
        if (GameController.Instance.coin > price)
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
        GameController.Instance.PlayEffectMusic("NormalMordel/Tower/TowerBulid");


        GameController.Instance.towerBuilder.m_TowerID = towerID;
        GameController.Instance.towerBuilder.m_TowerLevel = 1;
        GameObject towerGo = GameController.Instance.towerBuilder.GetProduct();
        towerGo.transform.SetParent(GameController.Instance.selectGrid.transform);
        towerGo.transform.position = GameController.Instance.selectGrid.transform.position;

        // 特效
        GameObject effectGo = GameController.Instance.GetGameObjectResource("BuildEffect");
        effectGo.transform.SetParent(GameController.Instance.transform);
        effectGo.transform.position = GameController.Instance.selectGrid.transform.position;

        // 建塔完成后 网格的后续操作
        GameController.Instance.selectGrid.AfterBuild();
        GameController.Instance.selectGrid.HideGrid();
        GameController.Instance.selectGrid.hasTower = true;
        GameController.Instance.ChangeCoin(-price);

        GameController.Instance.selectGrid = null;
        GameController.Instance.handleTowerCanvasGo.SetActive(false);

    }
#endif
}
