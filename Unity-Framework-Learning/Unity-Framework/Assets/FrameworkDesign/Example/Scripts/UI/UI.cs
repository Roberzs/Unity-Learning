/****************************************************
    文件：UI.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/6 15:42:38
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

namespace FrameworkDesign.Example
{
    public class UI : MonoBehaviour
    {
        private void Start()
        {
            GamePassEvent.Register(OnGamePass);
        }

        private void OnDestroy()
        {
            GamePassEvent.UnRegister(OnGamePass);
        }

        private void OnGamePass()
        {
            transform.Find("Canvas/GamePassPanel").gameObject.SetActive(true);
        }
    }
}


