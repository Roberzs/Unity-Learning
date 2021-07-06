/****************************************************
    文件：AppOfPhone.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class AppOfPhone
{
    public int appNum;
    public bool phoneState;
    //public List<string> appList;      // 用于简单测试的字段
    public List<AppProperty> appList;
}

public class AppProperty
{
    public string appName;
    public string userID;
    public bool isFavour;
    public List<int> useTimeList;
}