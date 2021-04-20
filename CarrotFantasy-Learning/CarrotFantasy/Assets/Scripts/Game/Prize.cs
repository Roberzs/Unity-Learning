/****************************************************
    文件：Prize.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class Prize : MonoBehaviour 
{
#if GameRuning
    private void Update()
    {
        if (GameController.Instance.gameOver)
        {
            GameController.Instance.PushGameObjectToFactory("Prize", gameObject);
        }
    }

    private void OnMouseDown()
    {
        GameController.Instance.PlayEffectMusic("NormalMordel/GiftGot");
        GameController.Instance.isPause = true;
        GameController.Instance.ShowPrizePage();
        GameController.Instance.PushGameObjectToFactory("Prize", gameObject);
    }
#endif
}