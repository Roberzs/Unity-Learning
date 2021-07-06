/****************************************************
    文件：Heart.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class Heart : MonoBehaviour 
{
    public float animationTime;
    public string resourcePath;

    private void OnEnable()
    {
        Invoke("DestroyEffect", animationTime);
    }
    private void DestroyEffect()
    {
        GameManager.Instance.factoryManager.factoryDict[FactoryType.UIFactory].PushItem(resourcePath, gameObject);
    }
}