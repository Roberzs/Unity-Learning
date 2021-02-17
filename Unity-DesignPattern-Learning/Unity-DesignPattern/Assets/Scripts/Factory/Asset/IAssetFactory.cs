/****************************************************
    文件：IAssetFactory.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/9 13:30:55
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAssetFactory
{
    GameObject LodaSoldier(string name);        // 战士
    GameObject LoadEnemy(string name);          // 敌人
    GameObject LoadWeapon(string name);         // 武器
    GameObject LoadEffect(string name);         // 特效
    AudioClip LoadAudioClip(string name);      // 声音
    Sprite LoadSprite(string name);         // 头像
}
