/****************************************************
	文件：BinarySerializeOption.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using ProtoBuf;

public class BinarySerializeOption
{

    public static bool ProtoBufSerialize<T>(string path, System.Object obj)
    {
        try
        {
            using (Stream stream = File.Create(path))
            {
                ProtoBuf.Serializer.Serialize(stream, obj);
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Protobuf serialize Error:" + e);
        }
        return false;
    }

    public static System.Object ProtoBufDeserialize(string path, Type type)
    {
        System.Object obj = null;
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                obj = Serializer.Deserialize(type, fs);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Protobuf deserialize Error:{path},{e}");
        }
        return obj;
    }

    public static T ProtoBufDeserialize<T>(string path) where T : class
    {
        T t = default(T);
        TextAsset textAsset = ResourceManager.Instance.LoadResource<TextAsset>(path);
        if (textAsset == null)
        {
            Debug.LogError($"cant load TextAsset:{path}");
            return null;
        }
        try
        {
            using (MemoryStream stream = new MemoryStream(textAsset.bytes))
            {
                //XmlSerializer xs = new XmlSerializer(typeof(T));
                //t = (T)xs.Deserialize(stream);

                t = Serializer.Deserialize<T>(stream);
            }
            ResourceManager.Instance.ReleaseResource(path, true);
        }
        catch (Exception e)
        {
            Debug.LogError($"load TextAsset exception:{path},{e}");
        }
        return t;
    }

    /// <summary>
    /// XML序列化
    /// </summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool XmlSerialize(string path, System.Object obj)
    {
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    // 修改命名空间
                    //XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    //namespaces.Add(string.Empty, string.Empty);

                    XmlSerializer xs = new XmlSerializer(obj.GetType());
                    xs.Serialize(sw, obj);
                }
            }
            return true;
            
        }
        catch(Exception e)
        {
            Debug.LogError($"此类转换为XML失败:{obj.GetType()},{e}");
            // 删除生成异常的文件 TODO
        }

		return false;
    }

    /// <summary>
    /// 编辑器环境XML反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T XmlDeserializeInFile<T>(string path) where T : class
    {
        T t = default(T);
        try
        {
            using(FileStream fs = new FileStream(path, FileMode.Open,FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                t = (T)xs.Deserialize(fs);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"xml deserialize Error:{path},{e}");
        }
        return t;
    }

    /// <summary>
    /// XML反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T XmlDeserialize<T>(string path) where T : class
    {
        T t = default(T);
        TextAsset textAsset = ResourceManager.Instance.LoadResource<TextAsset>(path);
        if (textAsset == null)
        {
            Debug.LogError($"cant load TextAsset:{path}");
            return null;
        }
        try
        {
            using (MemoryStream stream = new MemoryStream(textAsset.bytes))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                t = (T)xs.Deserialize(stream);
            }
            ResourceManager.Instance.ReleaseResource(path, true);
        }
        catch (Exception e)
        {
            Debug.LogError($"load TextAsset exception:{path},{e}");
        }
        return t;
    }

    /// <summary>
    /// 二进制序列化
    /// </summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool BinarySerilize(string path, System.Object obj)
    {
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, obj);

            }
            return true;

        }
        catch (Exception e)
        {
            Debug.LogError($"此类转换为Byte失败:{obj.GetType()},{e}");
            // 删除生成异常的文件 TODO
        }

        return false;
    }

    public static System.Object XmlDeserializeInFile(string path, Type type)
    {
        System.Object obj = null;
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                XmlSerializer xs = new XmlSerializer(type);
                obj = xs.Deserialize(fs);
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"xml deserialize Error:{path},{e}");
        }
        return obj;
    }

    /// <summary>
    /// 二进制反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T BinaryDeserialize<T>(string path) where T : class
    {
        T t = default(T);
        TextAsset textAsset = ResourceManager.Instance.LoadResource<TextAsset>(path);
        if (textAsset == null)
        {
            Debug.LogError($"cant load TextAsset:{path}");
            return null;
        }
        try
        {
            using (MemoryStream stream = new MemoryStream(textAsset.bytes))
            {
                BinaryFormatter bf = new BinaryFormatter();
                t = (T)bf.Deserialize(stream);
            }
            ResourceManager.Instance.ReleaseResource(path, true);
        }
        catch (Exception e)
        {
            Debug.LogError($"load TextAsset exception:{path},{e}");
        }
        return t;
    }
}

