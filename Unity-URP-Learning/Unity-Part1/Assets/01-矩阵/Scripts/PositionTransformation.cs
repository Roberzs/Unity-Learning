/****************************************************
	文件：PositionTransformation.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

namespace Part1_1
{
    public class PositionTransformation : Transformation
    {
        public Vector3 position;

        public override Matrix4x4 Matrix
        {
            get
            {
                /**
                 * 位移矩阵
                 * 
                 * |1   0   0   x|
                 * |0   1   0   y|
                 * |0   0   1   z|
                 * |0   0   0   1|
                 */
                Matrix4x4 matrix = new Matrix4x4();
                matrix.SetRow(0, new Vector4(1f, 0f, 0f, position.x));
                matrix.SetRow(1, new Vector4(0f, 1f, 0f, position.y));
                matrix.SetRow(2, new Vector4(0f, 0f, 1f, position.z));
                matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
                return matrix;
            }
        }

        /// <summary>
        /// 坐标偏移
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override Vector3 Apple(Vector3 point)
        {
            // 当前坐标+偏移坐标
            return point + position;
        }
    }
}
