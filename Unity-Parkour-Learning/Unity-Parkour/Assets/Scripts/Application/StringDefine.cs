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
    public const string AudioClipsRootPath = "Sound/";
    // 游戏物体资源根目录
    public const string PrefabsRootPath = "Prefabs/";

    // Sound
    public const string S_Jump = "Se_UI_Jump";
    public const string S_Slide = "Se_UI_Slide";
    public const string S_Huadong = "Se_UI_Huadong";

    // Event
    public const string E_ExitScene = "E_ExitScene";
    public const string E_EnterScene = "E_EnterScene";
    public const string E_StartUp = "E_StartUp";
    public const string E_EndGame = "E_EndGame";

    // Model
    public const string M_GameModel = "M_GameModel";

    // View 
    public const string V_PlayerMove = "V_PlayerMove";
    public const string V_PlayerAnim = "PlayerAnim";
}

public enum InputDirection
{
    NULL,
    RIGHT,
    LEFT,
    DOWN,
    UP
}