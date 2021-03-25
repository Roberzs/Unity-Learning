/****************************************************
    文件：IGameEventObserver.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/24 9:39:15
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGameEventObserver
{
    public abstract void Update();
    public abstract void SetSubject(IGameEventSubject sub);
}
