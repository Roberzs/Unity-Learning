/****************************************************
	文件：TransformationGrid.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Part1_1
{
    public class TransformationGrid : MonoBehaviour
    {
		public Transform prefab;
		public int gridResolution = 10;
		
		private Transform[] grids;
		private List<Transformation> transformations;
        private Matrix4x4 transformtion;

        private void Awake()
        {
			grids = new Transform[gridResolution * gridResolution * gridResolution];
			transformations = new List<Transformation>();

			for (int z = 0, i = 0; z < gridResolution; z++)
            {
                for (int y = 0; y < gridResolution; y++)
                {
                    for (int x = 0; x < gridResolution; x++, i++)
                    {
						grids[i] = CreateGridPoint(x, y, z);
					}
                }
            }
        }

		private Transform CreateGridPoint(int x, int y, int z)
        {
			Transform point = Instantiate<Transform>(prefab);
			point.localPosition = GetCoordinates(x, y, z);
			point.GetComponent<MeshRenderer>().material.color = new Color(
				(float)x / gridResolution,
				(float)y / gridResolution,
				(float)z / gridResolution
				);
			return point;
        }

		private Vector3 GetCoordinates(int x , int y, int z)
        {
			return new Vector3(
				x - (gridResolution - 1) * 0.5f,
				y - (gridResolution - 1) * 0.5f,
				z - (gridResolution - 1) * 0.5f
				);
        }

        private void Update()
        {
            UpdateTransformation();
			//GetComponents<Transformation>(transformations);
            for (int z = 0, i = 0; z < gridResolution; z++)
            {
                for (int y = 0; y < gridResolution; y++)
                {
                    for (int x = 0; x < gridResolution; x++, i++)
                    {
                        grids[i].localPosition = TransformPoint(x, y, z);
                    }
                }
            }
        }

		private Vector3 TransformPoint(int x, int y, int z)
        {
            Vector3 coordinates = GetCoordinates(x, y, z);
            //for (int i = 0; i < transformations.Count; i++)
            //{
            //    coordinates = transformations[i].Apple(coordinates);
            //}
            //return coordinates;
            
            // 矩阵方式运算
            return transformtion.MultiplyPoint(coordinates);
        }

        private void UpdateTransformation()
        {
            GetComponents<Transformation>(transformations);
            if (transformations.Count > 0)
            {
                transformtion = transformations[0].Matrix;
                for (int i = 1; i < transformations.Count; i++)
                {
                    transformtion *= transformations[i].Matrix;
                }
            }
        }
    }
}

