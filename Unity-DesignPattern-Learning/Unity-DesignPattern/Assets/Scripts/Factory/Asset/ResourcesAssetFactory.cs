/****************************************************
    文件：ResourcesAssetFactory.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/9 13:54:41
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesAssetFactory : IAssetFactory
{
    private const string SoldierPath = "Characters/Soldier/";
    private const string EnemyPaht = "Characters/Enenmy/";
    private const string WeaponPath = "Weapons/";
    private const string EffectPath = "Effects/";
    private const string AudioPath = "Audio/";
    private const string SpritePath = "Sprites/";

    public AudioClip LoadAudioClip(string name)
    {
        return LoadAsset(AudioPath + name) as AudioClip;
    }

    public GameObject LoadEffect(string name)
    {
        return InstantiateGameObject(EffectPath + name);
    }

    public GameObject LoadEnemy(string name)
    {
        return InstantiateGameObject(EnemyPaht + name);
    }

    public Sprite LoadSprite(string name)
    {
        return LoadSprite(SpritePath + name) as Sprite;
    }

    public GameObject LoadWeapon(string name)
    {
        return InstantiateGameObject(WeaponPath + name);
    }

    public GameObject LodaSoldier(string name)
    {
        return InstantiateGameObject(SoldierPath + name);
    }

    // 实例化对象
    private GameObject InstantiateGameObject(string path)
    {
        UnityEngine.Object o = Resources.Load(path);
        if (o == null)
        {
            Debug.LogError("无法加载该资源，路径错误:" + path); return null;
        }
        return UnityEngine.GameObject.Instantiate(o) as GameObject;
    }
    
    // 加载对象
    private UnityEngine.Object LoadAsset(string path)
    {
        UnityEngine.Object o = Resources.Load(path);
        if (o == null)
        {
            Debug.LogError("无法加载该资源，路径错误:" + path); return null;
        }
        return o;
    }
}
