using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager
{
    //����ģʽ  
    private static LocalizationManager _instance;

    public static LocalizationManager GetInstance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LocalizationManager();
            }

            return _instance;
        }
    }

    private const string chinese = "Language/Chinese";
    private const string english = "Language/English";

    //ѡ��������Ҫ�ı�������  
    public const string language = english;


    private Dictionary<string, string> dic = new Dictionary<string, string>();
    /// <summary>  
    /// ��ȡ�����ļ������ļ���Ϣ���浽�ֵ���  
    /// </summary>  
    public LocalizationManager()
    {
        TextAsset ta = Resources.Load<TextAsset>(language);
        string text = ta.text;

        string[] lines = text.Split('\n');
        foreach (string line in lines)
        {
            if (line == null)
            {
                continue;
            }
            string[] keyAndValue = line.Split('=');
            dic.Add(keyAndValue[0], keyAndValue[1]);
        }
    }

    /// <summary>  
    /// ��ȡvalue  
    /// </summary>  
    /// <param name="key"></param>  
    /// <returns></returns>  
    public string GetValue(string key)
    {
        if (dic.ContainsKey(key) == false)
        {
            return null;
        }
        string value = null;
        dic.TryGetValue(key, out value);
        return value;
    }
}
