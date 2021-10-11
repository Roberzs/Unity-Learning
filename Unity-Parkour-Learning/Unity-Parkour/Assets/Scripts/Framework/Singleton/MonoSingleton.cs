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
    protected static T mInstance = null;

    public static T Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<T>();

                if (FindObjectsOfType<T>().Length > 1)
                {
                    Debug.LogError("More than 1!");
                    return mInstance;
                }

                if (mInstance == null)
                {
                    string instanceName = typeof(T).Name;
                    Debug.Log("Instance Name: " + instanceName);
                    GameObject instanceGo = GameObject.Find(instanceName);

                    if (instanceGo == null)
                        instanceGo = new GameObject(instanceName);

                    mInstance = instanceGo.AddComponent<T>();
                    DontDestroyOnLoad(instanceGo);
                    Debug.Log("Add New Singleton " + mInstance.name + " in Game!");
                }
                else
                {
                    Debug.Log("Already exist: " + mInstance.name);
                }
            }
            return mInstance;
        }
    }

    protected virtual void OnDestroy()
    {
        mInstance = null;
    }
}


