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
    public const string E_PauseGame = "E_PauseGame";
    public const string E_ResumeGame = "E_ResumeGame";

    public const string E_UpdateDis = "E_UpdateDis";
    public const string E_UpdateCoin = "E_UpdateCoin";
    public const string E_AddTimer = "E_AddTimer";

    // Model
    public const string M_GameModel = "M_GameModel";

    // View 
    public const string V_PlayerMove = "V_PlayerMove";
    public const string V_PlayerAnim = "V_PlayerAnim";

    public const string V_UIBoard = "V_UIBoard";
    public const string V_UIPause = "V_UIPause";
    public const string V_UIResume = "V_UIResume";
}

public enum InputDirection
{
    NULL,
    RIGHT,
    LEFT,
    DOWN,
    UP
}

public enum ItemKind
{
    ItemInvincible,
    ItemMagnet,
    ItemMultiply,
}