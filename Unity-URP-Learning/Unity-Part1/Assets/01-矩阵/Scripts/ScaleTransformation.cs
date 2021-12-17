/****************************************************
	文件：ScaleTransformation.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

namespace Part1_1
{
    public class ScaleTransformation : Transformation
    {
        public Vector3 scale;

        public override Matrix4x4 Matrix
        {
            get
            {
                /**
                 * 缩放矩阵
                 * 
                 * |x   0   0   0|
                 * |0   y   0   0|
                 * |0   0   z   0|
                 * |0   0   0   1|
                 */
                Matrix4x4 matrix = new Matrix4x4();
                matrix.SetRow(0, new Vector4(scale.x, 0f, 0f, 0f));
                matrix.SetRow(1, new Vector4(0f, scale.y, 0f, 0f));
                matrix.SetRow(2, new Vector4(0f, 0f, scale.z, 0f));
                matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
                return matrix;
            }
        }

        public override Vector3 Apple(Vector3 point)
        {
            point.x *= scale.x;
            point.y *= scale.y;
            point.z *= scale.z;
            return point;
        }
    }
}

