/****************************************************
    文件：DP08Builder.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/10 22:45:29
    功能：建造者模式示例
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

/**
 *      与工厂模式不同 工厂模式注重整体对象的创建 而建造者模式更注重部件构造的过程
 */

public class DP08Builder: MonoBehaviour
{
    private void Start()
    {
        IBuilder fatBuilder = new FatPersonBuilder();
        IBuilder thinBuilder = new ThinPersonBuilder();

        Person fatPerson = Director.Construct(fatBuilder);
        fatPerson.Show();
    }
}

/** 产品类 */
class Person
{
    List<string> parts = new List<string>();

    public void AddPart(string part)
    {
        parts.Add(part);
    }

    public void Show()
    {
        foreach (var part in parts)
        {
            Debug.Log(part);
        }
    }
}

class FatPerson: Person { }
class ThinPerson: Person { }

/** 建造者类 */
interface IBuilder
{
    void AddHead();
    void AddBody();
    void AddHand();
    void AddFeet();
    Person GetResult();
}

class FatPersonBuilder : IBuilder
{
    private Person person;

    public FatPersonBuilder()
    {
        person = new FatPerson();
    }

    public void AddBody()
    {
        person.AddPart("胖人的身体");
    }

    public void AddFeet()
    {
        person.AddPart("胖人的脚");
    }

    public void AddHand()
    {
        person.AddPart("胖人的手");
    }

    public void AddHead()
    {
        person.AddPart("胖人的头");
    }

    public Person GetResult()
    {
        return person;
    }
}
class ThinPersonBuilder : IBuilder
{
    private Person person;

    public ThinPersonBuilder()
    {
        person = new ThinPerson();
    }

    public void AddBody()
    {
        person.AddPart("瘦人的身体");
    }

    public void AddFeet()
    {
        person.AddPart("瘦人的脚");
    }

    public void AddHand()
    {
        person.AddPart("瘦人的手");
    }

    public void AddHead()
    {
        person.AddPart("瘦人的头");
    }

    public Person GetResult()
    {
        return person;
    }
}

/** 工程师 */
class Director
{
    public static Person Construct(IBuilder builder)
    {
        builder.AddBody();
        builder.AddFeet();
        builder.AddHand();
        builder.AddFeet();
        return builder.GetResult();
    }
}