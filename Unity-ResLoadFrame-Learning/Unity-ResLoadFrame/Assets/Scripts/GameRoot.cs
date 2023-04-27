using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameRoot : MonoSingleton<GameRoot>
{
    private AudioSource m_Audio;

    private AudioClip tmpClip;

    private GameObject tmpObj;

    private Stack<GameObject> pool = new Stack<GameObject>();


    protected override void Awake()
    {
        base.Awake();

        Debug.Log(Application.persistentDataPath);

        DontDestroyOnLoad(gameObject);
        
        m_Audio = GetComponent<AudioSource>();
        
        ResourceManager.Instance.Init(this);
        ObjectManager.Instance.Init(transform.Find("RecyclePoolTrs"), transform.Find("SceneTrs"));
        HotFixManager.Instance.Init(this);
        UIManager.Instance.Init(transform.Find("UIRoot") as RectTransform,
            transform.Find("UIRoot/WndRoot") as RectTransform,
            transform.Find("UIRoot/UICamera").GetComponent<Camera>(),
            transform.Find("UIRoot/EventSystem").GetComponent<EventSystem>());
        RegisterUI();
    }

    private void Start()
    {

        //AssetBundleManager.Instance.LoadAssetBundleConfig();
        //LoadConfiger();

        //GameSceneManager.Instance.Init(this);

        UIManager.Instance.PopUpWnd("HotFixPanel.prefab", bResourceLoad: true);

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
        //    tmpObj.SetActive(false);
        //}, LoadResPriority.RES_HIGHT, true);

        //float timer = Time.realtimeSinceStartup;
        //ObjectManager.Instance.PreloadGameObject("Assets/GameData/Prefabs/Attack.prefab", LoadResPriority.RES_MIDDLE, 10);
        //ObjectManager.Instance.PreloadGameObject("Assets/GameData/Prefabs/Attack.prefab", LoadResPriority.RES_MIDDLE, 100);
        //ObjectManager.Instance.PreloadGameObject("Assets/GameData/Prefabs/Attack.prefab", LoadResPriority.RES_MIDDLE, 100);
        //Debug.Log($"加载所需时间:{Time.realtimeSinceStartup - timer}");



        // UI加载


        //ObjectManager.Instance.PreloadGameObject(new PreloadValue("Assets/GameData/Prefabs/Attack.prefab", 10000));



        //UIManager.Instance.PopUpWnd("MenuPanel.prefab");

        //ResourceManager.Instance.LoadResource<AudioClip>("Assets/GameData/Sounds/senlin.mp3");


        ////ObjectManager.Instance.ReleaseResource(tmpObj);

        ////UIManager.Instance.PopUpWnd("MenuPanel.prefab");

        //GameSceneManager.Instance.LoadSceen("MenuScene",
        //() =>
        //    {
        //        UIManager.Instance.PopUpWnd("LoadingPanel.prefab", true);
        //    },
        //    () =>
        //    {
        //        //UIManager.Instance.OnUpdate
        //        UIManager.Instance.CloseWnd("LoadingPanel.prefab");
        //        UIManager.Instance.PopUpWnd("MenuPanel.prefab");
        //    });
    }

    public IEnumerator StartGame(Image image, Text text)
    {
        AssetBundleManager.Instance.LoadAssetBundleConfig();
        yield return 0;
        LoadConfiger();
        yield return 0;
        
        yield return 0;
        GameSceneManager.Instance.Init(this);
        yield return 0;
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
        UIManager.Instance.Register<HotFixUI>("HotFixPanel.prefab");
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

            var tmpObj = ObjectManager.Instance.InstantiateObject("Assets/GameData/Prefabs/Attack.prefab", true);
            pool.Push(tmpObj);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //m_Audio.Stop();
            //m_Audio.clip = null;
            //ResourceManager.Instance.ReleaseResource(tmpClip, true);
            if (pool.Count > 0)
            {
                var item = pool.Pop();

                ObjectManager.Instance.ReleaseResource(item);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            //m_Audio.Stop();
            //m_Audio.clip = null;
            //ResourceManager.Instance.ReleaseResource(tmpClip, true);

            ObjectManager.Instance.PreloadGameObject(new PreloadValue("Assets/GameData/Prefabs/Attack.prefab", 10));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //m_Audio.Stop();
            //m_Audio.clip = null;
            //ResourceManager.Instance.ReleaseResource(tmpClip, true);

            ObjectManager.Instance.ReleaseResource(tmpObj);
            tmpObj = null;
        }

        UIManager.Instance.OnUpdate();
    }

    public static void OpenCommonConfirm(string title, string content, UnityAction confirmAction, UnityAction cancleAction)
    {
        var item = Instantiate(Resources.Load<GameObject>("CommonConfirm"));
        item.transform.SetParent(UIManager.Instance.WndRoot, false);
        var script = item.GetComponent<CommonConfirm>();
        script.OnShow(title, content, confirmAction, cancleAction);
    }
}
