/****************************************************
    文件：NewBehaviourScript.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：桥接模式示例
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class DP05Bridge : MonoBehaviour 
{

    private void Start()
    {
        //IRenderEngine renderEngine = new OpenGL();
        IRenderEngine renderEngine = new DirectX();

        Sphere sphere = new Sphere(renderEngine);
        sphere.Draw();

        Cube cube = new Cube(renderEngine);
        cube.Draw();
    }
    
    
}

/** 将抽象与实现分离 */

#region - BridgeMode

/** 图形基类 */
public class IShape
{
    public string name;
    public IRenderEngine renderEngine;

    public IShape(IRenderEngine renderEngine)
    {
        this.renderEngine = renderEngine;
    }

    public void Draw()
    {
        renderEngine.Render(name);
    }
}

/** 渲染引擎基类 */
public abstract class IRenderEngine
{
    public abstract void Render(string name);
}

/** 图形类 */
public class Sphere : IShape
{
    public Sphere(IRenderEngine re):base(re)
    {
        name = "Sphere";
    }
}

public class Cube : IShape
{
    public Cube(IRenderEngine re) : base(re)
    {
        name = "Cube";
    }
}


/** 引擎类 */
public class OpenGL:IRenderEngine
{
    public override void Render(string name)
    {
        Debug.Log("OpenGL已绘制: " + name);
    }
}

public class DirectX : IRenderEngine
{
    public override void Render(string name)
    {
        Debug.Log("DirectX已绘制: " + name);
    }
}

#endregion