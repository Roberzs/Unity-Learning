/****************************************************
    文件：DP16Decorator.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/26 10:59:21
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class DP16Decorator: MonoBehaviour
{
    
    private void Start()
    {
        Coffee coffee = new Espresso();
        coffee = coffee.AddDecorator(new Mocha());
        coffee = coffee.AddDecorator(new Whip());

        Debug.Log(coffee.Cost());
    }

}

public abstract class Coffee
{
    public abstract double Cost();
    public Coffee AddDecorator(Decorator decorator)
    {
        decorator.Coffee = this;
        return decorator;
    }
}

public class Espresso : Coffee
{
    public override double Cost()
    {
        return 2.5;
    }
}

public class Decaf : Coffee
{
    public override double Cost()
    {
        return 2;
    }
}


public abstract class Decorator: Coffee
{
    protected Coffee mCoffee;
    public Coffee Coffee { set { mCoffee = value; } }

    public override double Cost()
    {
        return mCoffee.Cost();
    }
}

public class Mocha : Decorator
{

    public override double Cost()
    {
        return mCoffee.Cost() + 0.1;
    }
}

public class Whip : Decorator
{

    public override double Cost()
    {
        return mCoffee.Cost() + 0.5;
    }
}