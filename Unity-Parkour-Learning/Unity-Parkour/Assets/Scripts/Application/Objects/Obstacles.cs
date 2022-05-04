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
	
	protected virtual void Awake()
    {

    }

	protected virtual void OnEnable()
    {

    }

	protected virtual void OnDisable()
    {

    }

	public virtual void HitPlayer()
    {
		// 播放特效
		GameObject itemEff = GameRoot.Instance.factoryManager.GetGameObjectResource("Effect/FX_ZhuangJi");
		itemEff.transform.position = transform.position;

		// 资源回收
		//GameRoot.Instance.factoryManager.PushGameObjectToFactory(SelfPath, gameObject);
		Destroy(gameObject);
    }
}
