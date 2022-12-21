using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IrregularShapeClick : MonoBehaviour
{
    void Start()
    {
        // 图片被点击位置透明度小于0.1f时，屏蔽点击事件
        // 不推荐使用 使用时必须打开图片Read/Write选项，会增加性能消耗以及图片将不能打包进图集
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
