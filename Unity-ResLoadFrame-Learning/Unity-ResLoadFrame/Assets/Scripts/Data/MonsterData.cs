/****************************************************
	文件：MonsterData.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using System.Xml.Serialization;
using System.Collections.Generic;

[System.Serializable]
public class MonsterData : ExcelBase
{
    public override void Construction()
    {
        AllMonster = new List<MonsterBase>();
        for(int i = 0; i < 5; i++)
        {
            MonsterBase monster = new MonsterBase();
            monster.Id = i + 1;
            monster.Name = "gw" + i;
            monster.OutLook = "Assets/GameData/Prefabs/Attack.prefab";
            monster.Rare = i % 3;
            monster.Height = i * 1.0f;
            AllMonster.Add(monster);
        }
    }

    public override void Init()
    {
        m_AllMonsterDic.Clear();
        foreach (var monster in AllMonster)
        {
            if (m_AllMonsterDic.ContainsKey(monster.Id))
            {
                Debug.LogError("重复ID!");
            }
            else
            {
                m_AllMonsterDic.Add(monster.Id, monster);
            }
        }
    }

    public MonsterBase FindMonsterById(int id)
    {
        return m_AllMonsterDic[id];
    }

    [XmlIgnore]
    public Dictionary<int, MonsterBase> m_AllMonsterDic = new Dictionary<int, MonsterBase>();

    [XmlElement("AllMonster")]
    public List<MonsterBase> AllMonster { get; set; }
}

[System.Serializable]
public class MonsterBase
{
    [XmlAttribute("Id")]
    public int Id { get; set; }
    [XmlAttribute("Name")]
    public string Name { get; set; }
    [XmlAttribute("OutLook")]
    public string OutLook { get; set; }
    [XmlAttribute("Level")]
    public int Level { get; set; }
    [XmlAttribute("Rare")]
    public int Rare { get; set; }
    [XmlAttribute("Height")]
    public float Height { get; set; }
}
