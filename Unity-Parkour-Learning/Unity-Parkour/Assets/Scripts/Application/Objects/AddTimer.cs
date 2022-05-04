/****************************************************
	文件：AddTimer.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class AddTimer : Item
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void HitPlayer(Transform player)
    {
        
        GameRoot.Instance.soundManager.PlayEffectAudio("Se_UI_Time");

        player.SendMessage("HitAddTimer", SendMessageOptions.DontRequireReceiver);

        base.HitPlayer(player);
    }
}
