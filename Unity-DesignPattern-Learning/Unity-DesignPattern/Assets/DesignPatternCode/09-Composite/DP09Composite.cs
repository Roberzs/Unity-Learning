/****************************************************
    文件：DP09Composite.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/16 23:46:27
    功能：组合模式示例
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class DP09Composite: MonoBehaviour
{
    private void Start()
    {
        DPComposite root = new DPComposite("Root");

        DPLeaf leaf1 = new DPLeaf("Leaf1");
        DPComposite composite = new DPComposite("composite1");
        DPLeaf leaf2 = new DPLeaf("Leaf2");

        root.AddChild(leaf1);
        root.AddChild(composite);
        root.AddChild(leaf2);

        DPLeaf leaf3 = new DPLeaf("Leaf3");
        DPLeaf leaf4 = new DPLeaf("Leaf4");
        composite.AddChild(leaf3);
        composite.AddChild(leaf4);

        ReadComponent(root);
    }

    // 深度搜索遍历
    private void ReadComponent(DPComponent component)
    {
        Debug.Log(component.Name);
        List<DPComponent> children = component.Children;
        if (children == null || children.Count == 0) return;
        foreach (var child in children)
        {
            ReadComponent(child);
        }
    }
}

public abstract class DPComponent
{
    protected string mName;
    protected List<DPComponent> mChildren;
    public string Name { get { return mName; } }
    public List<DPComponent> Children { get { return mChildren; } }
    public DPComponent(string name)
    {
        mName = name;
        mChildren = new List<DPComponent>();
    }

    public abstract void AddChild(DPComponent c);
    public abstract void RemoveChild(DPComponent c);
    public abstract DPComponent GetChild(int index);
}

public class DPLeaf : DPComponent
{
    public DPLeaf(string name): base(name) { }

    public override void AddChild(DPComponent c)
    {
        return;
    }

    public override DPComponent GetChild(int index)
    {
        return null;
    }

    public override void RemoveChild(DPComponent c)
    {
        return;
    }
}

public class DPComposite: DPComponent
{
    public DPComposite(string name) : base(name) { }

    public override void AddChild(DPComponent c)
    {
        mChildren.Add(c);
    }

    public override DPComponent GetChild(int index)
    {
        return mChildren[index];
    }

    public override void RemoveChild(DPComponent c)
    {
        mChildren.Remove(c);
    }
}
