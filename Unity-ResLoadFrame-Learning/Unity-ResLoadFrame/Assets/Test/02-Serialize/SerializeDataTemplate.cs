using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

#if UNITY_EDITOR
namespace ResLoadFrame.Test
{
    [System.Serializable]
    public class SerializeDataTemplate
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlElement("Score")]
        public ScoreTemplate Score { get; set; }

        [XmlArray("Hobby")] // [XmlElement("Score")]
        public List<string> Hobby { get; set; }
    }

    [System.Serializable]
    public class ScoreTemplate
    {
        [XmlAttribute("Mathematics")]
        public int Mathematics { get; set; }

        [XmlAttribute("English")]
        public int English { get; set; }
    }
}
#endif