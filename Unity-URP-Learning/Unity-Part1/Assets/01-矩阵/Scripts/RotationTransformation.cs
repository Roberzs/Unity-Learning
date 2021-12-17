/****************************************************
	文件：RotationTransformation.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

namespace Part1_1
{
    public class RotationTransformation : Transformation
    {
        public Vector3 rotation;

        public override Matrix4x4 Matrix
        {
            get
            {
                /**
                * 旋转矩阵
                * 
                * |cosY*cosZ                   -cosY*sinZ                  sinY         0|
                * |cosX*sinZ+sinX*sinY*cosZ    cosX*cosZ-sinX*sinY*sinZ    -sinX*cosY   0|
                * |sinX*sinZ-cosX*sinY*cosZ    sinX*cosZ+cosX*sinY*sinZ    cosX*cosY    0|
                * |0                           0                           0            1|
                */

                float radX = rotation.x * Mathf.Deg2Rad;
                float sinX = Mathf.Sin(radX);
                float cosX = Mathf.Cos(radX);

                float radY = rotation.y * Mathf.Deg2Rad;
                float sinY = Mathf.Sin(radY);
                float cosY = Mathf.Cos(radY);

                float radZ = rotation.z * Mathf.Deg2Rad;
                float sinZ = Mathf.Sin(radZ);
                float cosZ = Mathf.Cos(radZ);

                Matrix4x4 matrix = new Matrix4x4();
                matrix.SetColumn(0, new Vector4(cosY * cosZ,
                    cosX * sinZ + sinX * sinY * cosZ,
                    sinX * sinZ - cosX * sinY * cosZ,
                    0f));
                matrix.SetColumn(1, new Vector4(-cosY * sinZ,
                    cosX * cosZ - sinX * sinY * sinZ,
                    sinX * cosZ + cosX * sinY * sinZ,
                    0f));
                matrix.SetColumn(2, new Vector4(sinY,
                    -sinX * cosY,
                    cosX * cosY,
                    0f));
                matrix.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
                return matrix;
            }
        }

        public override Vector3 Apple(Vector3 point)
        {
            /**
             * 旋转后的最终矩阵
             * 
             * |cosY*cosZ                   -cosY*sinZ                  sinY        |
             * |cosX*sinZ+sinX*sinY*cosZ    cosX*cosZ-sinX*sinY*sinZ    -sinX*cosY  |
             * |sinX*sinZ-cosX*sinY*cosZ    sinX*cosZ+cosX*sinY*sinZ    cosX*cosY   |
             */

            float radX = rotation.x * Mathf.Deg2Rad;
            float sinX = Mathf.Sin(radX);
            float cosX = Mathf.Cos(radX);

            float radY = rotation.y * Mathf.Deg2Rad;
            float sinY = Mathf.Sin(radY);
            float cosY = Mathf.Cos(radY);

            float radZ = rotation.z * Mathf.Deg2Rad;
            float sinZ = Mathf.Sin(radZ);
            float cosZ = Mathf.Cos(radZ);

            Vector3 xAxis = new Vector3(
                cosY * cosZ,
                cosX * sinZ + sinX * sinY * cosZ,
                sinX * sinZ - cosX * sinY * cosZ
                );
            Vector3 yAxis = new Vector3(
                -cosY * sinZ,
                cosX * cosZ - sinX * sinY * sinZ,
                sinX * cosZ + cosX * sinY * sinZ
                );
            Vector3 zAxis = new Vector3(
                sinY,
                -sinX * cosY,
                cosX * cosY
                );

            return point.x * xAxis +
                point.y * yAxis +
                point.z * zAxis;
        }
    }
}

