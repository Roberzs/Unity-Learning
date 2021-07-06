/****************************************************
    文件：Memento.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using LitJson;
using System.IO;

public class Memento
{
    // 读取
    public PlayerManager LoadByJson()
    {
        PlayerManager playerManager = new PlayerManager();
        string filePath = string.Empty;
        if (GameManager.Instance.initPlayerManager)
        {
            filePath = Application.streamingAssetsPath + "/Json/" + "PlayerManagerInitData.json";
        }
        else
        {
            filePath = Application.streamingAssetsPath + "/Json/" + "PlayerManager.json";
        }
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            playerManager = JsonMapper.ToObject<PlayerManager>(jsonStr);
            return playerManager;
        }
        else
        {
            Debug.LogError("PlayerManager数据读取失败 路径:" + filePath);
            return null;
        }
        
    }

    // 存储
    public void SaveByJson()
    {
        PlayerManager playerManager = GameManager.Instance.playerManager;
        string filePath = Application.streamingAssetsPath + "/Json/" + "PlayerManager.json";
        string saveJsonStr = JsonMapper.ToJson(playerManager);
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close(); 
    }
}