/****************************************************
	文件：OfflineData.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class OfflineData : MonoBehaviour
{
	public Rigidbody m_Rigidbody;
	public Collider m_Collider;
	public Transform[] m_AllNode;
	public int[] m_AllNodeChildCount;
	public bool[] m_AllNodeActive;
	public Vector3[] m_Pos;
	public Vector3[] m_Scale;
	public Quaternion[] m_Rotate;

	/// <summary>
	/// 还原属性
	/// </summary>
	public virtual void ResetProp()
    {
		int allNodeCnt = m_AllNode.Length;
		for (int i = 0; i < allNodeCnt; i++)
        {
			var tmpTrs = m_AllNode[i];
			if (tmpTrs)
            {
				tmpTrs.localPosition = m_Pos[i];
				tmpTrs.localRotation = m_Rotate[i];
				tmpTrs.localScale = m_Scale[i];
				var active = m_AllNodeActive[i];
				if (active != tmpTrs.gameObject.activeSelf)
					tmpTrs.gameObject.SetActive(active);

				if (tmpTrs.childCount > m_AllNodeChildCount[i])
                {
					var childCnt = tmpTrs.childCount;
					for (int j = m_AllNodeChildCount[i]; j < childCnt; j++)
                    {
						var tmpObj = tmpTrs.GetChild(j).gameObject;
						if (!ObjectManager.Instance.IsObjectManagerCreate(tmpObj))
                        {
							GameObject.Destroy(tmpObj);
                        }
                    }
                }
            }

			
        }
    }

	/// <summary>
	/// 编辑器下保存初始数据
	/// </summary>
	public virtual void BindData()
    {
		m_Rigidbody = GetComponentInChildren<Rigidbody>(true);
		m_Collider = GetComponentInChildren<Collider>(true);
		m_AllNode = GetComponentsInChildren<Transform>(true);
		int allNodeCnt = m_AllNode.Length;
		m_AllNodeChildCount = new int[allNodeCnt];
		m_AllNodeActive = new bool[allNodeCnt];
		m_Pos = new Vector3[allNodeCnt];
		m_Scale = new Vector3[allNodeCnt];
		m_Rotate = new Quaternion[allNodeCnt];
		for (int i = 0; i < allNodeCnt; i++)
        {
			Transform tmp = m_AllNode[i];
			m_AllNodeChildCount[i] = tmp.childCount;
			m_AllNodeActive[i] = tmp.gameObject.activeSelf;
			m_Pos[i] = tmp.localPosition;
			m_Rotate[i] = tmp.localRotation;
			m_Scale[i] = tmp.localScale;
        }
    }
}
