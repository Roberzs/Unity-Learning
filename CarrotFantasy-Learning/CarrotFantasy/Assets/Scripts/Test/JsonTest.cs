/****************************************************
    文件：JsonTest.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class JsonTest : MonoBehaviour 
{
    private AppOfPhone appOfPhone;

    private void Start()
    {
        ComplexJsonTest();
    }


    // 复杂Json的存储读取
    private void ComplexJsonTest()
    {

        // 写入测试
        //appOfPhone = new AppOfPhone
        //{
        //    appNum = 1,
        //    phoneState = true,
        //    appList = new List<AppProperty>()
        //};
        //AppProperty appProperty = new AppProperty
        //{
        //    appName = "Bilibili",
        //    userID = "三二",
        //    isFavour = true,
        //    useTimeList = new List<int>() { 7, 8 }
        //};
        //appOfPhone.appList.Add(appProperty);
        //SaveByJson();

        // 读取测试
        appOfPhone = new AppOfPhone();
        appOfPhone = LoadByJson();
        Debug.Log(appOfPhone.appNum);
        Debug.Log(appOfPhone.phoneState);
        foreach (var item in appOfPhone.appList)
        {
            Debug.Log(item.appName);
            Debug.Log(item.userID);
            Debug.Log(item.isFavour);
            foreach (var time in item.useTimeList)
            {
                Debug.Log(time);
            }
        }
    }


    // 简单Json的存储读取
    private void SimpleJsonTest()
    {
        // 写入测试
        //appOfPhone = new AppOfPhone
        //{
        //    appNum = 3,
        //    phoneState = true,
        //    appList = new List<string>() 
        //    {
        //        "Bilibili", "QQ", "微信"
        //    }
        //};
        //SaveByJson();

        // 读取测试
        appOfPhone = new AppOfPhone();
        appOfPhone = LoadByJson();
        Debug.Log(appOfPhone.appNum);
        Debug.Log(appOfPhone.phoneState);
        foreach (var item in appOfPhone.appList)
        {
            Debug.Log("App: " + item);
        }
    }

    // Json文件存储
    private void SaveByJson()
    {
        string filePath = Application.streamingAssetsPath + "/Json/" + "AppOfPhone.json";

        // 利用JsonMapper将信息类转换成Json类型字符串
        string saveJsonStr = JsonMapper.ToJson(appOfPhone);

        // 创建一个文件流将字符串写入文件
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
    }

    // Json文件读取
    private AppOfPhone LoadByJson()
    {
        AppOfPhone appGo = new AppOfPhone();
        string filePath = Application.streamingAssetsPath + "/Json/" + "AppOfPhone.json";

        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();

            appGo = JsonMapper.ToObject<AppOfPhone>(jsonStr);
        }

        if (appOfPhone == null) Debug.LogError("文件读取失败");
        return appGo;
    }

}