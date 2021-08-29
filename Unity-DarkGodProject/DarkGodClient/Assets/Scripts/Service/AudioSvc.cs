/****************************************************
    文件：AudioSys.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/11/23 23:25:56
	功能：Nothing
*****************************************************/

using UnityEngine;

public class AudioSvc : MonoBehaviour 
{
    public static AudioSvc Instance = null;
    public AudioSource bgAudio, uiAudio;

    public void InitSvc()
    {
        Instance = this;
    }

    public void PlayBGMusic (string name, bool isLoop = true)
    {
        
        AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        if (bgAudio.clip == null || bgAudio.clip.name != audio.name)
        {
            PECommon.Log("播放背景音乐");
            bgAudio.clip = audio;
            bgAudio.loop = isLoop;
            bgAudio.Play();
        }
    }

    public void PlayUIAudio(string name)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        uiAudio.clip = audio;
        uiAudio.Play();
    }
}