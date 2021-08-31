/****************************************************
    文件：CoroutineTest.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：协程的测试
*****************************************************/

using UnityEngine;
using System.Collections;

namespace Timer.Example02
{
    public class CoroutineTest : MonoBehaviour
    {
        Coroutine coroutine = null;

        private void Start()
        {
            /** 协程同样是主线程驱动 会在每一帧去检测条件是否满足 */

            coroutine = StartCoroutine(FuncA());        // 1 - StartCoroutine(IEnumerator routine) 常用启动协程的方式
            //StartCoroutine("FuncA");                  // 2 - StartCoroutine(string routine[, object value])方式启动协程 可以传递一个object参数 但性能开销略大 
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopCoroutine(coroutine);               // 1 - 终止协程(常用)
                //StopCoroutine("FuncA");               // 2 - 只能终止用字符串启动的协程
                //StopAllCoroutines();                  // 3 - 终止所有协程
                                                        // 4 - 将挂载物体失活 不建议!!! 容易引发BUG 
            }
        }

        IEnumerator FuncA()
        {
            Debug.Log("FuncA" + 1);
            yield return new WaitForSeconds(2f);
            Debug.Log("FuncA" + 2);
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(FuncB());       // 将会在完成嵌套的协程之后才会继续执行下面代码
            Debug.Log("FuncA" + 3);
            
        }

        IEnumerator FuncB()
        {
            Debug.Log("FuncB" + 1);
            yield return new WaitForSeconds(2f);
            Debug.Log("FuncB" + 2);
        }

        // 异步加载方式加载资源
        IEnumerator AsyncGetRes()
        {
            ResourceRequest request = Resources.LoadAsync("AsyncTest");
            yield return request;
            Debug.Log("Load Done.");
        }
    }
}

