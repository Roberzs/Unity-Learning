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
    public void ClickAddTaskBtn()
    {
        PETimerSys.Instance.AddTimeTask(() => { Debug.Log("回调执行"); }, 2000f);
    }
}
