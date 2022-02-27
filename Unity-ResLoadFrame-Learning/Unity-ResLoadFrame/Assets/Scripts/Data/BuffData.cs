﻿/****************************************************
	文件：BuffData.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class BuffData : ExcelBase
{
    public override void Construction()
    {
        AllBuffList = new List<BuffBase>();
        for (int i = 0; i < 10; i++)
        {
            BuffBase tmpBuff = new BuffBase()
            {
                Id = i,
                Name = "Name" + i,
                OutLook = "Assets/GameData/Prefab" + i,
                Time = Random.Range(0.5f, 4.5f),
                BuffType = (BuffEnum)Random.Range(0, 4)
            };
            AllBuffList.Add(tmpBuff);
        }

        MonsterBuffList = new List<BuffBase>();
        for (int i = 0; i < 5; i++)
        {
            BuffBase tmpBuff = new BuffBase()
            {
                Id = i,
                Name = "Name" + i,
                OutLook = "Assets/GameData/Prefab" + i,
                Time = Random.Range(0.5f, 4.5f),
                BuffType = (BuffEnum)Random.Range(0, 4)
            };
            MonsterBuffList.Add(tmpBuff);
        }

    }

    public override void Init()
    {
        m_AllBuffDic.Clear();
        foreach (var buff in AllBuffList)
        {
            if (m_AllBuffDic.ContainsKey(buff.Id))
            {
                Debug.LogError("重复ID!");
            }
            else
            {
                m_AllBuffDic.Add(buff.Id, buff);
            }
        }
    }

    public BuffBase FindBuffById(int id)
    {
        return m_AllBuffDic[id];
    }

    [XmlIgnore]
    public Dictionary<int, BuffBase> m_AllBuffDic = new Dictionary<int, BuffBase>();

    [XmlElement("AllBuffList")]
    public List<BuffBase> AllBuffList { get; set; }
    [XmlElement("MonsterBuffList")]
    public List<BuffBase> MonsterBuffList { get; set; }
}

[System.Serializable]
public class BuffBase
{
    [XmlAttribute("Id")]
    public int Id { get; set; }
    [XmlAttribute("Name")]
    public string Name { get; set; }
    [XmlAttribute("OutLook")]
    public string OutLook { get; set; }
    [XmlAttribute("Time")]
    public float Time { get; set; }
    [XmlAttribute("BuffType")]
    public BuffEnum BuffType { get; set; }

}

public enum BuffEnum
{
    None = 0,
    Ranshao = 1,
    Bingdong = 2,
    Du = 3
}
