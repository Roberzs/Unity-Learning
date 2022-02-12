/****************************************************
	文件：GameSceneManager.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager>
{
	/// <summary>
	/// 场景加载开始回调
	/// </summary>
	public Action LoadSceneEnterCallBack;
	/// <summary>
	/// 场景加载结束回调
	/// </summary>
	public Action LoadSceneOverCallBack;

	/// <summary>
	/// 当前场景名
	/// </summary>
    public string CurrentMapName { get; set; }
	public static float LoadingProgress = 0;
	/// <summary>
	/// 场景加载是否完成
	/// </summary>
	public bool AlreadyLoadScene { get; set; }
	private MonoBehaviour m_Mono;

	public void Init(MonoBehaviour mono)
    {
		m_Mono = mono;
    }

	public void LoadSceen(string name, Action loadSceneEnterCallBack = null, Action loadSceneOverCallBack = null)
    {
		LoadingProgress = 0;
		LoadSceneEnterCallBack = loadSceneEnterCallBack;
		LoadSceneOverCallBack = loadSceneOverCallBack;
		m_Mono.StartCoroutine(LoadSceneAsync(name));
    }

	IEnumerator LoadSceneAsync(string name)
    {
		if (LoadSceneEnterCallBack != null)
        {
			LoadSceneEnterCallBack();
        }
		ClearCache();
		AlreadyLoadScene = false;
		AsyncOperation unLoadScene = SceneManager.LoadSceneAsync("EmptyScene", LoadSceneMode.Single);
		while(unLoadScene != null && !unLoadScene.isDone)
        {
			yield return new WaitForEndOfFrame();
        }

		LoadingProgress = 0;
		int targetProgress = 0;
		AsyncOperation asyncScene = SceneManager.LoadSceneAsync(name);
		if (asyncScene != null && !asyncScene.isDone)
        {
			asyncScene.allowSceneActivation = false;
			while(asyncScene.progress < 0.9f)
            {
				targetProgress = (int)asyncScene.progress * 100;
				yield return new WaitForEndOfFrame();

				// 平滑过度
				while (LoadingProgress < targetProgress)
                {
					++LoadingProgress;
					yield return new WaitForEndOfFrame();
                }
            }
			CurrentMapName = name;
			SetSceneSetting(name);
			targetProgress = 100;
			while (LoadingProgress < targetProgress - 2)
            {
				++LoadingProgress;
				yield return new WaitForEndOfFrame();
            }
			LoadingProgress = 100;
			asyncScene.allowSceneActivation = true;
			AlreadyLoadScene = true;
            if (LoadSceneOverCallBack != null)
            {
				LoadSceneOverCallBack();
            }
        }
		yield return null;
    }

	/// <summary>
	/// 跳转场景清除缓存
	/// </summary>
	private void ClearCache()
    {
		ObjectManager.Instance.ClearCache();
		ResourceManager.Instance.ClearCache();
    }

	private void SetSceneSetting(string name)
    {
		/// 设置场景环境
    }
}

