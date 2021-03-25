/****************************************************
    文件：SingletonTemplate.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：单例模板以及测试
*****************************************************/

using UnityEngine;

public class SingletonTest: SingletonTemplate<SingletonTest>
{
    private void Start()
    {
        // 测试
        SingletonTest.Instance.PrintLog();
    }
}

/** 单例模板 */
public abstract class SingletonTemplate<T> : MonoBehaviour 
    where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance { get => _instance; }

    private void Awake()
    {
        _instance = this as T;
    }

    public void PrintLog()
    {
        Debug.Log("单例模板测试");
    }
}