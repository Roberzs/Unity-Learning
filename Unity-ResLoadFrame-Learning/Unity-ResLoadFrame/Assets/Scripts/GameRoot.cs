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
        m_Audio = GetComponent<AudioSource>();
        AssetBundleManager.Instance.LoadAssetBundleConfig();
    }

    private void Start()
    {
        tmpClip = ResourceManager.Instance.LoadResource<AudioClip>("Assets/GameData/Sounds/senlin.mp3");

        m_Audio.clip = tmpClip;
        m_Audio.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            m_Audio.Stop();
            m_Audio.clip = null;
            ResourceManager.Instance.ReleaseResource(tmpClip, true);
        }
    }
}
