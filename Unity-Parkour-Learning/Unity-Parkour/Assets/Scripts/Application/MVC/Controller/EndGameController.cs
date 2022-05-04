/****************************************************
	文件：EndGameController.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class EndGameController : Controller
{

    public override void Execute(object data)
    {
		GameModel gm = GetModel<GameModel>();
		gm.IsPlay = false;

		// TODO UI
    }
}
