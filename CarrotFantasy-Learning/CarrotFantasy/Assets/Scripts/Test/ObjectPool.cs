/****************************************************
    文件：ObjectPool.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour 
{
    public GameObject cubePoolGO, activeCubePoolGO;

    private GameObject cube;
    private Stack<GameObject> cubePoolList;
    private Stack<GameObject> activeCubePoolList;

    private void Start()
    {
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.SetParent(transform);
        cube.SetActive(false);

        cubePoolList = new Stack<GameObject>();
        activeCubePoolList = new Stack<GameObject>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject itemGO = GetCube();
            itemGO.transform.position = Vector3.zero;
            activeCubePoolList.Push(itemGO);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (activeCubePoolList.Count > 0)
            {
                PushCube(activeCubePoolList.Pop());
            }
        }
    }

    private GameObject GetCube()
    {
        GameObject cubeGO;
        if (cubePoolList.Count <= 0)
        {
            cubeGO = Instantiate(cube);
        }
        else
        {
            cubeGO = cubePoolList.Pop();
        }
        cubeGO.transform.SetParent(activeCubePoolGO.transform);
        cubeGO.SetActive(true);
        return cubeGO;
    }

    private void PushCube(GameObject cubeGO)
    {
        cubeGO.transform.SetParent(cubePoolGO.transform);
        cubeGO.SetActive(false);
        cubePoolList.Push(cubeGO);
    }
}