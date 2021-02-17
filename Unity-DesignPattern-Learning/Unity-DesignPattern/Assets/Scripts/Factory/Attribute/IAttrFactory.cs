/****************************************************
    文件：IAttrFactory.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/15 23:11:51
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAttrFactory
{
    CharacterBaseAttr GetCharacterBaseAttr(System.Type t);      // 通过角色类别获取基础属性
    WeaponBaseAttr GetWeaponBaseAttr(WeaponType weaponType);     // 通过武器类别获取基础属性
}
