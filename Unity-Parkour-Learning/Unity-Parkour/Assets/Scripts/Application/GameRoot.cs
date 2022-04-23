/****************************************************
    文件：GameRoot.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：游戏入口
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(FactoryManager))]
[RequireComponent(typeof(SoundManager))]
public class GameRoot : MonoSingleton<GameRoot>
{
    public FactoryManager factoryManager { get; private set; }
    public SoundManager soundManager { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        // Init
        factoryManager = FactoryManager.Instance;
        soundManager = SoundManager.Instance;
    }

    private void Start()
    {
        
        // 添加场景加载回调
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 注册启动控制器
        MVC.RegisterController(StringDefine.E_StartUp, typeof(StartUpController));

        // 跳转
        GameRoot.Instance.LoadScene(2);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        // 删除场景加载回调
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// 加载新场景, 发送当前场景退出事件
    /// </summary>
    /// <param name="level">场景索引</param>

    public void LoadScene(int level)
    {
        /// 退出当前场景
        SceneArgs e = new SceneArgs();
        e.sceneIndex = SceneManager.GetActiveScene().buildIndex;

        SendEvent(StringDefine.E_ExitScene, e);

        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }

    /// <summary>
    /// 场景加载完成回调
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadSceneMode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneArgs e = new SceneArgs();
        e.sceneIndex = scene.buildIndex;
        SendEvent(StringDefine.E_EnterScene, e);
    }

    // 发送事件
    void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    }
}
