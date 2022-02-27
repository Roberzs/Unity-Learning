using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class GameRoot : MonoBehaviour
{
    private AudioSource m_Audio;

    private AudioClip tmpClip;

    private GameObject tmpObj;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        m_Audio = GetComponent<AudioSource>();
        AssetBundleManager.Instance.LoadAssetBundleConfig();
        ResourceManager.Instance.Init(this);
        ObjectManager.Instance.Init(transform.Find("RecyclePoolTrs"), transform.Find("SceneTrs"));

    }

    private void Start()
    {
        //ResourceManager.Instance.AsyncLoadResource("Assets/GameData/Sounds/senlin.mp3", (string path, Object obj, object param1, object parma2, object param3) =>
        //{
        //    tmpClip = obj as AudioClip;
        //    m_Audio.clip = tmpClip;
        //    m_Audio.Play();
        //}, LoadResPriority.RES_MIDDLE);

        //ResourceManager.Instance.PreloadRes("Assets/GameData/Sounds/senlin.mp3");

        //tmpObj = ObjectManager.Instance.InstantiateObject("Assets/GameData/Prefabs/Attack.prefab", true);
        //ObjectManager.Instance.InstantiateObjectAsync("Assets/GameData/Prefabs/Attack.prefab", (string path, Object obj, object param1, object param2, object param3) =>
        //{
        //    tmpObj = obj as GameObject;
        //}, LoadResPriority.RES_HIGHT, true);

        float timer = Time.realtimeSinceStartup;
        //ObjectManager.Instance.PreloadGameObject("Assets/GameData/Prefabs/Attack.prefab", 10);
        Debug.Log($"加载所需时间:{Time.realtimeSinceStartup - timer}");

        LoadConfiger();

        // UI加载
        UIManager.Instance.Init(transform.Find("UIRoot") as RectTransform,
            transform.Find("UIRoot/WndRoot") as RectTransform,
            transform.Find("UIRoot/UICamera").GetComponent<Camera>(),
            transform.Find("UIRoot/EventSystem").GetComponent<EventSystem>());

        GameSceneManager.Instance.Init(this);

        RegisterUI();

        //UIManager.Instance.PopUpWnd("MenuPanel.prefab");

        ResourceManager.Instance.LoadResource<AudioClip>("Assets/GameData/Sounds/senlin.mp3");

        tmpObj = ObjectManager.Instance.InstantiateObject("Assets/GameData/Prefabs/Attack.prefab", true, false);
        ObjectManager.Instance.ReleaseResource(tmpObj);

        GameSceneManager.Instance.LoadSceen("MenuScene",
            () =>
            {
                UIManager.Instance.PopUpWnd("LoadingPanel.prefab", true);
            },
            () =>
            {
                //UIManager.Instance.OnUpdate
                UIManager.Instance.CloseWnd("LoadingPanel.prefab");
                UIManager.Instance.PopUpWnd("MenuPanel.prefab");
            });
    }

    void RegisterUI()
    {
        UIManager.Instance.Register<LoadingUI>("LoadingPanel.prefab");
        UIManager.Instance.Register<MenuUI>("MenuPanel.prefab");
    }

    void LoadConfiger()
    {
        ConfigManager.Instance.LoadData<MonsterData>("Assets/GameData/Data/Binary/MonsterData.bytes");
        ConfigManager.Instance.LoadData<BuffData>("Assets/GameData/Data/Binary/BuffData.bytes");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //float time = Time.realtimeSinceStartup;
            //tmpClip = ResourceManager.Instance.LoadResource<AudioClip>("Assets/GameData/Sounds/senlin.mp3");
            //Debug.Log($"资源加载时长:{Time.realtimeSinceStartup - time}");
            //m_Audio.clip = tmpClip;
            //m_Audio.Play();

            tmpObj = ObjectManager.Instance.InstantiateObject("Assets/GameData/Prefabs/Attack.prefab", true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //m_Audio.Stop();
            //m_Audio.clip = null;
            //ResourceManager.Instance.ReleaseResource(tmpClip, true);

            ObjectManager.Instance.ReleaseResource(tmpObj);
            tmpObj = null;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            //m_Audio.Stop();
            //m_Audio.clip = null;
            //ResourceManager.Instance.ReleaseResource(tmpClip, true);

            ObjectManager.Instance.ReleaseResource(tmpObj, 0, true);
            tmpObj = null;
        }

        UIManager.Instance.OnUpdate();
    }
}
