/****************************************************
    文件：DP11ChainOfResponsibility.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/22 12:29:13
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class DP11ChainOfResponsibility: MonoBehaviour
{
    private void Start()
    {
        char problem = 'a';

        DPHandlerA handlerA = new DPHandlerA();
        DPHandlerB handlerB = new DPHandlerB();

        handlerA.SetNextHandler(handlerB);

        handlerA.Handle(problem);
    }
}

public abstract class IDPHandler
{
    protected IDPHandler mNextHandler = null;
    public IDPHandler nextHandler
    {
        set { mNextHandler = value; }
    }
    public IDPHandler SetNextHandler(IDPHandler handler)
    {
        mNextHandler = handler;
        return mNextHandler;
    }
    public virtual void Handle(char problem) { }
}

public class DPHandlerA: IDPHandler
{
    public override void Handle(char problem)
    {
        if (problem == 'a')
        {
            Debug.Log("问题A已处理");
        }
        else
        {
            if (mNextHandler != null)
            {
                mNextHandler.Handle(problem);
            }
        }
    }
}

public class DPHandlerB : IDPHandler
{
    public override void Handle(char problem)
    {
        if (problem == 'b')
        {
            Debug.Log("问题B已处理");
        }
        else
        {
            if (mNextHandler != null)
            {
                mNextHandler.Handle(problem);
            }
        }
    }
}