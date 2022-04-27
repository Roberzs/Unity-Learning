/****************************************************
	文件：Effects.cs
	作者：Zhangying
	邮箱：zhy18125@gmail.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using System.Collections;

public class Effects : MonoBehaviour
{
	public float time = 3.0f;

    private void OnEnable()
    {
        StartCoroutine(OnDestroySelf());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator OnDestroySelf()
    {
        yield return new WaitForSeconds(time);
        // 对象池回收
        GameRoot.Instance.factoryManager.PushGameObjectToFactory("", gameObject);
    }
}
