/****************************************************
    文件：DP12Observer.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/23 14:19:11
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class DP12Observer: MonoBehaviour
{
    private void Start()
    {
        ConcreteSubject1 sub1 = new ConcreteSubject1();
        ConcreteObserver1 ob1 = new ConcreteObserver1(sub1);

        sub1.RegisterObserver(ob1);

        sub1.SubjectState = "更新了一条消息";
    }
}

public abstract class Subject
{
    List<Observer> mObservers = new List<Observer>();

    public void RegisterObserver(Observer ob)
    {
        mObservers.Add(ob);
    }

    public void RemoveObserver(Observer ob)
    {
        mObservers.Remove(ob);
    }

    public void NotifyObserver()
    {
        foreach (var ob in mObservers)
        {
            ob.Update();
        }
    }
}

public class ConcreteSubject1: Subject
{
    private string mSubjectState;
    public string SubjectState
    {
        set
        {
            mSubjectState = value;
            NotifyObserver();
        }
        get
        {
            return mSubjectState;
        }
    }
}

public abstract class Observer
{
    public abstract void Update();
}

public class ConcreteObserver1 : Observer
{
    public ConcreteSubject1 mSub;
    public ConcreteObserver1(ConcreteSubject1 sub)
    {
        mSub = sub;
    }

    public override void Update()
    {
        Debug.Log("Observer1更新显示:" + mSub.SubjectState);
    }
}