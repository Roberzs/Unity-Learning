/****************************************************
    文件：SlideCanCoverSrollView.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SlideCanCoverSrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private float contentLength;        // 容器扩展长度
    private float beginMousePositionX;  
    private float endMousePositionX;
    private ScrollRect scrollRect;
    private float lastProportion;       // 上一个位置的比例

    public int cellLength;              // 每个单元格的长度
    public int spacing;                 // 单元格之间的间隔
    public int leftOffset;              // 左偏移量
    public int totalItemNum;            // 单元格个数

    private float upperLimit;           // 上限值
    private float lowerLimit;           // 下限值
    private float firstItemLength;      // 移动第一个单元格的距离
    private float oneItemLength;        // 移动一个单元格的距离
    private float oneItemProportion;    // 滑动一个单元格所占的比例

    private int currentIndex;           // 当前单元格的索引

    public Text pageText;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentLength = scrollRect.content.rect.xMax;
        firstItemLength = cellLength / 2 + leftOffset;
        oneItemLength = cellLength + spacing;
        oneItemProportion = oneItemLength / contentLength;
        upperLimit = 1 - firstItemLength / contentLength;
        lowerLimit = firstItemLength / contentLength;
        currentIndex = 1;
        scrollRect.horizontalNormalizedPosition = 0;

        if (pageText != null) pageText.text = currentIndex.ToString() + "/" + totalItemNum;
    }

    public void Init()
    {
        currentIndex = 1;
        scrollRect.horizontalNormalizedPosition = 0;
        lastProportion = 0;
        pageText.text = currentIndex.ToString() + "/" + totalItemNum;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginMousePositionX = Input.mousePosition.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endMousePositionX = Input.mousePosition.x;
        float offSetX = (beginMousePositionX - endMousePositionX) * 1;

        if (Mathf.Abs(offSetX) > firstItemLength)
        {
            if (offSetX > 0)        // 右滑
            {
                if (currentIndex >= totalItemNum) return;       // 如果已到达最右端  直接return

                int moveCount = (int)((offSetX - firstItemLength) / oneItemLength) + 1;     // 当前需要移动的单元格数量
                currentIndex += moveCount;
                
                if (currentIndex >= totalItemNum) currentIndex = totalItemNum;      // 如果移动数量大于单元格总数量 则移动到最右端 索引设置为最大值

                lastProportion += oneItemProportion * moveCount;
                if (lastProportion >= upperLimit) lastProportion = 1;
            }
            else
            {
                if (currentIndex <= 1) return;       // 如果已到达最左端  直接return

                int moveCount = (int)((offSetX + firstItemLength) / oneItemLength) - 1;     // 当前需要移动的单元格数量
                currentIndex += moveCount;

                if (currentIndex <= 1) currentIndex = 1;      // 如果移动数量小于1 则移动到最左端 索引设置为1

                lastProportion += oneItemProportion * moveCount;
                if (lastProportion <= lowerLimit) lastProportion = 0;
            }

            if (pageText != null) pageText.text = currentIndex.ToString() + "/" + totalItemNum;
        }
        
        DOTween.To(
                    () =>
                    scrollRect.horizontalNormalizedPosition,                // 我们想要改变的对象值
                    lerpValue                         // 经过DoTween计算得到的结果（当前值到目标值的插值）
                    => scrollRect.horizontalNormalizedPosition = lerpValue,   // 将计算的结果赋值给想要改变的对象值
                    lastProportion,             // 想要得到的对象值
                    0.5f                              // 完成动画所需的时间
                    ).SetEase(Ease.OutQuint);     // 缓动函数
    }
}