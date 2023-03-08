/****************************************************
	文件：Window.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Window
{
    /// <summary>
    /// GameObject引用
    /// </summary>
    public GameObject GameObject { get; set; }
    /// <summary>
    /// Transform引用
    /// </summary>
    public Transform Transform { get; set; }
    /// <summary>
    /// Panel 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 所有的Button
    /// </summary>
    protected List<Button> m_AllButton = new List<Button>();
    /// <summary>
    /// 所有的Toggle
    /// </summary>
    protected List<Toggle> m_AllToggle = new List<Toggle>();


    public virtual void Awake(params object[] paramList)
    {

    }

    public virtual void OnShow(params object[] paramList)
    {

    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnDisable()
    {

    }

    public virtual bool OnMessage(UIMsgID msgID, params object[] paramList)
    {
        return true;
    }

    public virtual void OnMessage(string msgName)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnMessage<T>(string msgName, T arg)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnMessage<T, X>(string msgName, T arg1, X arg2)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnMessage<T, X, Y>(string msgName, T arg1, X arg2, Y arg3)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnMessage<T, X, Y, Z>(string msgName, T arg1, X arg2, Y arg3, Z arg4)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnMessage<T, X, Y, Z, W>(string msgName, T arg1, X arg2, Y arg3, Z arg4, W arg5)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnClose()
    {
        RemoveAllButtonListener();
        RemoveAllToggleListener();
        m_AllButton.Clear();
        m_AllToggle.Clear();
    }

    /// <summary>
    /// 移除所有Button事件
    /// </summary>
    public void RemoveAllButtonListener()
    {
        foreach (var btn in m_AllButton)
        {
            btn.onClick.RemoveAllListeners();
        }
    }

    /// <summary>
    /// 移除所有Toggle事件
    /// </summary>
    public void RemoveAllToggleListener()
    {
        foreach (var toggle in m_AllToggle)
        {
            toggle.onValueChanged.RemoveAllListeners();
        }
    }

    /// <summary>
    /// 添加Button监听事件
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="action"></param>
    public void AddButtonClickListener(Button btn, UnityAction action)
    {
        if (btn)
        {
            if (!m_AllButton.Contains(btn))
            {
                m_AllButton.Add(btn);
            }
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(action);
            btn.onClick.AddListener(PlayBtnSound);
        }
    }

    public void AddToggleClickListener(Toggle toggle, UnityAction<bool> action)
    {
        if (toggle)
        {
            if (!m_AllToggle.Contains(toggle))
            {
                m_AllToggle.Add(toggle);
            }
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(action);
            toggle.onValueChanged.AddListener(PlayToggleSound);
        }
    }

    private void PlayBtnSound()
    {

    }

    private void PlayToggleSound(bool isOn)
    {

    }

    public bool ChangeImageSprite(string path, Image img, bool setNativeSize = false)
    {
        if (!img)
            return true;

        Sprite sp = ResourceManager.Instance.LoadResource<Sprite>(path);
        if (!sp)
        {
            if (img.sprite != null)
                img.sprite = null;

            img.sprite = sp;

            if (setNativeSize)
                img.SetNativeSize();

            return true;
        }
        return false;
    }

    public void ChangeImageSpriteAsync(string path, Image img, bool setNativeSize = false)
    {
        if (!img)
            return;

        ResourceManager.Instance.AsyncLoadResource(path, (string path_TMP, Object obj, object param1, object parma2,
            object param3) =>
        {
            if (obj != null)
            {
                Sprite sp = obj as Sprite;
                Image img_TMP = param1 as Image;
                bool setNativeSize_TMP = (bool)parma2;

                if (img_TMP.sprite != null)
                    img_TMP.sprite = null;

                img_TMP.sprite = sp;

                if (setNativeSize_TMP)
                    img_TMP.SetNativeSize();

            }
        }, LoadResPriority.RES_MIDDLE, true, img, setNativeSize);
    }
}

