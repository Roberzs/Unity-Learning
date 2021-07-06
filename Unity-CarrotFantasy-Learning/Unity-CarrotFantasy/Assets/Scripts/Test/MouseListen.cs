/****************************************************
    文件：MouseListen.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.EventSystems;

public class MouseListen : MonoBehaviour , IPointerDownHandler
{
    // 该接口事件只对UI有效
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown事件");
    }

    // OnMouse类型事件只能作用于鼠标左键事件
    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown事件");
    }
}