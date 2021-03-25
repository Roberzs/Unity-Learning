/****************************************************
    文件：SlideScrollView.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SlideScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private float beginMousePositionX;
    private float endMousePositionX;
    
    private ScrollRect scrollRect;
    private RectTransform contentTrans;

    public int cellLength;              // 每个单元格的长度
    public int spacing;                 // 单元格之间的间隔
    public int leftOffset;              // 左偏移量
    public int totalItemNum;            // 单元格个数
    
    private float moveOneItemLength;
    private Vector3 currentContentLocalPos; // 上一次的坐标
    private Vector3 contentInitPos;

    private int currentIndex;           // 当前单元格的索引

    public Text pageText;

    public bool needSeedMessage;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentTrans = scrollRect.content;
        moveOneItemLength = cellLength + spacing;
        currentContentLocalPos = contentTrans.localPosition;
        contentInitPos = contentTrans.localPosition;
        currentIndex = 1;

        if (pageText != null) pageText.text = currentIndex.ToString() + "/" + totalItemNum;
    }
    
    public void Init()
    {
        currentIndex = 1;
        if (contentTrans != null) 
        {
            contentTrans.localPosition = contentInitPos;
            currentContentLocalPos = contentInitPos;
        }

        if (pageText != null) pageText.text = currentIndex.ToString() + "/" + totalItemNum;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginMousePositionX = Input.mousePosition.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endMousePositionX = Input.mousePosition.x;
        float offSetX = beginMousePositionX - endMousePositionX;
        float moveDistance = 0;


        if (offSetX > 0)        // 右滑
        {
            if (currentIndex >= totalItemNum) return;
            moveDistance = - moveOneItemLength;
            currentIndex++;

            if (needSeedMessage) UpdatePanel(true);
        }
        else
        {
            if (currentIndex <= 1) return;
            moveDistance = moveOneItemLength;
            currentIndex--;

            if (needSeedMessage) UpdatePanel(false);
        }

        if (pageText != null) pageText.text = currentIndex.ToString() + "/" + totalItemNum;

        DOTween.To(
                    () =>
                    contentTrans.localPosition,                // 我们想要改变的对象值
                    lerpValue                         // 经过DoTween计算得到的结果（当前值到目标值的插值）
                    => contentTrans.localPosition = lerpValue,   // 将计算的结果赋值给想要改变的对象值
                    currentContentLocalPos + new Vector3(moveDistance, 0, 0 ),             // 想要得到的对象值
                    0.5f                              // 完成动画所需的时间
                    ).SetEase(Ease.OutQuint);     // 缓动函数

        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
    }

    public void ToNextPage()
    {
        if (currentIndex >= totalItemNum) return;
        if (needSeedMessage) UpdatePanel(true);
        float moveDistance = 0;
        moveDistance = -moveOneItemLength;
        currentIndex++;

        DOTween.To(
                    () =>
                    contentTrans.localPosition,                // 我们想要改变的对象值
                    lerpValue                         // 经过DoTween计算得到的结果（当前值到目标值的插值）
                    => contentTrans.localPosition = lerpValue,   // 将计算的结果赋值给想要改变的对象值
                    currentContentLocalPos + new Vector3(moveDistance, 0, 0),             // 想要得到的对象值
                    0.5f                              // 完成动画所需的时间
                    ).SetEase(Ease.OutQuint);     // 缓动函数

        currentContentLocalPos += new Vector3(moveDistance, 0, 0);

    }

    public void ToLastPage()
    {
        if (currentIndex <= 1) return;
        if (needSeedMessage) UpdatePanel(false);
        float moveDistance = 0;
        moveDistance = moveOneItemLength;
        currentIndex--;

        DOTween.To(
                    () =>
                    contentTrans.localPosition,                // 我们想要改变的对象值
                    lerpValue                         // 经过DoTween计算得到的结果（当前值到目标值的插值）
                    => contentTrans.localPosition = lerpValue,   // 将计算的结果赋值给想要改变的对象值
                    currentContentLocalPos + new Vector3(moveDistance, 0, 0),             // 想要得到的对象值
                    0.5f                              // 完成动画所需的时间
                    ).SetEase(Ease.OutQuint);     // 缓动函数

        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
    }

    // 设置视图大小
    public void SetContentLength(int itemNum)
    {
        // 设置前 先将视图恢复默认大小
        contentTrans.sizeDelta = new Vector2(0, contentTrans.sizeDelta.y);

        contentTrans.sizeDelta = new Vector2(contentTrans.sizeDelta.x + (cellLength + spacing) * (itemNum - 1), contentTrans.sizeDelta.y);
        totalItemNum = itemNum;
    }

    // 发送翻页信息
    public void UpdatePanel(bool toNext)
    {
        if (toNext)
        {
            gameObject.SendMessageUpwards("ToNextLevel");
        }
        else
        {
            gameObject.SendMessageUpwards("ToLastLevel");
        }
    }
}