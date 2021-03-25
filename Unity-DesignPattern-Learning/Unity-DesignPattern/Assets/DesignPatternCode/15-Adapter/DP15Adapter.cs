/****************************************************
    文件：DP15Adapter.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/25 10:48:08
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class DP15Adapter: MonoBehaviour
{
    private void Start()
    {
        //StandardInterface si = new StandardImplementA();
        //si.Request();

        Adapter adapter = new Adapter(new NewPlugin());
        StandardInterface si = adapter;
        si.Request();
    }
}

/** 标准接口以及一个标准实现类 */
interface StandardInterface
{
    void Request();
}

class StandardImplementA : StandardInterface
{
    public void Request()
    {
        Debug.Log("标准方法实现请求");
    }
}

/** 新的实现类 */
class NewPlugin
{
    public void SpecificRequest()
    {
        Debug.Log("插件方法实现请求");
    }
}

/** 适配器 */
class Adapter : StandardInterface
{
    private NewPlugin mPlugin;

    public Adapter(NewPlugin plugin)
    {
        mPlugin = plugin;
    }

    public void Request()
    {
        mPlugin.SpecificRequest();
    }
}