/****************************************************
    文件：FactoryManager.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class FactoryManager : MonoSingleton<FactoryManager>
{
    public BaseFactory prefabsFactory;
    public BaseResourcesFactory<AudioClip> audioClipFactory;
    public BaseResourcesFactory<Sprite> spriteFactory;

    protected override void Awake()
    {
        base.Awake();
        prefabsFactory = new BaseFactory(StringDefine.PrefabsRootPath, gameObject);
        audioClipFactory = new BaseResourcesFactory<AudioClip>(StringDefine.AudioClipsRootPath);
        spriteFactory = new BaseResourcesFactory<Sprite>(StringDefine.SpritesRootPath);

        Debug.Log("FactoryManager Init Done.");
    }

    // 获取Sprite资源
    public Sprite GetSprite(string resourcePath)
    {
        return spriteFactory.GetSingleResources(resourcePath);
    }

    // 获取AudioClip资源
    public AudioClip GetAudioClip(string resourcePath)
    {
        return audioClipFactory.GetSingleResources(resourcePath);
    }


    // 获取游戏物体
    public GameObject GetGameObjectResource(string resourcePath)
    {
        return prefabsFactory.GetItem(resourcePath);
    }

    // 将游戏物体放回对象池
    public void PushGameObjectToFactory(string resourcePath, GameObject itemGo)
    {
        prefabsFactory.PushItem(resourcePath, itemGo);
    }
}
