/****************************************************
    文件：DP14Visitor.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/24 15:52:44
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class DP14Visitor:MonoBehaviour
{
    private void Start()
    {
        DPShpere shpere1 = new DPShpere();
        DPCylinder cylinder1 = new DPCylinder();
        DPCube cube1 = new DPCube();

        DPShapeContainer container = new DPShapeContainer();
        container.AddShape(shpere1);
        container.AddShape(cylinder1);
        container.AddShape(cube1);

        AmountVisitor amountVisitor = new AmountVisitor();
        container.RunVisitor(amountVisitor);
        Debug.Log("图形个数共有:" + amountVisitor.amount);
    }
}

class DPShapeContainer
{
    private List<IDPShape> mShapes = new List<IDPShape>();

    public void AddShape(IDPShape shape)
    {
        mShapes.Add(shape);
    }

    public void RunVisitor(IShapeVisitor visitor)
    {
        foreach (var shape in mShapes)
        {
            shape.RunVisitor(visitor);
        }
    }
}

/** 图形基类与各种图形 */
abstract class IDPShape
{
    public abstract void RunVisitor(IShapeVisitor visitor);
}

class DPShpere : IDPShape
{
    public override void RunVisitor(IShapeVisitor visitor)
    {
        visitor.VisitSphere(this);
    }
}

class DPCylinder : IDPShape
{
    public override void RunVisitor(IShapeVisitor visitor)
    {
        visitor.VisitCylinder(this);
    }
}

class DPCube : IDPShape
{
    public override void RunVisitor(IShapeVisitor visitor)
    {
        visitor.VisitCube(this);
    }
}


/** 访问者基类 */
abstract class IShapeVisitor
{
    public abstract void VisitSphere(DPShpere shpere);
    public abstract void VisitCylinder(DPCylinder cylinder);
    public abstract void VisitCube(DPCube cube);
}

// 访问者; 用于计算所有图形的个数
class AmountVisitor : IShapeVisitor
{
    public int amount;

    public override void VisitCube(DPCube cube)
    {
        amount += 1;
    }

    public override void VisitCylinder(DPCylinder cylinder)
    {
        amount += 1;
    }

    public override void VisitSphere(DPShpere shpere)
    {
        amount += 1;
    }
}