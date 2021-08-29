/****************************************************
    文件：PETools.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2020/11/28 14:8:16
	功能：工具类
*****************************************************/

using UnityEngine;

public class PETools  
{
    public static int RdInt(int min, int max, System.Random rd = null)
    {
        if (rd == null) rd = new System.Random();
        int val = rd.Next(min, max + 1);
        return val;
    }
}