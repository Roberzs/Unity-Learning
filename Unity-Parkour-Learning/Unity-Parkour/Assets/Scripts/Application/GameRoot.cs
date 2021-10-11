/****************************************************
    文件：GameRoot.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(FactoryManager))]
[RequireComponent(typeof(SoundManager))]
public class GameRoot : MonoSingleton<GameRoot>
{
    public FactoryManager factoryManager { get; set; }
    public SoundManager soundManager { get; set; }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        factoryManager = FactoryManager.Instance;
        soundManager = SoundManager.Instance;

        // 初始化
        MVC.RegisterController(StringDefine.E_StartUp, typeof(StartUpController));

        LoadScene(1);
    }

    void LoadScene(int level)
    {
        SceneArgs e = new SceneArgs();
        e.sceneIndex = SceneManager.GetActiveScene().buildIndex;

        SendEvent(StringDefine.E_ExitScene, e);

        SceneManager.LoadScene(level);
    }

    private void OnLevelWasLoaded(int level)
    {
        SceneArgs e = new SceneArgs();
        e.sceneIndex = level;
        SendEvent(StringDefine.E_EnterScene, e);
    }

    // 发送事件
    void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    }
}
