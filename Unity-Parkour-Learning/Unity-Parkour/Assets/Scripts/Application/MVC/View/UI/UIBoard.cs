/****************************************************
	文件：UIBoard.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class UIBoard : View
{
    public override string Name => StringDefine.V_UIBoard;
    

    public override void HandleEvent(string name, object data)
    {
        switch (name)
        {
            case StringDefine.E_UpdateDis:
                DistanceArgs e = data as DistanceArgs;
                Distance = e.distance;
                break;
            default:
                break;
        }
    }

    public override void RegisterAttentionEvent()
    {
        AttentionList.Add(StringDefine.E_UpdateDis);
    }

    #region Field
    int m_Coin = 0;
    int m_Distance = 0;

    public Text txtCoin;
    public Text txtDistance;
    #endregion

    // 略: 封装属性快捷键 Ctrl + R,E

    public int Coin { get => m_Coin; set { m_Coin = value; txtCoin.text = value.ToString(); } }
    public int Distance { get => m_Distance; set {  m_Distance = value; txtDistance.text = value.ToString() + "米"; } }
}
