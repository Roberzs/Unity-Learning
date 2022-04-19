/****************************************************
    文件：MonoSingleton.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/29 15:54:35
    功能：Nothing
*****************************************************/

using UnityEngine;


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static readonly object syslock = new object();
    private static T mInstance = null;

    public static T Instance
    {
        get
        {
            if (mInstance == null)
            {
                lock (syslock)
                {
                    if (mInstance == null)
                    {
                        T[] ts = FindObjectsOfType<T>();
                        if (ts.Length == 0)
                        {
                            string instanceName = typeof(T).Name;
                            Debug.Log("New Instance Name: " + instanceName);

                            GameObject instanceGo = GameObject.Find(instanceName);

                            if (instanceGo == null)
                                instanceGo = new GameObject(instanceName);

                            mInstance = instanceGo.AddComponent<T>();

                        }
                        else if (ts.Length == 1)
                        {
                            mInstance = ts[0];
                            Debug.Log("Already exist: " + mInstance.name);
                        }
                        else
                        {
                            Debug.LogError("More than 1!");

                            return null;
                        }

                        // Set gameObject will DontDestroyOnLoad
                        DontDestroyOnLoad(mInstance.gameObject);
                    }
                }
            }
                
            return mInstance;
        }
    }

    protected virtual void Awake()
    {
        
    }

    protected virtual void OnDestroy()
    {
        mInstance = null;
    }
}


