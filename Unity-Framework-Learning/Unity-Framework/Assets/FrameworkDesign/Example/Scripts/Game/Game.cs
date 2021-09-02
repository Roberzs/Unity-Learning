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
    public class Game : MonoBehaviour, IController
    {
        private void Start()
        {
            this.RegisterEvent<GameStartEvent>(OnGameStart);
        }

        private void OnDestroy()
        {
            this.UnRegisterEvent<GameStartEvent>(OnGameStart);
        }

        private void OnGameStart(GameStartEvent e)
        {
            transform.Find("Enemies").gameObject.SetActive(true);
        }

        public IArchitecture GetArchitecture()
        {
            return PointGame.Interface;
        }
    }
}

