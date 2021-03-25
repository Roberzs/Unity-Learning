/****************************************************
    文件：IBaseFactory.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：游戏物体工厂接口
*****************************************************/

using UnityEngine;

public interface IBaseFactory 
{
    GameObject GetItem(string itemName);

    void PushItem(string itemName, GameObject item);
}