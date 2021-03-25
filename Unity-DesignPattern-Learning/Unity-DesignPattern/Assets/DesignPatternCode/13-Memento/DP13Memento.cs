/****************************************************
    文件：DP13Memento.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/24 13:33:45
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class DP13Memento:MonoBehaviour
{
    private void Start()
    {
        CareTaker careTaker = new CareTaker();

        Originator originator = new Originator();
        originator.SetState("State1");
        originator.ShowState();
        careTaker.AddMemento("v1.0", originator.CreateMemento());

        originator.SetState("State2");
        originator.ShowState();
        careTaker.AddMemento("v2.0", originator.CreateMemento());

        Memento memento = careTaker.GetMemento("v1.0");
        originator.SetMemento(memento);
        originator.ShowState();
    }
}

class Originator
{
    private string mState;
    public void SetState(String state)
    {
        mState = state;
    }

    public void ShowState()
    {
        Debug.Log("Originator State" + mState);
    }

    public Memento CreateMemento()
    {
        Memento memento = new Memento();
        memento.SetState(mState);
        return memento;
    }

    public void SetMemento (Memento memento)
    {
        SetState(memento.GetState(memento));
    }
}

class Memento
{
    private string mState;

    public void SetState(string state)
    {
        mState = state;
    }

    public string GetState(Memento memento)
    {
        return mState;
    }
}

class CareTaker
{
    Dictionary<string, Memento> mMementoDict = new Dictionary<string, Memento>();

    public void AddMemento(string version, Memento memento)
    {
        mMementoDict.Add(version, memento);
    }

    public Memento GetMemento(string version)
    {
        if (mMementoDict.ContainsKey(version) == false)
        {
            Debug.LogError("快照字典中不包含key:" + version); return null;
        }
        return mMementoDict[version];
    }
}