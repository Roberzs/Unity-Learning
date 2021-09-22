/****************************************************
    文件：PETimerSys.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：基于Plane老师的PETimer定时器 阉割了服务器端功能
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class PETimerSys: MonoBehaviour
{
    private static PETimerSys _instance;
    public static PETimerSys Instance => _instance;

    private readonly string objlock = "lock";
    private int tid;
    private List<int> tidLst = new List<int>();
    private List<int> recTidLst = new List<int>();

    /// <summary>
    /// 定时任务缓存列表
    /// </summary>
    /// 因可能会在其他线程添加Task 所以需要先将定时任务添加到缓存列表 错开操作时间 此操作可用以避免加锁
    private List<PETimeTask> tmpTimeLst = new List<PETimeTask>();
    /// <summary>
    /// 定时任务列表
    /// </summary>
    private List<PETimeTask> taskTimeLst = new List<PETimeTask>();

    private int frameCounter;
    private List<PEFrameTask> tmpFrameLst = new List<PEFrameTask>();
    private List<PEFrameTask> taskFrameLst = new List<PEFrameTask>();

    private void Awake()
    {
        _instance = this;
        Debug.Log("PETimerSys Init Done.");
    }

    private void Update()
    {
        CheckTimeTask();
        CheckFrameTask();

        // 清理定时任务ID
        if (recTidLst.Count > 0)
        {
            RecycleTid();
        }
    }

    private void CheckTimeTask()
    {
        // 将缓存区内容加入定时任务列表 
        foreach (var item in tmpTimeLst) taskTimeLst.Add(item);
        tmpTimeLst.Clear();

        // 遍历任务是否已达到条件
        for (int index = 0; index < taskTimeLst.Count; index++)
        {
            PETimeTask task = taskTimeLst[index];
            if (Time.realtimeSinceStartup * 1000 < task.destTime) continue;
            else
            {
                Action cb = task.callback;
                try
                {
                    cb?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }

                // 检查任务完成情况
                if (task.count == 1)
                {
                    taskTimeLst.RemoveAt(index);
                    index--;
                    recTidLst.Add(task.tid);
                }
                else
                {
                    if (task.count != 0)
                    {
                        task.count--;
                    }
                    task.destTime += task.delay;
                }
            }
        }
    }

    private void CheckFrameTask()
    {
        // 将缓存区内容加入定时任务列表 
        foreach (var item in tmpFrameLst) taskFrameLst.Add(item);
        tmpFrameLst.Clear();

        frameCounter++;

        // 遍历任务是否已达到条件
        for (int index = 0; index < taskFrameLst.Count; index++)
        {
            PEFrameTask task = taskFrameLst[index];
            if (frameCounter < task.destFrame) continue;
            else
            {
                Action cb = task.callback;
                try
                {
                    cb?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }

                // 检查任务完成情况
                if (task.count == 1)
                {
                    taskFrameLst.RemoveAt(index);
                    index--;
                    recTidLst.Add(task.tid);
                }
                else
                {
                    if (task.count != 0)
                    {
                        task.count--;
                    }
                    task.destFrame += task.delay;
                }
            }
        }

    }

    #region TimeTask
    public int AddTimeTask(Action callback, float delay, PETImeUnit timeUnit = PETImeUnit.Millsecond, int count = 1)
    {
        if (timeUnit != PETImeUnit.Millsecond)
        {
            switch (timeUnit)
            {
                case PETImeUnit.Second:
                    delay = delay * 1000;
                    break;
                case PETImeUnit.Minute:
                    delay = delay * 1000 * 60;
                    break;
                case PETImeUnit.Hour:
                    delay = delay * 1000 * 60 * 60;
                    break;
                case PETImeUnit.Day:
                    delay = delay * 1000 * 60 * 60 * 24;
                    break;
            }
        }

        float destTime = Time.realtimeSinceStartup * 1000 + delay;
        int tid = GetTid();
        tmpTimeLst.Add(new PETimeTask(tid, callback, destTime, delay, count));
        tidLst.Add(tid);
        return tid;
    }

    public bool DeleteTimeTask(int tid)
    {
        bool exist = false;
        for (int i = 0; i < taskTimeLst.Count; i++)
        {
            PETimeTask task = taskTimeLst[i];
            if (task.tid == tid)
            {
                taskTimeLst.RemoveAt(i);
                for (int j = 0; j < tidLst.Count; j++)
                {
                    if (tidLst[j] == tid)
                    {
                        tidLst.RemoveAt(j);
                        exist = true;
                        break;
                    }
                }
            }
        }
        if (!exist)
        {
            for (int i = 0; i < tmpTimeLst.Count; i++)
            {
                PETimeTask task = tmpTimeLst[i];
                if (task.tid == tid)
                {
                    tmpTimeLst.RemoveAt(i);
                    for (int j = 0; j < tidLst.Count; j++)
                    {
                        if (tidLst[j] == tid)
                        {
                            tidLst.RemoveAt(j);
                            exist = true;
                            break;
                        }
                    }
                }
            }
        }
        return exist;
    }

    public bool ReplaceTimeTask(int tid, Action callback, float delay, PETImeUnit timeUnit = PETImeUnit.Millsecond, int count = 1)
    {
        if (timeUnit != PETImeUnit.Millsecond)
        {
            switch (timeUnit)
            {
                case PETImeUnit.Second:
                    delay = delay * 1000;
                    break;
                case PETImeUnit.Minute:
                    delay = delay * 1000 * 60;
                    break;
                case PETImeUnit.Hour:
                    delay = delay * 1000 * 60 * 60;
                    break;
                case PETImeUnit.Day:
                    delay = delay * 1000 * 60 * 60 * 24;
                    break;
            }
        }
        float destTime = Time.realtimeSinceStartup * 1000 + delay;
        PETimeTask newTask = new PETimeTask(tid, callback, destTime, delay, count);

        bool isRep = false;
        for (int i = 0; i < taskTimeLst.Count; i++)
        {
            if (taskTimeLst[i].tid == tid)
            {
                taskTimeLst[i] = newTask;
                isRep = true;
                break;
            }
        }
        if (!isRep)
        {
            for (int i = 0; i < tmpTimeLst.Count; i++)
            {
                if (tmpTimeLst[i].tid == tid)
                {
                    tmpTimeLst[i] = newTask;
                    isRep = true;
                    break;
                }
            }
        }
        return isRep;
    }
    #endregion

    #region FrameTask
    public int AddFrameTask(Action callback, int delay, int count = 1)
    {
        int destFrame = frameCounter + delay;
        int tid = GetTid();
        tmpFrameLst.Add(new PEFrameTask(tid, callback, destFrame, delay, count));
        tidLst.Add(tid);
        return tid;
    }

    public bool DeleteFrameTask(int tid)
    {
        bool exist = false;
        for (int i = 0; i < taskFrameLst.Count; i++)
        {
            PEFrameTask task = taskFrameLst[i];
            if (task.tid == tid)
            {
                taskFrameLst.RemoveAt(i);
                for (int j = 0; j < tidLst.Count; j++)
                {
                    if (tidLst[j] == tid)
                    {
                        tidLst.RemoveAt(j);
                        exist = true;
                        break;
                    }
                }
            }
        }
        if (!exist)
        {
            for (int i = 0; i < tmpFrameLst.Count; i++)
            {
                PEFrameTask task = tmpFrameLst[i];
                if (task.tid == tid)
                {
                    tmpFrameLst.RemoveAt(i);
                    for (int j = 0; j < tidLst.Count; j++)
                    {
                        if (tidLst[j] == tid)
                        {
                            tidLst.RemoveAt(j);
                            exist = true;
                            break;
                        }
                    }
                }
            }
        }
        return exist;
    }

    public bool ReplaceFrameTask(int tid, Action callback, int delay, int count = 1)
    {
        int destFrame = frameCounter + delay;
        PEFrameTask newTask = new PEFrameTask(tid, callback, destFrame, delay, count);

        bool isRep = false;
        for (int i = 0; i < taskFrameLst.Count; i++)
        {
            if (taskFrameLst[i].tid == tid)
            {
                taskFrameLst[i] = newTask;
                isRep = true;
                break;
            }
        }
        if (!isRep)
        {
            for (int i = 0; i < tmpFrameLst.Count; i++)
            {
                if (tmpFrameLst[i].tid == tid)
                {
                    tmpFrameLst[i] = newTask;
                    isRep = true;
                    break;
                }
            }
        }
        return isRep;
    }
    #endregion

    private int GetTid()
    {
        lock (objlock)
        {
            tid += 1;
            // 安全校验
            while (true)
            {
                if (tid == int.MaxValue) tid = 0;

                bool used = false;
                for (int index = 0; index < tidLst.Count; index++)
                {
                    if (tid == tidLst[index])
                    {
                        used = true;
                        break;
                    }
                }
                if (!used)
                {
                    break;
                }
                else
                {
                    tid += 1;
                }
            }
        }
        return tid;
    }

    private void RecycleTid()
    {
        for (int i = 0; i < recTidLst.Count; i++)
        {
            int tid = recTidLst[i];
            for (int j = 0; j < tidLst.Count; j++)
            {
                if (tidLst[j] == tid)
                {
                    tidLst.RemoveAt(j);
                    break;
                }
            } 
        }
        recTidLst.Clear();
    }
}

/// <summary>
/// 定时任务数据类
/// </summary>
class PETimeTask
{
    public int tid;
    /// <summary>
    /// 定时完成时间(单位毫秒)
    /// </summary>
    public float destTime;
    /// <summary>
    /// 延时时长
    /// </summary>
    public float delay;
    /// <summary>
    /// 回调方法
    /// </summary>
    public Action callback;
    /// <summary>
    /// 执行次数 0为持续循环
    /// </summary>
    public int count;

    public PETimeTask(int tid,Action callback, float destTime, float delay, int count)
    {
        this.tid = tid;
        this.callback = callback;
        this.destTime = destTime;
        this.delay = delay;
        this.count = count;
    }
}

/// <summary>
/// 帧定时任务数据类
/// </summary>
class PEFrameTask
{
    public int tid;
    /// <summary>
    /// 定时完成时间(单位毫秒)
    /// </summary>
    public int destFrame;
    /// <summary>
    /// 延时时长
    /// </summary>
    public int delay;
    /// <summary>
    /// 回调方法
    /// </summary>
    public Action callback;
    /// <summary>
    /// 执行次数 0为持续循环
    /// </summary>
    public int count;

    public PEFrameTask(int tid, Action callback, int destFrame, int delay, int count)
    {
        this.tid = tid;
        this.callback = callback;
        this.destFrame = destFrame;
        this.delay = delay;
        this.count = count;
    }
}

/// <summary>
/// 时间单位
/// </summary>
public enum PETImeUnit
{
    Millsecond,
    Second,
    Minute,
    Hour,
    Day
}