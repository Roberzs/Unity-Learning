using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommonConfirm : BaseItem
{
    [SerializeField]
    private Text txt_Title;
    [SerializeField]
    private Text txt_Content;
    [SerializeField]
    private Button btn_Confirm;
    [SerializeField]
    private Button btn_Cancle;

    public void OnShow(string title, string content, UnityAction confirmAction, UnityAction cancleAction)
    {
        txt_Title.text = title;
        txt_Content.text = content;
        AddButtonClickListener(btn_Confirm, ()=> 
        {
            confirmAction();
            Destroy(gameObject);
        });
        AddButtonClickListener(btn_Cancle, () =>
        {
            cancleAction();
            Destroy(gameObject);
        });
    }
}
