/****************************************************
    文件：AStarManager.cs
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
    public class AStarManager
    {
        private static AStarManager _instance;
        public static AStarManager Instance
        {
            get
            {
                if (_instance == null) _instance = new AStarManager();
                return _instance;
            }
        }

        private int mapW;
        private int mapH;

        private AStarNode[,] nodes;

        private List<AStarNode> openList;
        private List<AStarNode> closeList;

        /// <summary>
        /// 地图的初始化方法
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void InitMapInfo(int w, int h)
        {
            nodes = new AStarNode[w, h];
            for (int x = 0; x < w; ++x)
                for (int y = 0; y < h; ++y)
                {
                    AStarNode node = new AStarNode(x, y, Random.Range(0, 100) < 20 ? E_Node_Type.Stop : E_Node_Type.Walk);
                    nodes[x, y] = node;
                }

        }

        public List<AStarNode> FindPath(Vector2 starPos, Vector2 endPos)
        {
            return null;
        }
    }
}

