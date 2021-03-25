/****************************************************
    文件：ButtonSellTower.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class ButtonSellTower : MonoBehaviour 
{
    private int price;
    private Button button;
    private Text text;
    private GameController gameController;

    private void OnEnable()
    {
        if (text == null) return;

        price = gameController.selectGrid.towerPersonalProperty.sellPrice;
        text.text = price.ToString();

    }

    private void Start()
    {
        gameController = GameController.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(SellTower);
    }

    private void SellTower()
    {
        gameController.selectGrid.towerPersonalProperty.SellTower();

        gameController.selectGrid.handleTowerCanvasGo.SetActive(false);
        gameController.selectGrid.InitGrid();
        gameController.selectGrid.HideGrid();
        
        gameController.selectGrid = null;
    }
}