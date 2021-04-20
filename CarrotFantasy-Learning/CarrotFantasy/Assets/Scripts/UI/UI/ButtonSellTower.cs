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
#if GameRuning
    private int price;
    private Button button;
    private Text text;

    private void OnEnable()
    {
        if (text == null) return;

        price = GameController.Instance.selectGrid.towerPersonalProperty.sellPrice;
        text.text = price.ToString();

    }

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SellTower);
    }

    private void SellTower()
    {
        GameController.Instance.selectGrid.towerPersonalProperty.SellTower();

        GameController.Instance.selectGrid.handleTowerCanvasGo.SetActive(false);
        GameController.Instance.selectGrid.InitGrid();
        GameController.Instance.selectGrid.HideGrid();
        
        GameController.Instance.selectGrid = null;
    }
#endif
}