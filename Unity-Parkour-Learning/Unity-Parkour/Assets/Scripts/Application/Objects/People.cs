/****************************************************
	文件：People.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class People : Obstacles
{
    private bool isHitTrigger = false;
    private bool isFlying = false;
    private float moveSpeed = 10f;
    private Animation m_Anim;

    protected override void Awake()
    {
        base.Awake();
        m_Anim = GetComponentInChildren<Animation>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        isHitTrigger = false;
        isFlying = false;
    }

    public override void HitPlayer()
    {
        // 播放特效
        GameObject itemEff = GameRoot.Instance.factoryManager.GetGameObjectResource("Effect/FX_ZhuangJi");
        itemEff.transform.position = transform.position;
        m_Anim.Play("fly");
        isHitTrigger = false;
        isFlying = true;
    }

    /// <summary>
    /// 激活角色进行移动
    /// </summary>
    public void HitTrigger()
    {
        isHitTrigger = true;
    }

    private void Update()
    {
        if (isHitTrigger)
        {
            transform.Translate(transform.right * moveSpeed * Time.deltaTime);
        }
        else if (isFlying)
        {
            transform.position += new Vector3(0, moveSpeed, moveSpeed) * Time.deltaTime;
        }
    }
}
