/****************************************************
    文件：DM06Strategy.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class DP06Strategy : MonoBehaviour 
{
    private void Start()
    {
        StrategyContext context = new StrategyContext();
        context.strategy = new ConcreteStrategyA();

        context.Cal();
    }
}

#region - StrategyMode

/** 策略模式类拥有者 */
public class StrategyContext
{
    public IStrategy strategy;
    public void Cal()
    {
        strategy.Cal();
    }
}

/** 策略接口 */
public interface IStrategy
{
    void Cal();
}

/** 具体方法 */

public class ConcreteStrategyA : IStrategy
{
    public void Cal()
    {
        Debug.Log("已实现接口A");
    }
}

public class ConcreteStrategyB : IStrategy
{
    public void Cal()
    {
        Debug.Log("已实现接口B");
    }
}

#endregion