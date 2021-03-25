/****************************************************
    文件：DoTweenTest.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DoTweenTest : MonoBehaviour 
{
    private Image maskImage;

    private Tween maskTween;        // 存储DoTween动画

    private void Start()
    {
        maskImage = GetComponent<Image>();

        /** 静态方法 通过 DOTween.To() 改变对象值 */
        //DOTween.To(
        //    () => 
        //    maskImage.color,                // 我们想要改变的对象值
        //    toColor                         // 经过DoTween计算得到的结果（当前值到目标值的插值）
        //    => maskImage.color = toColor,   // 将计算的结果赋值给想要改变的对象值
        //    new Color(1,1,1,0),             // 想要得到的对象值
        //    2f                              // 完成动画所需的时间
        //    );

        /** DOTween 直接作用于 Transform */
        //Tween tween = transform.DOLocalMoveX(300, 0.5f);        // 向右移动300px
        //tween.PlayForward();                                    // 动画正播
        //tween.PlayBackwards();                                  // 动画倒播 (必须先正播才能倒播。不存在直接倒播的情况)

        /** 动画的循环使用 */
        maskTween = transform.DOLocalMoveX(300, 0.5f);          // 定义动画
        maskTween.SetAutoKill(false);                           // 设置动画在播放之后不立即销毁（如果不设置，在动画第一次播放之后将直接销毁）
        maskTween.Pause();                                      // 暂停动画 如果不暂停 动画将自动播放

        /** 动画的事件回调 */
        Tween tween = transform.DOLocalMoveX(300, 0.5f);
        tween.OnComplete(                           // OnComplete() 回调函数
            () =>
            {
                DOTween.To(
                    () => 
                    maskImage.color,                // 我们想要改变的对象值
                    toColor                         // 经过DoTween计算得到的结果（当前值到目标值的插值）
                    => maskImage.color = toColor,   // 将计算的结果赋值给想要改变的对象值
                    new Color(1,1,1,0),             // 想要得到的对象值
                    2f                              // 完成动画所需的时间
                    );
            }
            );

        /** 动画的缓动函数以及循环状和次数 */
        tween.SetEase(Ease.InOutBounce);            // SetEase() 缓动函数 参数为枚举类型
        tween.SetLoops(-1, LoopType.Yoyo);          // SetLoops() 循环 第一个参数是循环次数  第二个参数是循环状 枚举类型 Incremental(增量播放) Yoyo(循环播放) Restart(从头开始重复)
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //maskTween.Play();       // Tween 的Play()方法相对与倒播只能播放一次，不能倒播。
            maskTween.PlayForward();
        }
        if (Input.GetMouseButton(1))
        {
            maskTween.PlayBackwards();
        }
    }
}