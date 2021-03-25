using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// 角色类基类
public abstract class ICharacter
{
    protected ICharacterAttr mAttr;     // 角色属性

    protected GameObject mGameObject;       // 角色模型
    protected NavMeshAgent mNavAgent;       // 角色导航组件
    protected Animation mAnim;              // 角色动画组件
    protected AudioSource mAudio;       // 播放音效
    protected IWeapon mWeapon;      // 角色武器

    public GameObject GameObject        // 设置物体
    {
        set
        {
            mGameObject = value;
            mNavAgent = mGameObject.GetComponent<NavMeshAgent>();
            mAudio = mGameObject.GetComponent<AudioSource>();
            mAnim = mGameObject.GetComponentInChildren<Animation>();
        }
    }

    public ICharacterAttr Attr { set { mAttr = value; } }       // 设置属性

    public IWeapon Weapon       // 设置武器以及武器拥有者
    {
        set 
        { 
            mWeapon = value;
            mWeapon.Owner = this;

            GameObject child = UnityTool.FindChild(mGameObject, "weapon-point");
            UnityTool.Attach(child, mWeapon.GameObject);
        } 
    }     

    public float AtkRang { get { return mWeapon.AtkRange; } }

    public abstract void UpdateFSMAI(List<ICharacter> targets);

    public Vector3 Position
    {
        get
        {
            if (mGameObject == null)
            {
                Debug.LogError("该对象为空"); return Vector3.zero;
            }
            return mGameObject.transform.position;
        }
    }

    public void Update()
    {
        mWeapon.Update();
    }

    public void Attack(ICharacter target)
    {
        mWeapon.Fire(target.Position);
        mGameObject.transform.LookAt(target.Position);      // 朝向目标位置
        PlayAnim("attack");
        target.UnderAttack(mWeapon.Atk + mAttr.CritValue);  
    }

    // 掉血效果
    public virtual void UnderAttack(int damage)
    {
        mAttr.TakeDamage(damage);


    }

    // 死亡处理
    public void Killed()
    {
        // TODO
    }

    public void PlayAnim(string animName)
    {
        mAnim.CrossFade(animName);
    }

    public void MoveTo(Vector3 targetPosition)
    {
        mNavAgent.SetDestination(targetPosition);
        PlayAnim("move");
    }

    // 播放特效
    protected void DoPlayEffect(string effectName)
    {
        // 加载特效 TODO
        GameObject effectGO = FactoryManager.AssetFactory.LoadEffect(effectName);
        effectGO.transform.position = Position;
        // 销毁特效 TODO
        effectGO.AddComponent<DestoryForTime>();
    }

    // 播放声音
    protected void DoPlaySound(string soundName)
    {
        AudioClip clip = FactoryManager.AssetFactory.LoadAudioClip(soundName);
        mAudio.clip = clip;
        mAudio.Play();
    }
}

