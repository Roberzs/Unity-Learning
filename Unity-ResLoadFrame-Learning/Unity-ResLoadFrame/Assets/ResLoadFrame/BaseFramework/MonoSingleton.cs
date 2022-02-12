/****************************************************
	文件：MonoSingleton.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	protected static T _instance;

	public static T Instance
    {
        get { return _instance; }
    }

	protected virtual void Awake()
    {
		if (_instance == null)
        {
			_instance = (T)this;
        }
		else
        {
			Debug.LogError($"Get a second instance of this class:{this.GetType()}");
        }
    }
}

