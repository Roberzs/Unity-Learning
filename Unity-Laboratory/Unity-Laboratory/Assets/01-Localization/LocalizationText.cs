/****************************************************
    文件：LocalizationText.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/4/1 22:44:24
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class LocalizationText : MonoBehaviour
{
    public string key = " ";
    void Start()
    {
        GetComponent<Text>().text = LocalizationManager.GetInstance.GetValue(key);
    }

}
