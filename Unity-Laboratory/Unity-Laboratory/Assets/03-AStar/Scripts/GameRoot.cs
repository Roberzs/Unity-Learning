/****************************************************
    文件：GameRoot.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：总管理类 负责初始化地图与传递寻路数据
*****************************************************/

using UnityEngine;
using System.Collections;

namespace AStar
{
    public class GameRoot : MonoBehaviour
    {
        [SerializeField] private int mapW = 10;
        [SerializeField] private int mapH = 10;

        [Header("Prefabs")]
        [SerializeField ] private GameObject gridPrefab;
        [SerializeField] private GameObject characterPrefab;
        [SerializeField] private GameObject barrierPrefab;

        [Header("MapData")]
        private Grid[,] maps;

        private void Start()
        {
            StartCoroutine(InitMap(mapW, mapH));
        }

        private IEnumerator InitMap(int w, int h)
        {
            Debug.Log("Map Init Start.");
            maps = new Grid[w, h];
            Debug.Log("Grid Init Start.");
            for (int x = 0; x < w; ++x)
            {
                for (int z = 0; z < h; ++z)
                {
                    yield return new WaitForSeconds(0.05f);
                    GameObject item = Instantiate(gridPrefab, transform);
                    item.transform.position = new Vector3(x, 0f, z);
                    Grid gridScript = item.GetComponent<Grid>();
                    gridScript.InitGrid();
                    gridScript.coordinate = new Vector2(x, z);
                    maps[x, z] = gridScript;
                }
            }
            Debug.Log("Grid Init Done.");

            yield return new WaitForSeconds(0.2f);

            Debug.Log("Barrier Init Start.");
            int rand = Random.Range(3, 6);
            for (int i = 0; i < rand; i++)
            {
                yield return new WaitForSeconds(0.05f);
                GameObject item = Instantiate(barrierPrefab, transform);
                item.transform.localScale = new Vector3(Random.Range(1f, 3f), 1f, 1f);
                item.transform.eulerAngles = new Vector3(0f, Random.Range(25f, 135f), 0f);
                item.transform.position = new Vector3(Random.Range(2, w - 1), 2f, Random.Range(2, h - 1));
            }
            Debug.Log("Barrier Init Done.");

            yield return new WaitForSeconds(3.5f);

            Debug.Log("Character Init Start.");
            while (true)
            {
                int randeX = Random.Range(0, w), randeY = Random.Range(0, h);
                if (maps[randeX, randeY].gridType == GridType.Walk)
                {
                    GameObject character = Instantiate(characterPrefab, transform);
                    character.transform.position = new Vector3(randeX, 1f, randeY);
                    break;
                }
            }
            Debug.Log("Character Init Done.");

            Debug.Log("Map Init Done.");

            Debug.Log("Call AStarMgr InitMapInfo.");
            AStarManager.Instance.InitMapInfo(maps);

        }
    }
}

