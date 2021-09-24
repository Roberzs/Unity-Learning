/****************************************************
    文件：CubeCreater.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

public class CubeCreater : MonoBehaviour 
{
    [SerializeField]
    private float interval = 1.5f;

    [SerializeField]
    private int size = 100;

    [SerializeField]
    private GameObject cubePrefab;

    private void Start()
    {
        float startTime = Time.realtimeSinceStartup;

        Vector3 tmpVector = new Vector3(0f, 0f, 0f);
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    GameObject tmpCube = Instantiate(cubePrefab, transform);
                    tmpCube.transform.position = tmpVector;
                    tmpVector.z += interval;
                }
                tmpVector.y += interval;
                tmpVector.z = 0;
            }
            tmpVector.x += interval;
            tmpVector.y = 0f;
        }
        Debug.Log((Time.realtimeSinceStartup - startTime));
    }
}

