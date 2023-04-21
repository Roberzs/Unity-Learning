/****************************************************
	文件：BaseItem.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseItem : MonoBehaviour
{
    /// <summary>
    /// 添加Button监听事件
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="action"></param>
    public void AddButtonClickListener(Button btn, UnityAction action)
    {
        if (btn)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(action);
            btn.onClick.AddListener(PlayBtnSound);
        }
    }

    private void PlayBtnSound()
    {

    }
}

