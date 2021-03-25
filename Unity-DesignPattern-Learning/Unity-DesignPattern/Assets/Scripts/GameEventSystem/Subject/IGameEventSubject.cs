/****************************************************
    文件：IGameEventSubject.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/24 9:39:35
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class IGameEventSubject
{
    private List<IGameEventObserver> mObservers = new List<IGameEventObserver>();

    public void RegisterObserver(IGameEventObserver ob)
    {
        mObservers.Add(ob);
    }

    public void RemoveObserver(IGameEventObserver ob)
    {
        mObservers.Add(ob);
    }

    public virtual void Notify()
    {
        foreach (IGameEventObserver observer in mObservers)
        {
            observer.Update();
        }
    }
}
