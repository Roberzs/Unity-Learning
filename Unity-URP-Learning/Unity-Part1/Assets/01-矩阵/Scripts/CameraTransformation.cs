/****************************************************
	文件：CameraTransformation.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

namespace Part1_1
{
    public class CameraTransformation : Transformation
    {
        public float focalLength = 1f;

        public override Matrix4x4 Matrix
        {
            get
            {
                /**
                 * 正交像机投影 (放弃z轴)
                 * 
                 * |1   0   0   0|
                 * |0   1   0   0|
                 * |0   0   0   0|
                 * |0   0   0   1|
                 */

                /**
                 * 透视摄像机
                 * 
                 * |1   0   0   0|
                 * |0   1   0   0|
                 * |0   0   0   0|
                 * |0   0   1   0|
                 */

                Matrix4x4 matrix = new Matrix4x4();

                //matrix.SetRow(0, new Vector4(1f, 0f, 0f, 0f));
                //matrix.SetRow(1, new Vector4(0f, 1f, 0f, 0f));
                //matrix.SetRow(2, new Vector4(0f, 0f, 0f, 0f));
                //matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));


                matrix.SetRow(0, new Vector4(focalLength, 0f, 0f, 0f));
                matrix.SetRow(1, new Vector4(0f, focalLength, 0f, 0f));
                matrix.SetRow(2, new Vector4(0f, 0f, 0f, 0f));
                matrix.SetRow(3, new Vector4(0f, 0f, 1f, 0f));

                return matrix;
            }
        }

        public override Vector3 Apple(Vector3 point)
        {
            throw new System.NotImplementedException();
        }
    }
}

