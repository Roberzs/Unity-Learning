/****************************************************
    文件：ResourcesAssetProxyFactory.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/25 22:55:39
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesAssetProxyFactory: IAssetFactory
{
    private ResourcesAssetFactory mAssetFactory = new ResourcesAssetFactory();
    private Dictionary<string, GameObject> mSoldiers = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> mEnemys = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> mEffects = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> mWeapons = new Dictionary<string, GameObject>();
    private Dictionary<string, AudioClip> mAudioClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, Sprite> mSprites = new Dictionary<string, Sprite>();

    public AudioClip LoadAudioClip(string name)
    {
        if (mAudioClips.ContainsKey(name))
        {
            return mAudioClips[name];
        }
        AudioClip audioClip = Resources.Load(ResourcesAssetFactory.AudioPath + name, typeof(AudioClip)) as AudioClip;
        mAudioClips.Add(name, audioClip);
        return audioClip;
    }

    public GameObject LoadEffect(string name)
    {
        if (mEffects.ContainsKey(name))
        {
            return GameObject.Instantiate(mEffects[name]);
        }
        GameObject asset = mAssetFactory.LoadAsset(ResourcesAssetFactory.EffectPath + name) as GameObject;
        mEffects.Add(name, asset);
        return GameObject.Instantiate(asset);
    }

    public GameObject LoadEnemy(string name)
    {
        if (mEnemys.ContainsKey(name))
        {
            return GameObject.Instantiate(mEnemys[name]);
        }
        GameObject asset = mAssetFactory.LoadAsset(ResourcesAssetFactory.EnemyPath + name) as GameObject;
        mEnemys.Add(name, asset);
        return GameObject.Instantiate(asset);
    }

    public Sprite LoadSprite(string name)
    {
        if (mSprites.ContainsKey(name))
        {
            return mSprites[name];
        }
        Sprite sprite = Resources.Load(ResourcesAssetFactory.SpritePath + name, typeof(Sprite)) as Sprite;
        mSprites.Add(name, sprite);
        return sprite;
    }

    public GameObject LoadWeapon(string name)
    {
        if (mWeapons.ContainsKey(name))
        {
            return GameObject.Instantiate(mWeapons[name]);
        }
        GameObject asset = mAssetFactory.LoadAsset(ResourcesAssetFactory.WeaponPath + name) as GameObject;
        mWeapons.Add(name, asset);
        return GameObject.Instantiate(asset);
    }

    public GameObject LodaSoldier(string name)
    {
        if (mSoldiers.ContainsKey(name))
        {
            return GameObject.Instantiate(mSoldiers[name]);
        }
        GameObject asset = mAssetFactory.LoadAsset(ResourcesAssetFactory.SoldierPath + name) as GameObject;
        mSoldiers.Add(name, asset);
        return GameObject.Instantiate(asset);
    }
}
