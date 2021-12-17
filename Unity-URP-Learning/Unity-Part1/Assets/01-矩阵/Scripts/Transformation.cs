/****************************************************
	文件：Transformation.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

namespace Part1_1
{
    public abstract class Transformation : MonoBehaviour
    {
		public abstract Matrix4x4 Matrix { get; }
		public abstract Vector3 Apple(Vector3 point);
    }
}

