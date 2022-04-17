/****************************************************
	文件：EffectOfflineData.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class EffectOfflineData : OfflineData
{
	public ParticleSystem[] m_Particle;
	public TrailRenderer[] m_Trail;

    public override void ResetProp()
    {
        base.ResetProp();
        foreach (var item in m_Particle)
        {
            item.Clear(true);
            item.Play();
        }
        foreach (var item in m_Trail)
        {
            item.Clear();
        }
    }

    public override void BindData()
    {
        base.BindData();
        m_Particle = GetComponentsInChildren<ParticleSystem>(true);
        m_Trail = GetComponentsInChildren<TrailRenderer>(true);
    }
}
