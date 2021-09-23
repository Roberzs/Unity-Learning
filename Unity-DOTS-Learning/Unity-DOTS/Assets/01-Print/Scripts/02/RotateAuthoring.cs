/****************************************************
    文件：RotateAuthoring.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class RotateAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    [SerializeField]
    private float degresPerSecond;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new RotateComponentData() { radiansPerSecond = math.radians(degresPerSecond) });
        dstManager.AddComponentData(entity, new RotationEulerXYZ());
    }
}