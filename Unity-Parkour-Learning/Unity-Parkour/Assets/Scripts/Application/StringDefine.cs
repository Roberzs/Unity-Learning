/****************************************************
    文件：StringDefine.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

public static class StringDefine
{
    // 2D资源根目录
    public const string SpritesRootPath = "";
    // 音频资源根目录
    public const string AudioClipsRootPath = "";
    // 游戏物体资源根目录
    public const string PrefabsRootPath = "Prefabs/";

    // Event
    public const string E_ExitScene = "E_ExitScene";
    public const string E_EnterScene = "E_EnterScene";
    public const string E_StartUp = "E_StartUp";

    // Model
    public const string M_GameModel = "M_GameModel";

    // View 
    public const string V_PlayerMove = "V_PlayerMove";
}

public enum InputDirection
{
    NULL,
    RIGHT,
    LEFT,
    DOWN,
    UP
}