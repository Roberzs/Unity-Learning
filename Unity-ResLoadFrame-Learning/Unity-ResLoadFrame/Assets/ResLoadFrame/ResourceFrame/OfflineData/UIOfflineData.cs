/****************************************************
	文件：UIOfflineData.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class UIOfflineData : OfflineData
{
	public Vector2[] m_AnchorMax;
	public Vector2[] m_AnchorMin;
	public Vector2[] m_Pivot;
	public Vector2[] m_SizeDelta;
	public Vector3[] m_AnchoredPos;
	public ParticleSystem[] m_Particle;

    public override void ResetProp()
    {
        int allNodeCnt = m_AllNode.Length;
        for (int i = 0; i < allNodeCnt; i++)
        {
            var tmpTrs = m_AllNode[i] as RectTransform;
            if (tmpTrs)
            {
                tmpTrs.localPosition = m_Pos[i];
                tmpTrs.localRotation = m_Rotate[i];
                tmpTrs.localScale = m_Scale[i];

                tmpTrs.anchorMax = m_AnchorMax[i];
                tmpTrs.anchorMin= m_AnchorMin[i];
                tmpTrs.pivot = m_Pivot[i];
                tmpTrs.sizeDelta = m_SizeDelta[i];
                tmpTrs.anchoredPosition3D = m_AnchoredPos[i];

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

        int particleCnt = m_Particle.Length;
        for (int i = 0; i < particleCnt; i++)
        {
            m_Particle[i].Clear(true);
            m_Particle[i].Play();
        }
    }

    public override void BindData()
    {
		Transform[] allTrs = gameObject.GetComponentsInChildren<Transform>(true);
		int allTrsCnt = allTrs.Length;
		for (int i = 0; i < allTrsCnt; i++)
        {
			if (!(allTrs[i] is RectTransform))
            {
				allTrs[i].gameObject.AddComponent<RectTransform>();
            }
        }

        m_AllNode = GetComponentsInChildren<RectTransform>(true);
        int allNodeCnt = m_AllNode.Length;
        m_AllNodeChildCount = new int[allNodeCnt];
        m_AllNodeActive = new bool[allNodeCnt];
        m_Pos = new Vector3[allNodeCnt];
        m_Scale = new Vector3[allNodeCnt];
        m_Rotate = new Quaternion[allNodeCnt];

        m_AnchorMax = new Vector2[allNodeCnt];
        m_AnchorMin = new Vector2[allNodeCnt];
        m_Pivot = new Vector2[allNodeCnt];
        m_SizeDelta = new Vector2[allNodeCnt];
        m_AnchoredPos = new Vector3[allNodeCnt];
        m_Particle = GetComponentsInChildren<ParticleSystem>(true);

        for (int i = 0; i < allNodeCnt; i++)
        {
            RectTransform tmp = m_AllNode[i] as RectTransform;
            m_AllNodeChildCount[i] = tmp.childCount;
            m_AllNodeActive[i] = tmp.gameObject.activeSelf;
            m_Pos[i] = tmp.localPosition;
            m_Rotate[i] = tmp.localRotation;
            m_Scale[i] = tmp.localScale;

            m_AnchorMax[i] = tmp.anchorMax;
            m_AnchorMin[i] = tmp.anchorMin;
            m_Pivot[i] = tmp.pivot;
            m_SizeDelta[i] = tmp.sizeDelta;
            m_AnchoredPos[i] = tmp.anchoredPosition3D;
        }


    }
}

