/****************************************************
	文件：Coin.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections;
using UnityEngine;

public class Coin : Item
{
	float flySpeed = 40.0f;

    public override void HitPlayer(Transform player)
    {
        
		PlayEffect("FX_JinBi", player.position);
		GameRoot.Instance.soundManager.PlayEffectAudio("Se_UI_JinBi");

		player.SendMessage("HitCoin", SendMessageOptions.DontRequireReceiver);

		base.HitPlayer(player);
	}

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
		if (other.CompareTag(TagDefine.MagnetCollider))
        {
			StartCoroutine(HitMagnet(other.transform));
        }
    }

	IEnumerator HitMagnet(Transform player)
    {
		bool isLoop = true;
        while (isLoop)
        {
			transform.position = Vector3.Lerp(transform.position, player.position, flySpeed * Time.deltaTime);
			if (Vector3.Distance(transform.position, player.position) < 0.2f)
            {
				isLoop = false;
            }
			yield return new WaitForEndOfFrame();
        }
    }
}
