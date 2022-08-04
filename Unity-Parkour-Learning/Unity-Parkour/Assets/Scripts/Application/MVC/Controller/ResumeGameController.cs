/****************************************************
	文件：ResumeGameController.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class ResumeGameController : Controller
{
    public override void Execute(object data)
    {
		UIResume uiResume = GetView<UIResume>();
		uiResume.StartCounter();



	}
}
