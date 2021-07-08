/****************************************************
    文件：Game.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/6 15:37:57
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

namespace FrameworkDesign.Example
{
    public class Game : MonoBehaviour
    {
        private void Start()
        {
            GameStartEvent.Register(OnGameStart);
        }

        private void OnDestroy()
        {
            GameStartEvent.UnRegister(OnGameStart);
        }

        private void OnGameStart()
        {
            transform.Find("Enemies").gameObject.SetActive(true);
        }
    }
}

