using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class PrintSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref PrintComponentData printComponentData) =>
        {
            Debug.Log(printComponentData.printData);
        });
    }
}
