/****************************************************
    文件：IWeaponFactory.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/10 16:15:27
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponFactory
{
    IWeapon CreateWeapon(WeaponType weaponType);
}
