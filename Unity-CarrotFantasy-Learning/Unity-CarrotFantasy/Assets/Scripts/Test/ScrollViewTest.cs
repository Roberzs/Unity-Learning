/****************************************************
    文件：ScrollViewTest.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollViewTest : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ScrollRect scrollRect;
    private RectTransform contentRectTrans;

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentRectTrans = scrollRect.content;

        Debug.Log("当前content的世界坐标:" + contentRectTrans.position);

        Debug.Log("当前content的局部坐标:" + contentRectTrans.localPosition);

        // Debug.Log("当前content的右边距(过时):" + contentRectTrans.rect.right);
        Debug.Log("当前content的右边距:" + contentRectTrans.rect.xMax);
        Debug.Log("当前content的右边距:" + contentRectTrans.rect.width);

        Debug.Log("当前content的左边距:" + contentRectTrans.rect.xMin);
        Debug.Log("当前content的左边距:" + contentRectTrans.rect.x);

        Debug.Log("当前content的高度:" + contentRectTrans.rect.height);

        Debug.Log("当前content transform的 x 轴的方向:" + contentRectTrans.right);            // 就像transform.right

        Debug.Log("当前content的底部到顶部的相对高度:" + contentRectTrans.rect.y);            // 负数说明该content为从上向下延伸

        //  当前UI的宽高  sizeDelta.x sizeDelta.y 该宽为右边距的大小
        Debug.Log("当前content的宽高:" + contentRectTrans.sizeDelta);

        Debug.Log("当前content真实的宽高:" + scrollRect.GetComponent<RectTransform>().sizeDelta);

        // 改变content 的宽高 因为约束 宽度为右边距的值 所以 宽度应为想要增加的值
        contentRectTrans.sizeDelta = new Vector2(375, 300);

        // 水平滚动位置 (0-1)
        scrollRect.horizontalNormalizedPosition = 1;

        // 滑动代理事件 传递进去的值为水平与垂直滚动位置(0-1)
        scrollRect.onValueChanged.AddListener(PrintValue);
    }

    private void PrintValue(Vector2 vector2)
    {
        Debug.Log("传递进来的值:" + vector2);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("开始滑动");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("滑动中");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("结束滑动");
    }
}