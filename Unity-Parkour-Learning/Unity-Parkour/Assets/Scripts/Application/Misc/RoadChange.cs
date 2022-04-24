/****************************************************
	文件：RoadChange.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class RoadChange : MonoBehaviour
{
    private GameObject mParentObj;
    private GameObject roadNow;
    private GameObject roadNext;

    private void Start()
    {
        if (mParentObj == null)
        {
            mParentObj = new GameObject();
            mParentObj.transform.position = Vector3.zero;
            mParentObj.name = "RoadParent";
        }
        
        roadNow = GetRandomRoad();
        roadNow.transform.position = Vector3.zero;
        roadNow.transform.SetParent(mParentObj.transform);

        roadNext = GetRandomRoad();
        roadNext.transform.position = roadNow.transform.position + new Vector3(0, 0, 160);
        roadNext.transform.SetParent(mParentObj.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagDefine.Road))
        {
            GameRoot.Instance.factoryManager.PushGameObjectToFactory("Road/" + roadNow.name.Replace("(Clone)", ""), roadNow);

            // 获取新道路
            GetNewRoad();
        }
    }

    private void GetNewRoad()
    {
        roadNow = roadNext;

        roadNext = GetRandomRoad();
        roadNext.transform.position = roadNow.transform.position + new Vector3(0, 0, 160);
        roadNext.transform.SetParent(mParentObj.transform);
    }

    private GameObject GetRandomRoad()
    {
        string newRoadName = "Pattern_" + Random.Range(1, 5).ToString();
        return GameRoot.Instance.factoryManager.GetGameObjectResource("Road/" + newRoadName);
    }
}
