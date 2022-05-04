/****************************************************
	文件：Invincible.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class Invincible : Item
{
    public override void HitPlayer(Transform player)
    {
        base.HitPlayer(player);

		player.SendMessage("HitInvincible", SendMessageOptions.DontRequireReceiver);
    }
}
