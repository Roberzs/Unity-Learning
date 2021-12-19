/****************************************************
    文件：Constants.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/9/8 22:49:18
	功能：常量配置
*****************************************************/

using UnityEngine;

public enum TxtColor
{
    Red,
    Green,
    Blue,
    Yellow
}

public class Constants 
{
    //-- 文字染色工具 --//
    private const string ColorRed = "<color=#FF0000FF>";
    private const string ColorGreen = "<color=#00FF00FF>";
    private const string ColorBlue = "<color=#00B4FFFF>";
    private const string ColorYellow = "<color=#FFFF00FF>";
    private const string ColorEnd = "</color>";

    public static string Color(string str, TxtColor c)
    {
        string result = "";
        switch (c)
        {
            case TxtColor.Red:
                result = ColorRed + str + ColorEnd;
                break;
            case TxtColor.Green:
                result = ColorGreen + str + ColorEnd;
                break;
            case TxtColor.Blue:
                result = ColorBlue + str + ColorEnd;
                break;
            case TxtColor.Yellow:
                result = ColorYellow + str + ColorEnd;
                break;
        }
        return result;
    }

    //-- 场景名称 --//
    public const string SceneLogin = "SceneLogin";
    //public const string SceneMainCity = "SceneMainCity";
    public const int MainCityMapID = 10000;

    //-- 音效名称 --//
    public const string BGLogin = "bgLogin";
    public const string BGMainCity = "bgMainCity";
    public const string BGHuangYe = "bgHuangYe";

    public const string UILoginBtn = "uiLoginBtn";
    public const string UIClickBtn = "uiClickBtn";
    public const string UIExtenBtn = "uiExtenBtn";
    public const string UIOpenPage = "uiOpenPage";
    public const string FBItem = "fbItem";

    //-- 屏幕标准宽高 --//
    public const float ScreenStandardWidth = 1334;
    public const float ScreenStandardHeight = 750;

    public const float ScreenOPDis = 90;        // 摇杆移动标准距离

    //-- 移动速度 --//
    public const float PlayerMoveSpeed = 8;
    public const float MonsterMoveSpeed = 5;

    //-- 运动平滑加速度 --//
    public const float AccelerSpeed = 5;

    //-- 混合参数 --//
    public const float BlendIdle = 0;
    public const float BlendWalk = 1;

    //-- NPC ID --//
    public const int NPCWiseMan = 0;
    public const int NPCGeneral = 1;
    public const int NPCArtisan = 2;
    public const int NPCTrader = 3;
}