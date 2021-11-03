using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class XMLSerializeTest : MonoBehaviour
{
    private void Start()
    {
        // XML
        //XMLSerializeData();
        //XMLDeSerializeData();

        // Binary
        //BinarySerializeData();
        BinaryDeSerializeData();
    }

    private void XMLSerializeData()
    {
        // 新建数据
        SerializeDataTemplate data = new SerializeDataTemplate();
        data.Id = 1;
        data.Name = "Tom";
        data.Score = new ScoreTemplate();
        data.Score.English = 98;
        data.Score.Mathematics = 86;
        data.Hobby = new List<string>();
        data.Hobby.Add("Exercise");
        data.Hobby.Add("Study");

        // 创建文件流 将数据转成XML格式写入本地
        FileStream fileStream = new FileStream(Application.dataPath + "/testXML.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xml = new XmlSerializer(data.GetType());
        xml.Serialize(sw, data);
        sw.Close();
        fileStream.Close();
        Debug.Log("XMLSerializeData Done.");
    }

    private void XMLDeSerializeData()
    {
        FileStream fileStream = new FileStream(Application.dataPath + "/testXML.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
        XmlSerializer xml = new XmlSerializer(typeof(SerializeDataTemplate));
        SerializeDataTemplate data = xml.Deserialize(fileStream) as SerializeDataTemplate;
        fileStream.Close();

        Debug.Log("XMLDeSerializeData Done.");
        Debug.Log("------------------------");
        Debug.Log("Id:" + data.Id);
        Debug.Log("Name:" + data.Name);
        Debug.Log("Score:");
        string score = "--Mathematics:" + data.Score.Mathematics + " English:" + data.Score.English;
        Debug.Log(score);
        Debug.Log("Hobby:");
        string hobby = "--";
        foreach (var item in data.Hobby) hobby = hobby + "[" + item + "]";
        Debug.Log(hobby);
    }

    private void BinarySerializeData()
    {
        // 新建数据
        SerializeDataTemplate data = new SerializeDataTemplate();
        data.Id = 1;
        data.Name = "Tom";
        data.Score = new ScoreTemplate();
        data.Score.English = 98;
        data.Score.Mathematics = 86;
        data.Hobby = new List<string>();
        data.Hobby.Add("Exercise");
        data.Hobby.Add("Study");

        FileStream fileStream = new FileStream(Application.dataPath + "/testBinary.bytes", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fileStream, data);
        fileStream.Close();
        Debug.Log("BinarySerializeData Done.");
    }

    private void BinaryDeSerializeData()
    {
        TextAsset textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/testBinary.bytes");
        MemoryStream stream = new MemoryStream(textAsset.bytes);
        BinaryFormatter bf = new BinaryFormatter();
        SerializeDataTemplate data = bf.Deserialize(stream) as SerializeDataTemplate;

        Debug.Log("XMLDeSerializeData Done.");
        Debug.Log("------------------------");
        Debug.Log("Id:" + data.Id);
        Debug.Log("Name:" + data.Name);
        Debug.Log("Score:");
        string score = "--Mathematics:" + data.Score.Mathematics + " English:" + data.Score.English;
        Debug.Log(score);
        Debug.Log("Hobby:");
        string hobby = "--";
        foreach (var item in data.Hobby) hobby = hobby + "[" + item + "]";
        Debug.Log(hobby);

    }
}
