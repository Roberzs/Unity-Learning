/****************************************************
    文件：MeshTest1.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class MeshTest1 : MonoBehaviour
{
    private void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = GetVertices();
        mesh.triangles = GetTriangles();
    }

    private Vector3[] GetVertices()
    {
        return new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 0)
        };
    }

    private int[] GetTriangles()
    {
        return new int[] 
        { 
            0, 1, 2, 
            0, 2, 3
        };
    }
}
