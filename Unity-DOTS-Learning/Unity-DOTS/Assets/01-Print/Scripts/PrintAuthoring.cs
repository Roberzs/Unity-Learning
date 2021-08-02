using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class PrintAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    [SerializeField] private float printData;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new PrintComponentData() { printData = this.printData});
    }
}
