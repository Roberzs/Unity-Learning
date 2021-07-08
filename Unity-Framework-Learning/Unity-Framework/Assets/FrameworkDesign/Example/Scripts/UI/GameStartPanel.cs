/****************************************************
    文件：GameStartPanel.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/6 15:24:27
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace FrameworkDesign.Example
{
    public class GameStartPanel : MonoBehaviour
    {
        //[SerializeField] private GameObject Enemies;

        private void Start()
        {
            transform.Find("btnGameStart").GetComponent<Button>().onClick
                .AddListener(() =>
                {
                    gameObject.SetActive(false);

                    new StartGameCommand().Execute();
                });
        }
    }

}

