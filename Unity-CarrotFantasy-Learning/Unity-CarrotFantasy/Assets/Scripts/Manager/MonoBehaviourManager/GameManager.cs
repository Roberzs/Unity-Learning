/****************************************************
    文件：GameManager.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：游戏总管理，管理所有的管理者
*****************************************************/

using UnityEngine;

public class GameManager : MonoBehaviour 
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    public bool initPlayerManager;      // 是否重置游戏

    public LevelType levelType;
    public Stage currentStage;  

    public PlayerManager playerManager;
    public FactoryManager factoryManager;
    public AudioSourceManager audioSourceManager;
    public UIManager uIManager;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _instance = this;

        playerManager = new PlayerManager();
        //playerManager.SaveData();
        playerManager.ReadData();
        factoryManager = new FactoryManager();
        audioSourceManager = new AudioSourceManager();
        // 注释 用以方便测试
        uIManager = new UIManager();
        uIManager.mUIFacade.currentSceneState.EnterScene();
    }

    public GameObject CreateItem(GameObject itemGo)
    {
        GameObject go = Instantiate(itemGo);
        return go;
    }

    // 获取Sprite资源
    public Sprite GetSprite(string resourcePath)
    {
        return factoryManager.spriteFactory.GetSingleResources(resourcePath);
    }

    // 获取AudioClip资源
    public AudioClip GetAudioClip(string resourcePath)
    {
        return factoryManager.audioClipFactory.GetSingleResources(resourcePath);
    }

    // 获取动画控制器资源
    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return factoryManager.runtimeAnimatorControllerFactory.GetSingleResources(resourcePath);
    }

    // 获取游戏物体
    public GameObject GetGameObjectResource(FactoryType factoryType, string resourcePath)
    {
        return factoryManager.factoryDict[factoryType].GetItem(resourcePath);
    }

    // 将游戏物体放回对象池
    public void PushGameObjectToFactory(FactoryType factoryType, string resourcePath, GameObject itemGo)
    {
        factoryManager.factoryDict[factoryType].PushItem(resourcePath, itemGo);
    }
}