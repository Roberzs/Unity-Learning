/****************************************************
    文件：DP07TemplateMethod.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DP07TemplateMethod : MonoBehaviour 
{
    private void Start()
    {
        IGame game = new FootBall();
        game.PlayGame();
    }
}

#region - TemplateMethod

public abstract class IGame
{
    public void PlayGame()
    {
        InitGame();
        StartGame();
        EndGame();
    }

    private void InitGame()
    {
        Debug.Log("初始化游戏");
    }

    protected virtual void StartGame() 
    {

    }
    

    private void EndGame()
    {
        Debug.Log("结束游戏");
    }
}

public class BasketBall : IGame
{
    protected override void StartGame()
    {
        Debug.Log("开始打篮球");
    }
}

public class FootBall: IGame
{
    protected override void StartGame()
    {
        Debug.Log("开始踢足球");
    }
}

#endregion