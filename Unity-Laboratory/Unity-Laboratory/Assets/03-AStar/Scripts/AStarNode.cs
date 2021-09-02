/****************************************************
    文件：AStarNode.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{ 

    public enum E_Node_Type
    {
        Null,
        Walk,
        Stop,
    }

    public class AStarNode
    {
        /// <summary>
        /// 坐标
        /// </summary>
        public Vector2 coordinate;
        /// <summary>
        /// 父节点
        /// </summary>
        public AStarNode parentNode;
        /// <summary>
        /// 网格类型
        /// </summary>
        public E_Node_Type type;

        public float f;     // 寻路消耗
        public float g;     // 离起点的距离
        public float h;     // 离终点的距离

        public AStarNode(int x, int y, E_Node_Type type)
        {
            this.coordinate = new Vector2(x, y);
            this.type = type;
        }
    }
}

