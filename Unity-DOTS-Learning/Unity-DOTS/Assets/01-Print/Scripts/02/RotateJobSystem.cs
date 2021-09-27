/****************************************************
    文件：RotateJobSystem.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;


public class RotateJobSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        return Entities.ForEach((ref RotationEulerXYZ rotationEulerXYZ, in RotateComponentData rotateComponentData) =>
                {
                    rotationEulerXYZ.Value.y += rotateComponentData.radiansPerSecond * deltaTime;
                })
                .WithBurst()
                .Schedule(inputDeps);
    }
}
