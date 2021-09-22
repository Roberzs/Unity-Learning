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

        private List<AStarNode> openList = new List<AStarNode>();
        private List<AStarNode> closeList = new List<AStarNode>();

        /// <summary>
        /// 地图的初始化方法
        /// </summary>
        /// <param name="maps"></param>
        public void InitMapInfo(Grid[,] maps)
        {
            mapW = maps.GetLength(1);
            mapH = maps.GetLength(0);
            nodes = new AStarNode[mapW, mapH];
            for (int x = 0; x < mapW; x++)
                for (int y = 0; y < mapH; y++)
                {
                    AStarNode node = new AStarNode(x, y, maps[x,y].gridType == GridType.Stop ? E_Node_Type.Stop : E_Node_Type.Walk);
                    nodes[x, y] = node;
                }
            //Debug.Log(mapW + " " + mapH);
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
                FindNearlyNodeToOpenList((int)startNode.coordinate.x - 1, (int)startNode.coordinate.y + 1, Mathf.Sqrt(2), startNode, endNode);
                FindNearlyNodeToOpenList((int)startNode.coordinate.x, (int)startNode.coordinate.y + 1, 1f, startNode, endNode);
                FindNearlyNodeToOpenList((int)startNode.coordinate.x + 1, (int)startNode.coordinate.y + 1, Mathf.Sqrt(2), startNode, endNode);
                FindNearlyNodeToOpenList((int)startNode.coordinate.x - 1, (int)startNode.coordinate.y, 1f, startNode, endNode);
                FindNearlyNodeToOpenList((int)startNode.coordinate.x + 1, (int)startNode.coordinate.y, 1f, startNode, endNode);
                FindNearlyNodeToOpenList((int)startNode.coordinate.x - 1, (int)startNode.coordinate.y - 1, Mathf.Sqrt(2), startNode, endNode);
                FindNearlyNodeToOpenList((int)startNode.coordinate.x, (int)startNode.coordinate.y - 1, 1f, startNode, endNode);
                FindNearlyNodeToOpenList((int)startNode.coordinate.x + 1, (int)startNode.coordinate.y - 1, Mathf.Sqrt(2), startNode, endNode);

                openList.Sort(SortOpenList);
                if (openList == null) return null;

                closeList.Add(openList[0]);
                startNode = openList[0];
                openList.RemoveAt(0);

                if (startNode == endNode)
                {
                    // 已寻到终点
                    List<AStarNode> pathNode = new List<AStarNode>();
                    //Debug.Log(pathNode.Count);
                    pathNode.Add(endNode);
                    while (endNode.parentNode != null)
                    {
                        endNode = endNode.parentNode;
                        pathNode.Add(endNode);
                    }
                    pathNode.Reverse();
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

            // ????? 这里的判断有问题 不是最佳路径！ 
            // 取点
            AStarNode thisNode = nodes[x, y];
            if (thisNode.type == E_Node_Type.Stop || openList.Contains(thisNode) || closeList.Contains(thisNode))
                return;
            //AStarNode thisNode = new AStarNode(x, y, nodes[x, y].type);
            //if (thisNode.type == E_Node_Type.Stop)
            //    return;
            //foreach (var item in openList)
            //{
            //    if (item.coordinate == thisNode.coordinate && item.parentNode == parentNode) return;
            //}
            //foreach (var item in closeList)
            //{
            //    if (item.coordinate == thisNode.coordinate && item.parentNode == parentNode) return;
            //}

            thisNode.parentNode = parentNode;
            thisNode.g = thisNode.parentNode.g + g;
            thisNode.h = Mathf.Abs(endNode.coordinate.x - x) + Mathf.Abs(endNode.coordinate.y - y);
            thisNode.f = thisNode.g + thisNode.h;

            openList.Add(thisNode);
            //Debug.Log("1");
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

