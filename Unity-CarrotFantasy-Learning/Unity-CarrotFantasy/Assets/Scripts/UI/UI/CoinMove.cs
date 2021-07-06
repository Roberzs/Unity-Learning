/****************************************************
    文件：CoinMove.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CoinMove : MonoBehaviour 
{
    private Text coinText;
    private Image coinImage;
    public Sprite[] coinSprites;
    [HideInInspector]
    public int prize;

    private void Awake()
    {
        coinText = transform.Find("Txt_Coin").GetComponent<Text>();
        coinImage = transform.Find("Img_Coin").GetComponent<Image>();

        coinSprites = new Sprite[2];
        coinSprites[0] = GameController.Instance.GetSprite("NormalMordel/Game/Coin");
        coinSprites[1] = GameController.Instance.GetSprite("NormalMordel/Game/ManyCoin");
    }

    private void OnEnable()
    {
        ShowCoin();
    }

    private void ShowCoin()
    {
        coinText.text = prize.ToString();
        // 图片的显示
        if(prize >= 500)
        {
            coinImage.sprite = coinSprites[1];
        }
        else
        {
            coinImage.sprite = coinSprites[0];
        }

        transform.DOLocalMoveY(60, 0.5f);
        DOTween.To(
            ()=>coinImage.color,
            toColor => coinImage.color = toColor,
            new Color(1,1,1,0),
            0.5f
            );
        Tween tween = DOTween.To(
            () => coinText.color,
            toColor => coinText.color = toColor,
            new Color(1, 1, 1, 0),
            0.5f
            );
        tween.OnComplete(DestoryCoin);
    }

    // 销毁UI
    private void DestoryCoin()
    {
        transform.localPosition = Vector3.zero;
        coinText.color = coinImage.color = new Color(1, 1, 1, 1);
        GameController.Instance.PushGameObjectToFactory("CoinCanvas", transform.parent.gameObject);
    }
}