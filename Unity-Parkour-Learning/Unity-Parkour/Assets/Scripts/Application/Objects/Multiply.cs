/****************************************************
	文件：Multiply.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class Multiply : Item
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void HitPlayer(Transform player)
    {
        base.HitPlayer(player);

        PlayEffect("FX_XingXing", player.position);
        GameRoot.Instance.soundManager.PlayEffectAudio("Se_UI_JinBi");

        //player.SendMessage("HitMultiply", SendMessageOptions.DontRequireReceiver);
        player.SendMessage("HitItem", ItemKind.ItemMultiply, SendMessageOptions.DontRequireReceiver);
    }


    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
