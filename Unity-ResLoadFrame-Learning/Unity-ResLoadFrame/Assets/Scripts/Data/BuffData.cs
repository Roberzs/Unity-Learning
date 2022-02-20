/****************************************************
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
        base.Construction();
    }

    public override void Init()
    {
        base.Init();
    }

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

