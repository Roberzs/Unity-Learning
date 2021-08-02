/****************************************************
    文件：IStorage.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/21 12:3:19
    功能：Nothing
*****************************************************/

using UnityEngine;
using FrameworkDesign;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Counter
{
    public interface IStorage : IUtility
    {
        void SaveInt(string key, int value);
        int LoadInt(string key, int defaultValue = 0);
    }

    public class PlayerPrefsStorage : IStorage
    {
        public int LoadInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }
    }

    public class EditorPrefsStorage : IStorage
    {
        public int LoadInt(string key, int defaultValue = 0)
        {
#if UNITY_EDITOR
            return EditorPrefs.GetInt(key, defaultValue);
#else
            return 0;
#endif
        }

        public void SaveInt(string key, int value)
        {
#if UNITY_EDITOR
            EditorPrefs.SetInt(key, value);
#endif
        }
    }

}
