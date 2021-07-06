/****************************************************
    文件：AudioSourceManager.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：声音管理
*****************************************************/

using UnityEngine;

public class AudioSourceManager
{
    private AudioSource[] audioSource;      // 0 - 音乐  1 - 音效
    // 音乐与音效开关
    private bool playBGMusic = true;
    private bool playEffectMusic = true;

    public AudioSourceManager()
    {
        audioSource = GameManager.Instance.GetComponents<AudioSource>();
    }

    // 播放音乐
    public void PlayBGMusic(AudioClip audioClip)
    {
        // 当音乐不处于播放状态或音乐已切换时执行
        if (!audioSource[0].isPlaying || audioSource[0].clip != audioClip)
        {
            audioSource[0].clip = audioClip;
            audioSource[0].Play();
        }
    }

    // 播放音效
    public void PlayEffectMusic(AudioClip audioClip)
    {
        if (playEffectMusic)
        {
            audioSource[1].PlayOneShot(audioClip);
        }
    }

    

    /** 声音的打开或关闭 */

    public void CloseOrOpenEffectMusic()
    {
        playEffectMusic = !playEffectMusic;
    }

    public void CloseOrOpenBGMusic()
    {
        playBGMusic = !playBGMusic;
        if (playBGMusic)
        {
            OpenBGMusic();
        }
        else
        {
            CloseBGMusic();
        }
    }

    // 打开和关闭音乐
    public void OpenBGMusic()
    {
        audioSource[0].Play();
    }

    public void CloseBGMusic()
    {
        audioSource[0].Stop();
    }

    public void PlayButtonAudioClip()
    {
        PlayEffectMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("Main/Button"));
    }

    public void PlayPagingAudioClip()
    {
        PlayEffectMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("Main/Paging"));
    }
}