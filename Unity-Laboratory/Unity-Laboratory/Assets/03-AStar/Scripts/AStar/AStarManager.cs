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
        /// <param name="maps"></param>
        public void InitMapInfo(Grid[,] maps)
        {
            mapW = maps.Rank;
            mapH = maps.GetLength(0);
            nodes = new AStarNode[mapW, mapH];
            for (int x = 0; x < mapW; ++x)
                for (int y = 0; y < mapH; ++y)
                {
                    AStarNode node = new AStarNode(x, y, maps[x,y].gridType == GridType.Stop ? E_Node_Type.Stop : E_Node_Type.Walk);
                    nodes[x, y] = node;
                }
            Debug.Log("AStarManager InitMapInfo Done.");
        }

        public List<AStarNode> FindPath(Vector2 starPos, Vector2 endPos)
        {
            if (starPos.x < 0 || starPos.x >= mapW || starPos.y < 0 || starPos.y >= mapH ||
                endPos.x < 0 || endPos.x >= mapW || endPos.y < 0 || endPos.y >= mapH)
                return null;
            if (nodes[(int)starPos.x, (int)starPos.y].type == E_Node_Type.Stop ||
                nodes[(int)endPos.x, (int)endPos.y].type == E_Node_Type.Stop)
                return null;

            openList.Clear();
            closeList.Clear();
            AStarNode startNode = nodes[(int)starPos.x, (int)starPos.y];
            startNode.parentNode = null;
            startNode.f = 0;
            startNode.g = 0;
            startNode.h = 0;
            closeList.Add(startNode);

            AStarNode endNode = nodes[(int)endPos.x, (int)endPos.y];

            while (true)
            {
                FindNearlyNodeToOpenList((int)starPos.x - 1, (int)starPos.y + 1, Mathf.Sqrt(2), startNode, endNode);
                FindNearlyNodeToOpenList((int)starPos.x, (int)starPos.y + 1, 1f, startNode, endNode);
                FindNearlyNodeToOpenList((int)starPos.x + 1, (int)starPos.y + 1, Mathf.Sqrt(2), startNode, endNode);
                FindNearlyNodeToOpenList((int)starPos.x - 1, (int)starPos.y, 1f, startNode, endNode);
                FindNearlyNodeToOpenList((int)starPos.x + 1, (int)starPos.y, 1f, startNode, endNode);
                FindNearlyNodeToOpenList((int)starPos.x - 1, (int)starPos.y - 1, Mathf.Sqrt(2), startNode, endNode);
                FindNearlyNodeToOpenList((int)starPos.x, (int)starPos.y - 1, 1f, startNode, endNode);
                FindNearlyNodeToOpenList((int)starPos.x + 1, (int)starPos.y - 1, Mathf.Sqrt(2), startNode, endNode);

                openList.Sort(SortOpenList);

                closeList.Add(openList[0]);
                startNode = openList[0];
                openList.RemoveAt(0);

                if (startNode == endNode)
                {
                    // 已寻到终点
                    List<AStarNode> pathNode = new List<AStarNode>();
                    pathNode.Add(endNode);
                    while (endNode.parentNode == null)
                    {
                        endNode = endNode.parentNode;
                        pathNode.Add(endNode);
                    }
                    return pathNode;
                }
            }
        }

        /// <summary>
        /// 寻找判断附近点并添加到开启列表
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="g"></param>
        /// <param name="parentNode"></param>
        /// <param name="endNode"></param>
        private void FindNearlyNodeToOpenList(int x, int y, float g, AStarNode parentNode, AStarNode endNode)
        {
            // 边界检测
            if (x < 0 || x >= mapW || y < 0 || y >= mapH) 
                return;
            // 取点
            AStarNode thisNode = nodes[x, y];
            if (thisNode.type == E_Node_Type.Stop || openList.Contains(thisNode) || closeList.Contains(thisNode))
                return;
            thisNode.parentNode = parentNode;
            thisNode.g = thisNode.parentNode.g + g;
            thisNode.h = Mathf.Abs(endNode.coordinate.x - x) + Mathf.Abs(endNode.coordinate.y - y);
            thisNode.f = thisNode.g + thisNode.h;

            openList.Add(thisNode);
        }

        /// <summary>
        /// 开启列表排序算法
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int SortOpenList(AStarNode a, AStarNode b)
        {
            if (a.f > b.f)
                return 1;
            else if (a.f == b.f)
                return 1;
            return -1;
        }
    }
}

