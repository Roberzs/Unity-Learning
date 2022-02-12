/****************************************************
	文件：UIManager.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : Singleton<UIManager>
{
	/// <summary>
	/// UI根节点
	/// </summary>
	public RectTransform m_UIRoot;
	/// <summary>
	/// 窗体根节点
	/// </summary>
	private RectTransform m_WndRoot;
	/// <summary>
	/// UI摄像机
	/// </summary>
	private Camera m_UICamera;
	/// <summary>
	/// EventSystem节点
	/// </summary>
	private EventSystem m_EventSystem;
	/// <summary>
	/// 屏幕的宽高比
	/// </summary>
	private float m_CanvasRate = 0;
	/// <summary>
	/// 所有已打开的Panel
	/// </summary>
	private Dictionary<string, Window> m_WindowDic = new Dictionary<string, Window>();
	/// <summary>
	/// 注册的字典
	/// </summary>
	private Dictionary<string, System.Type> m_RegisterDic = new Dictionary<string, System.Type>();
	/// <summary>
	/// UIPanel根路径
	/// </summary>
	private const string UIPREFABPATH = "Assets/GameData/Prefabs/UGUI/Panel/";
	/// <summary>
	/// 已打开的Panel列表
	/// </summary>
	private List<Window> m_WindowList = new List<Window>();

	/// <summary>
	/// 初始化
	/// </summary>
	/// <param name="uiRoot">UI根节点</param>
	/// <param name="wndRoot">窗体根节点</param>
	/// <param name="uiCamera">UI摄像机</param>
	/// <param name="eventSystem">EventSystem节点</param>
	public void Init(RectTransform uiRoot, RectTransform wndRoot, Camera uiCamera, EventSystem eventSystem)
    {
		m_UIRoot = uiRoot;
		m_WndRoot = wndRoot;
		m_UICamera = uiCamera;
		m_EventSystem = eventSystem;
		m_CanvasRate = Screen.height / (m_UICamera.orthographicSize * 2);
    }

	public Window PopUpWnd(string wndName, bool bTop = false, params object[] paramList)
    {
		Window wnd = FindWndByName<Window>(wndName);
		if (wnd == null)
        {
			System.Type tp = null;
			if (m_RegisterDic.TryGetValue(wndName, out tp))
            {
				wnd = System.Activator.CreateInstance(tp) as Window;
            }
            else
            {
				Debug.LogError($"找不到窗口对应脚本, 窗口名:{wndName}");
            }

			GameObject wndObj = ObjectManager.Instance.InstantiateObject(UIPREFABPATH + wndName, false, false);
			if (!wndObj)
            {
				Debug.Log("创建Panel失败:" + wndName);
				return null;
            }
			if (!m_WindowDic.ContainsKey(wndName))
            {
				m_WindowList.Add(wnd);
				m_WindowDic.Add(wndName, wnd);
            }

			wnd.GameObject = wndObj;
			wnd.Transform = wndObj.transform;
			wnd.Name = wndName;
			wnd.Awake(paramList);
			wndObj.transform.SetParent(m_WndRoot, false);

			if (bTop)
            {
				wndObj.transform.SetAsLastSibling();
            }

			wnd.OnShow(paramList);

        }
		else
        {
			ShowWnd(wndName, bTop, paramList);
		}
		return wnd;
    }

	/// <summary>
	/// 根据Panel名称显示Panel
	/// </summary>
	/// <param name="name"></param>
	/// <param name="paramList"></param>
	public void ShowWnd(string name, bool bTop = true, params object[] paramList)
    {
		Window wnd = FindWndByName<Window>(name);
		ShowWnd(wnd, bTop, paramList);
    }

    public void ShowWnd(Window wnd, bool bTop = true, params object[] paramList)
    {
		if (wnd != null)
        {
			if (wnd.GameObject != null && !wnd.GameObject.activeSelf)
				wnd.GameObject.SetActive(true);
			if (bTop)
				wnd.Transform.SetAsLastSibling();
			wnd.OnShow(paramList);
        }
    }

	public void CloseWnd(string name, bool destory = false)
    {
        Window wnd = FindWndByName<Window>(name);
        CloseWnd(wnd, destory);
    }

    public void CloseWnd(Window wnd, bool destory = false)
    {
        if (wnd != null)
        {
			wnd.OnDisable();
			wnd.OnClose();
			if (m_WindowDic.ContainsKey(wnd.Name))
			{
				m_WindowList.Remove(wnd);
				m_WindowDic.Remove(wnd.Name);
            }
			if (destory)
            {
				ObjectManager.Instance.ReleaseResource(wnd.GameObject, 0, true);
            }
			else
            {
				ObjectManager.Instance.ReleaseResource(wnd.GameObject, recycleParent: false);
			}
			wnd.GameObject = null;
			wnd = null;
        }
    }

    /// <summary>
    /// 根据Panel名称查找Panel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T FindWndByName<T>(string name) where T : Window
    {
		Window wnd = null;
		if (m_WindowDic.TryGetValue(name, out wnd))
        {
			return (T)wnd;
        }
		return null;
    }

	/// <summary>
	/// Panel 的注册方法
	/// </summary>
	/// <typeparam name="T">窗口泛型类</typeparam>
	/// <param name="name">窗口名称</param>
	public void Register<T>(string name) where T : Window
    {
		m_RegisterDic[name] = typeof(T);
    }

	public void CloseAllWnd()
    {
        for (int i = m_WindowList.Count - 1; i >= 0; i++)
        {
			CloseWnd(m_WindowList[i]);
        }
    }

	/// <summary>
	/// 切换到某个唯一窗口
	/// </summary>
	/// <param name="name"></param>
	/// <param name="bTop"></param>
	/// <param name="paramList"></param>
	public void SwitchStateByName(string name, bool bTop = true, params object[] paramList)
    {
		CloseAllWnd();
		PopUpWnd(name, bTop, paramList);
    }

	public void HideWnd(string name)
    {
		Window wnd = FindWndByName<Window>(name);
		HideWnd(wnd);
    }

	public void HideWnd(Window wnd)
    {
		if (wnd != null)
        {
			wnd.GameObject.SetActive(false);
			wnd.OnDisable();
        }
    }

	/// <summary>
	/// 显示或隐藏所有UI
	/// </summary>
	public void ShowOrHideUI(bool show)
    {
		if (m_UIRoot != null)
        {
			m_UIRoot.gameObject.SetActive(show);
        }
    }

	/// <summary>
	/// 设置默认选择对象
	/// </summary>
	/// <param name="obj"></param>
	public void SetNormalSelectObj(GameObject obj)
    {
		if (m_EventSystem == null)
        {
			m_EventSystem = EventSystem.current;
        }
		m_EventSystem.firstSelectedGameObject = obj;
    }

	public void OnUpdate()
    {
        foreach (var item in m_WindowList)
        {
			if (item != null)
            {
				item.OnUpdate();
            }
        }
    }

	/// <summary>
	/// 发送消息给Panel
	/// </summary>
	/// <param name="name">窗口名称</param>
	/// <param name="msgID">消息类型</param>
	/// <param name="paramList">参数数组</param>
	/// <returns></returns>
	public bool SeedMessageToWnd(string name, UIMsgID msgID = 0, params object[] paramList)
    {
		Window wnd = FindWndByName<Window>(name);
		if (wnd != null)
        {
			return wnd.OnMessage(msgID, paramList);
        }
		return false;
    }
}

public enum UIMsgID
{
	Null,
}