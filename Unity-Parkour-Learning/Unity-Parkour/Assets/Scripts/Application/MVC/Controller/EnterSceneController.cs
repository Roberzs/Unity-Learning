/****************************************************
	文件：EnterSceneController.cs
	作者：Zhangying
	邮箱：zhy18125@gmail.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class EnterSceneController : Controller
{
    public override void Execute(object data)
    {
		SceneArgs e = data as SceneArgs;
        switch (e.sceneIndex)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                // 游戏场景
                RegisterView(GameObject.Find(TagDefine.Player).GetComponent<PlayerMove>());
                RegisterView(GameObject.Find(TagDefine.Player).GetComponent<PlayerAnim>());
                break;
            case 4:
                break;
            default:
                break;
        }
    }
}
