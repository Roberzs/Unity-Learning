/****************************************************
	文件：UIPause.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class UIPause : View
{
    public override string Name => StringDefine.V_UIPause;

    public Text txtScore;
    public Text txtDistance;
    public Text txtCoin;

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

    public void OnClickResumeBtn()
    {
        Hide();
        SendEvent(StringDefine.E_ResumeGame);
    }

    public void SetPauseData(PauseArgs e)
    {
        txtDistance.text = e.distance.ToString();
        txtScore.text = e.score.ToString();
        txtCoin.text = e.coin.ToString();
    }

    #endregion
}
