/****************************************************
    文件：IWeapon.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public enum WeaponType
{
    Gun,
    Rifle,
    Rocket,
    MAX_LV
}

public abstract class IWeapon 
{
    protected WeaponBaseAttr mBaseAttr;      // 基础属性

    protected GameObject mGameObject;
    protected ICharacter mOwner;       // 武器拥有者

    protected ParticleSystem mPariticle;
    protected LineRenderer mLine;
    protected Light mLight;
    protected AudioSource mAudio;

    protected float mEffectDisplayTime = 0;     // 特效显示的时间

    public ICharacter Owner
    {
        set
        {
            mOwner = value;
        }
    }

    public float AtkRange { get { return mBaseAttr.AtkRange; } }     // 返回武器攻击范围
    public int Atk { get { return mBaseAttr.Atk; } }     // 返回武器攻击力
    public GameObject GameObject { get { return mGameObject; } }    // 返回武器物体

    // -----
    public IWeapon(WeaponBaseAttr baseAttr, GameObject gameObject)
    {
        mBaseAttr = baseAttr;

        mGameObject = gameObject;

        Transform effect = mGameObject.transform.Find("Effect");
        mPariticle = effect.GetComponent<ParticleSystem>();
        mLine = effect.GetComponent<LineRenderer>();
        mLight = effect.GetComponent<Light>();
        mAudio = effect.GetComponent<AudioSource>();
    }

    // 检测特效状态
    public void Update()
    {
        if (mEffectDisplayTime > 0)
        {
            mEffectDisplayTime -= Time.deltaTime;
            if (mEffectDisplayTime <= 0)
            {
                DisableEffect();
            }
        }
    }

    // 向目标方向开火
    public void Fire(Vector3 targetPosition) 
    {
        // 枪口特效
        PlayMuzzleEffect();

        // 显示子弹轨迹特效
        PlayBulletEffect(targetPosition);

        // 设置特效显示时间
        SetEffectDisplayTime();

        // 播放声音
        PlaySound();
    }

    protected abstract void SetEffectDisplayTime();

    protected virtual void PlayMuzzleEffect()
    {
        mPariticle.Stop();
        mPariticle.Play();
        mLight.enabled = true;
    }

    protected abstract void PlayBulletEffect(Vector3 targetPosition);
    protected void DoPlayBulletEffect(float width,Vector3 targetPosition)
    {
        mLine.enabled = true;
        mLine.startWidth = width; mLine.endWidth = width;
        mLine.SetPosition(0, mGameObject.transform.position);
        mLine.SetPosition(1, targetPosition);
    }

    // 播放音效
    protected abstract void PlaySound();
    protected void DoPlaySound(string clipName)
    {
        AudioClip clip= FactoryManager.AssetFactory.LoadAudioClip(clipName);  

        mAudio.clip = clip;
        mAudio.Play();

    }

    // 关闭特效
    private void DisableEffect()
    {
        mLight.enabled = false;
        mLine.enabled = false;
    }
}