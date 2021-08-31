/****************************************************
    文件：LifeCycleTest.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：测试游戏的生命周期
*****************************************************/

using UnityEngine;

namespace Timer.Example01
{
    public class LifeCycleTest : MonoBehaviour
    {
        private void Awake()
        {
            // 只要物体激活 就会调用Awake 只会调用一次
            Debug.Log("Awake");
        }

        private void OnEnable()
        {
            // 只要物体激活就会调用
            Debug.Log("OnEnable");
        }

        private void Start()
        {
            // 只会调用一次 Tip: 假设这是总管理类脚本 建议在此处初始化各类服务 这样可以有效的控制脚本的初始化顺序
            Debug.Log("Start");
        }

        private void FixedUpdate()
        {
            // 由 Fixed TimeStep 决定调用频率 常用于刚体运算 例如刚体移动
        }

        private void Update()
        {
            
        }

        private void OnDisable()
        {
            // 只要物体失活就会调用
            Debug.Log("OnDisable");
        }
    }
}


