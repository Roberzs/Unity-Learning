/****************************************************
    文件：ThreadTest.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Timer.Example01
{
    public class ThreadTest : MonoBehaviour
    {
        private void Start()
        {
            ThreadStart threadStart = new ThreadStart(ThreadMain);
            Thread thread = new Thread(threadStart);
            thread.Start();
            Debug.Log("UnityMain 线程 ID:" + Thread.CurrentThread.ManagedThreadId.ToString());
        }

        void ThreadMain()
        {
            Debug.Log("New 线程 ID:" + Thread.CurrentThread.ManagedThreadId.ToString());
            // 此时将会报错 为保证数据安全 禁止访问重要数据
            Debug.Log(transform.gameObject.name);
        }
    }
}


