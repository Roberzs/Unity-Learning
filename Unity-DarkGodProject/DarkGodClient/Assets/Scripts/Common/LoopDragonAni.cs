/****************************************************
    文件：LoopDragonAni.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/11/26 22:19:12
	功能：Nothing
*****************************************************/

using UnityEngine;

public class LoopDragonAni : MonoBehaviour 
{
    private new Animation animation;

    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    private void Start()
    {
        if (animation != null)
        {
            InvokeRepeating("LoopPlayAni", 0, 20);
        }
    }

    private void LoopPlayAni()
    {
        if (animation != null)
        {
            animation.Play();
        }
    }
}