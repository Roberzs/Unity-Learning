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

#if GameRuning

    private void OnEnable()
    {
        if (text == null) return;
        UpdateUIView();
    }

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(UpLevel);
        canUpLevelSprite = GameController.Instance.GetSprite("NormalMordel/Game/Tower/Btn_CanUpLevel");
        cantUpLevelSprite = GameController.Instance.GetSprite("NormalMordel/Game/Tower/Btn_CantUpLevel");
        reachHighestLevel = GameController.Instance.GetSprite("NormalMordel/Game/Tower/Btn_ReachHighestLevel");
        text = transform.Find("Text").GetComponent<Text>();
        image = GetComponent<Image>();
    }

    private void UpdateUIView()
    {
        if (GameController.Instance.selectGrid.towerLevel >= 3)      // 塔已升至顶级
        {
            image.sprite = reachHighestLevel;
            button.interactable = false;
            text.enabled = false;
        }
        else
        {
            text.enabled = true;
            price = GameController.Instance.selectGrid.towerPersonalProperty.upLevelPrice;
            text.text = price.ToString();
            if (GameController.Instance.coin >= price)
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
        GameController.Instance.towerBuilder.m_TowerID = GameController.Instance.selectGrid.tower.towerID;
        GameController.Instance.towerBuilder.m_TowerLevel = GameController.Instance.selectGrid.towerLevel + 1;

        // 销毁之前等级的塔
        GameController.Instance.selectGrid.towerPersonalProperty.UpLevelTower();

        // 生成升级塔以及后续处理
        GameObject towerGo = GameController.Instance.towerBuilder.GetProduct();
        towerGo.transform.SetParent(GameController.Instance.selectGrid.transform);
        towerGo.transform.position = GameController.Instance.selectGrid.transform.position;
        GameController.Instance.selectGrid.AfterBuild();
        GameController.Instance.selectGrid.HideGrid();
        GameController.Instance.selectGrid = null;
    }
#endif
}