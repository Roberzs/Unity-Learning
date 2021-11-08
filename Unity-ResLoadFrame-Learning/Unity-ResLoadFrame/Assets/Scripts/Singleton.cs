/****************************************************
    文件：Singleton.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

public class Singleton<T> where T: new()
{
    private static readonly object lockObj = new object();

    private static T m_Instance;
    public static T Instance
    {
        get
        {
            lock (lockObj)
            {
                if (m_Instance== null)
                {
                    m_Instance = new T();
                }
                return m_Instance ;
            }
            
        }
    }
}
