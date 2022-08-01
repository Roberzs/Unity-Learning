/****************************************************
	文件：UIPause.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class UIPause : View
{
    public override string Name => StringDefine.V_UIPause;

    public override void HandleEvent(string name, object data)
    {
        throw new System.NotImplementedException();
    }

    #region Func
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    #endregion
}
