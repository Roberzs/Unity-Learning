/****************************************************
    文件：Grid.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：格子数据脚本 用于模拟存储真实的地图网格数据
*****************************************************/

using UnityEngine;
using DG.Tweening;

namespace AStar
{
    public enum GridType
    {
        Null,
        Walk,
        Stop,
    }

    public class Grid : MonoBehaviour
    {
        public GridType gridType = GridType.Null;

        public Vector2 coordinate;

        public void InitGrid()
        {
            // 一个小动画
            transform.DOMoveY(0.2f, 0.1f).OnComplete(() => transform.DOMoveY(0f, 0.1f));

            gridType = GridType.Walk;
            SetMaterialColor(Color.white);
        }

        private void OnMouseDown()
        {
            Debug.Log("点击了坐标 " + coordinate + " 的网格" );
        }

        private void SetMaterialColor(Color color)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Barrier"))
            {
                if (gridType != GridType.Stop)
                {
                    gridType = GridType.Stop;
                    SetMaterialColor(Color.red);
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Barrier"))
            {
                if (gridType != GridType.Walk)
                {
                    gridType = GridType.Walk;
                    SetMaterialColor(Color.white);
                }
            }
        }
    }
}
