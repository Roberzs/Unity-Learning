/****************************************************
    文件：ICharacterVisitor.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/24 22:20:27
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ICharacterVisitor
{
    public abstract void VisitEnemy(IEnemy enemy);
    public abstract void VisitSoldier(ISoldier soldier);
}
