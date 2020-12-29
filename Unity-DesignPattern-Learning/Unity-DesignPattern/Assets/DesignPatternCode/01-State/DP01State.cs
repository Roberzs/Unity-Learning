using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DP01State: MonoBehaviour
{
    private void Start()
    {
        Context context = new Context();

        context.SetState(new ConcreteStateA(context));

        context.Handle(5);
        context.Handle(10);
        context.Handle(15);
        context.Handle(10);
        context.Handle(5);

    }
}

#region - StateMode

/**
 * 
 *          状态模式(State)：
 *          
 *          当一个对象的内在状态改变时允许改变其行为。这个对象看起来像是改变了其类。
 *          
 *          用途：当控制一个对象状态的条件表达式过于复杂时，把状态的判断逻辑转移到表示不同的一系列类中，可以把复杂的逻辑判断简单化。
 *          例如，主程序段有复杂的 switch-case 判断，可以使用到状态模式。将逻辑判断转移。
 * 
 *          [         ]             [          ]
 *          | Context | ----------> |   State  |
 *          [         ]             [          ]
 *                                        ^
 *                                        |
 *                                        |
 *                              ---------------------
 *                              |                   |
 *                              |                   |
 *                              |                   |
 *                     [                ]   [                ]
 *                     | ConcreteState1 ]   | ConcreteState2 ]
 *                     [                ]   [                ]
 *                     
 *          Context类： 环境角色。用于维护一个ConcreteState子类的实例，这个实例定义当前的状态
 *          State类： 抽象状态角色。定义一个接口以封装与Context的一个特定接口状态相关的行为。
 *          ConcreteState类： 具体状态对象。实现抽象状态定义的接口。
 *          
 */

/** 环境角色 */
public class Context
{
    private IState mState;
    public void SetState(IState state)
    {
        mState = state;
    }

    public void Handle(int arg)
    {
        mState.Handle(arg);
    }
}

/** 抽象状态角色 */
public interface IState
{
    void Handle(int arg);
}

/** 具体状态角色 */
public class ConcreteStateA: IState
{
    public Context mContext;
    public ConcreteStateA(Context context)
    {
        mContext = context;
    }

    public void Handle(int arg)
    {
        Debug.Log("ConcreteStateA.Handle " + arg);
        // 满足条件 切换状态
        if (arg > 10)
        {
            mContext.SetState(new ConcreteStateB(mContext));
        }
    }
}

public class ConcreteStateB : IState
{
    public Context mContext;
    public ConcreteStateB(Context context)
    {
        mContext = context;
    }

    public void Handle(int arg)
    {
        Debug.Log("ConcreteStateB.Handle " + arg);
        // 满足条件 切换状态
        if (!(arg > 10))
        {
            mContext.SetState(new ConcreteStateA(mContext));
        }
    }
}

#endregion