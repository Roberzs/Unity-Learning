/****************************************************
    文件：SoundManager.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    private AudioSource bgAudioSource, fgAudioSource;

    protected override void Awake()
    {
        base.Awake();

        bgAudioSource = gameObject.AddComponent<AudioSource>();
        fgAudioSource = gameObject.AddComponent<AudioSource>();
        bgAudioSource.playOnAwake = false;
        bgAudioSource.loop = true;

        Debug.Log("SoundManager Init Done.");
    }

    private string thisBgAudioName = "";

    // 播放背景音乐
    public void PlayBgAudio(string audioName)
    {
        if (thisBgAudioName != audioName)
        {
            thisBgAudioName = audioName;

            AudioClip audioClip = FactoryManager.Instance.GetAudioClip(audioName);
            bgAudioSource.clip = audioClip;
            bgAudioSource.Play();
        }
    }

    // 播放音效
    public void PlayEffectAudio(string audioName)
    {
        AudioClip audioClip = FactoryManager.Instance.GetAudioClip(audioName);
        fgAudioSource.PlayOneShot(audioClip);
    }
}
