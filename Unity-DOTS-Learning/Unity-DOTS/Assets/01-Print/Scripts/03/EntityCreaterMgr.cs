/****************************************************
    文件：EntityCreaterMgr.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class EntityCreaterMgr : MonoBehaviour
{
    [SerializeField]
    private float interval = 1.5f;

    [SerializeField]
    private int size = 100;

    [SerializeField]
    private GameObject cubeEntity;

    private void Start()
    {
        float startTime = Time.realtimeSinceStartup;
        GameObjectConversionSettings tmpSettings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        Entity cubeEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(cubeEntity, tmpSettings);
        EntityManager tmpEntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        Translation tmpTranslation = new Translation();
        tmpTranslation.Value.x = 0f;
        tmpTranslation.Value.y = 0f;
        tmpTranslation.Value.z = 0f;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    Entity tmpCube = tmpEntityManager.Instantiate(cubeEntityPrefab);
                    tmpEntityManager.SetComponentData(tmpCube, tmpTranslation);
                    tmpTranslation.Value.z += interval;
                }
                tmpTranslation.Value.y += interval;
                tmpTranslation.Value.z = 0;
            }

            tmpTranslation.Value.x += interval;
            tmpTranslation.Value.y = 0f;
        }
        Debug.Log((Time.realtimeSinceStartup - startTime));
    }
}