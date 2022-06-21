/****************************************************
	文件：ClickUI.cs
	作者：Zhangying
	邮箱：zhy18125@gmail.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickUI : MonoBehaviour, IPointerClickHandler
{
    private int _index;

    private void Start()
    {
        _index = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetColor();
        ExecuteAllClickEvent(eventData);
    }

	private void SetColor()
    {
        if (_index == 0)
        {
            GetComponent<Image>().color = Color.red;
        }
        else
        {
            GetComponent<Image>().color = Color.green;
        }
        _index = _index == 0 ? 1 : 0;
        
    }
    
    /// <summary>
    /// 当物体接收到点击事件时，调用执行其他所有物体点击事件
    /// </summary>
    /// <param name="eventData"></param>
    private void ExecuteAllClickEvent(PointerEventData eventData)
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        foreach (var item in raycastResults)
        {
            if (item.gameObject != gameObject)
            {
                ExecuteEvents.Execute(item.gameObject, eventData, ExecuteEvents.pointerClickHandler);
            }
        }
    }
}
