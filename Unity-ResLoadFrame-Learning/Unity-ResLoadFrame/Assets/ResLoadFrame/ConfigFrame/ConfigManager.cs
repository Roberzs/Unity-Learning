/****************************************************
	文件：ConfigManager.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : Singleton<ConfigManager>
{
	/// <summary>
	/// 存储所有已加载的配置表
	/// </summary>
	protected Dictionary<string, ExcelBase> m_AllExcelData = new Dictionary<string, ExcelBase>();

	private bool m_IsLoadInExcel = true;

	/// <summary>
	/// 加载配置表
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="path"></param>
	/// <returns></returns>
	public T LoadData<T>(string path) where T : ExcelBase
    {
		if (string.IsNullOrEmpty(path))
        {
			return null;
        }

		if (m_AllExcelData.ContainsKey(path))
        {
			Debug.LogError($"重复加载配置文件:{path}");
			return m_AllExcelData[path] as T;
        }

		T data = BinarySerializeOption.BinaryDeserialize<T>(path);
#if UNITY_EDITOR
		if (m_IsLoadInExcel)
		{
			var splits = path.Replace(".bytes", "").Split('/');
			var name = splits[splits.Length - 1];
			//Debug.Log(path);
			data = DataEditor.GetObjectFromExcel(name) as T;


		}
		else if (object.ReferenceEquals(data, null))
        {
			Debug.Log($"{path}不存在，将尝试从xml加载数据");
			string xmlPath = path.Replace("Binary", "Xml").Replace(".bytes", ".xml");
			data = BinarySerializeOption.XmlDeserialize<T>(xmlPath);
        }
#endif
		if (!ReferenceEquals(data, null))
        {
			data.Init();
        }
		m_AllExcelData.Add(path, data);
		return data;
    }

	public T FindData<T>(string path) where T : ExcelBase
    {
		if (string.IsNullOrEmpty(path))
        {
			return null;
        }
		ExcelBase data = null;
		if (m_AllExcelData.TryGetValue(path, out data))
        {
			return data as T;
        }
		else
        {
			data = LoadData<T>(path);
			return data as T;
        }
    }
}

