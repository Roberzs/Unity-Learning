/****************************************************
    文件：TaskWnd.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/4/15 16:31:55
	功能：Nothing
*****************************************************/

using PEProtocol;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskWnd : WindowRoot
{
    public Transform scrollTrans;

    private PlayerData pd = null;

    private List<TaskRewardData> trdList = new List<TaskRewardData>();

    protected override void InitWnd()
    {
        base.InitWnd();

        pd = GameRoot.Instance.PlayerData;
        RefreshUI();
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

    public void RefreshUI()
    {
        trdList.Clear();

        for (int i = 0; i < scrollTrans.childCount; i++)
        {
            Destroy(scrollTrans.GetChild(i).gameObject);
        }

        List<TaskRewardData> todoList = new List<TaskRewardData>();
        List<TaskRewardData> doneList = new List<TaskRewardData>();

        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            TaskRewardData trd = new TaskRewardData
            {
                ID = int.Parse(taskInfo[0]),
                prgs = int.Parse(taskInfo[1]),
                taked = taskInfo[2].Equals("1")
            };
            if (trd.taked)
            {
                doneList.Add(trd);
            }
            else
            {
                todoList.Add(trd);
            }
        }

        trdList.AddRange(todoList);
        trdList.AddRange(doneList);

        for (int i = 0; i < trdList.Count; i++)
        {
            GameObject go = resSvc.LoadPrefab(PathDefine.TaskItemPrefab);
            go.transform.SetParent(scrollTrans);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.name = "taskItem_" + i;

            TaskRewardData trd = trdList[i];
            TaskRewardCfg trf = resSvc.GeTaskRewardData(trd.ID);

            SetText(GetTrans(go.transform, "txtName"), trf.taskName);
            //Debug.Log(trf.taskName);
            SetText(GetTrans(go.transform, "txtPrg"), trd.prgs + "/" + trf.count);
            SetText(GetTrans(go.transform, "txtExp"), "奖励：    经验" +  trf.exp);
            SetText(GetTrans(go.transform, "txtCoin"), "金币" + trf.coin);

            Image imgPrg = GetTrans(go.transform, "prgBar/prgVal").GetComponent<Image>();
            float prgVal = trd.prgs * 1.0f / trf.count;
            imgPrg.fillAmount = prgVal;

            Button btnTake = GetTrans(go.transform, "btnTake").GetComponent<Button>();
            btnTake.onClick.AddListener(() => {
                ClickTakeBtn(go.name);
            });

            Transform transComp = GetTrans(go.transform, "imgComp");
            if (trd.taked)
            {
                btnTake.interactable = false;
                SetActive(transComp);
            }
            else
            {
                SetActive(transComp, false);
                if (trd.prgs == trf.count)
                {
                    btnTake.interactable = true;
                }
                else
                {
                    btnTake.interactable = false;
                }
            }
        }
    }

    private void ClickTakeBtn(string name)
    {
        string[] nameArr = name.Split('_');
        int index = int.Parse(nameArr[1]);
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ReqTakeTaskReward,
            reqTakeTaskReward = new ReqTakeTaskReward
            {
                rid = trdList[index].ID,
            }
        };
        netSvc.SendMsg(msg);
    }
}