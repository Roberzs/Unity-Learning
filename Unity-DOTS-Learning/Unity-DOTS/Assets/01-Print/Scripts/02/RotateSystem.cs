/****************************************************
    文件：RotateSystem.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class RotateSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref RotationEulerXYZ rotationEulerXYZ, ref RotateComponentData rotateComponentData) =>
        {
            rotationEulerXYZ.Value.y += rotateComponentData.radiansPerSecond * Time.DeltaTime;
        });
    }
}
