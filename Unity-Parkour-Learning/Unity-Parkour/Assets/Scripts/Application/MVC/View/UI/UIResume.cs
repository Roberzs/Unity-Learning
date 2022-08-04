/****************************************************
	文件：UIResume.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIResume : View
{
    public override string Name => StringDefine.V_UIResume;

    public Image imgCounter;
    public Sprite[] spCounters = new Sprite[3];

    public override void HandleEvent(string name, object data)
    {
        throw new System.NotImplementedException();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void StartCounter()
    {
        Show();
        StartCoroutine(StartCounterCor());
    }

    IEnumerator StartCounterCor()
    {
        int i = 3;
        while (i > 0)
        {
            imgCounter.sprite = spCounters[i - 1];
            imgCounter.SetNativeSize();
            yield return new WaitForSeconds(1.0f);
            i--;
        }
        Hide();

        // TODO
        GetModel<GameModel>().IsPause = false;
    }
}
