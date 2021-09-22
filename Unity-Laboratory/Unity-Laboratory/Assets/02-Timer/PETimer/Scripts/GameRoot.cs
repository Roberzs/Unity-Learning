/****************************************************
    文件：GameRoot.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    private int mTid;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            mTid = PETimerSys.Instance.AddTimeTask(FuncA, 1f, PETImeUnit.Second, 0);
            Debug.Log("Start Task Done.");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            bool isDel = PETimerSys.Instance.ReplaceTimeTask(mTid, FuncB, 2f, PETImeUnit.Second, 2);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            bool isDel = PETimerSys.Instance.DeleteTimeTask(mTid);
            Debug.Log("Cancel Task " + mTid + " " + (isDel ? "Succeed" : "Failed"));
        }
    }

    private void FuncA()
    {
        Debug.Log("FuncA Executing, Tid:" + mTid);
    }

    private void FuncB()
    {
        Debug.Log("FuncB Executing, Tid:" + mTid);
    }
}
