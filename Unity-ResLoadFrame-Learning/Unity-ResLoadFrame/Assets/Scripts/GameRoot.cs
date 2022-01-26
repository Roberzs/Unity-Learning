using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameRoot : MonoBehaviour
{
    private AudioSource m_Audio;

    private AudioClip tmpClip;

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

        ResourceManager.Instance.PreloadRes("Assets/GameData/Sounds/senlin.mp3");
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            float time = Time.realtimeSinceStartup;
            tmpClip = ResourceManager.Instance.LoadResource<AudioClip>("Assets/GameData/Sounds/senlin.mp3");
            Debug.Log($"资源加载时长:{Time.realtimeSinceStartup - time}");
            m_Audio.clip = tmpClip;
            m_Audio.Play();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            m_Audio.Stop();
            m_Audio.clip = null;
            ResourceManager.Instance.ReleaseResource(tmpClip, true);
        }
    }
}
