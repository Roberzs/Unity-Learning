/****************************************************
    文件：MeshTest1.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class MeshTest1 : MonoBehaviour
{
    public Vector4 Tangent;
    private Mesh _mesh;

    private void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        _mesh.name = "Mesh1";
        _mesh.vertices = GetVertices();
        _mesh.triangles = GetTriangles();
        _mesh.uv = GetUVs();
        _mesh.normals = GetNormals();
    }

    private void Update()
    {
        _mesh.tangents = GetTangents();
    }

    private Vector4[] GetTangents()
    {
        return new Vector4[]
        {
            Tangent,
            Tangent,
            Tangent,
            Tangent
        };
    }

    private Vector3[] GetNormals()
    {
        return new Vector3[]
        {
            Vector3.right,
            Vector3.right,
            Vector3.right,
            Vector3.right
        };
    }

    private Vector2[] GetUVs()
    {
        return new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(-1, 0),
            new Vector2(-1, 1),
            new Vector2(0, 1),
        };
    }

    private Vector3[] GetVertices()
    {
        return new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(-1, 1, 0),
            new Vector3(0, 1, 0)
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
