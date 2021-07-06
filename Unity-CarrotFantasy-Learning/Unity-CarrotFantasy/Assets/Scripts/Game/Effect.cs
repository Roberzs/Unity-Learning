/****************************************************
    文件：Effect.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/22 18:25:36
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float animationTime;
    public string resourcePath;

    private void OnEnable()
    {
        Invoke("DestroyEffect", animationTime);
    }
    private void DestroyEffect()
    {
        GameController.Instance.PushGameObjectToFactory(resourcePath, gameObject);
    }
}
