/****************************************************
	文件：Obstacles.cs
	作者：Zhangying
	邮箱：zhy18125@gmail.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class Obstacles : MonoBehaviour
{
	public string SelfPath;

	public void OnEnable()
    {

    }

	public void OnDisable()
    {

    }

	public void HitPlayer()
    {
		// 播放特效
		GameObject itemEff = GameRoot.Instance.factoryManager.GetGameObjectResource("Effect/FX_ZhuangJi");
		itemEff.transform.position = transform.position;

		// 播放声音
		GameRoot.Instance.soundManager.PlayEffectAudio("");

		// 资源回收
		//GameRoot.Instance.factoryManager.PushGameObjectToFactory(SelfPath, gameObject);
		Destroy(gameObject);
    }
}
