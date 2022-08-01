/****************************************************
	文件：GamePauseController.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class GamePauseController : Controller
{
    public override void Execute(object data)
    {
		GameModel gm = GetModel<GameModel>();
		gm.IsPause = true;

		UIPause uIPause = GetView<UIPause>();
		uIPause.Show();
    }
}
