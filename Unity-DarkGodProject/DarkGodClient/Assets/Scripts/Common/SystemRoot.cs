/****************************************************
    文件：SystemRoot.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/11/26 22:7:9
	功能：Nothing
*****************************************************/

using UnityEngine;

public class SystemRoot : MonoBehaviour 
{
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;
    protected NetSvc netSvc;
    protected TimerSvc timerSvc;

    public virtual void InitSys()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
        timerSvc = TimerSvc.Instance;
    }
}