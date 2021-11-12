using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    private void Awake()
    {
        AssetBundleManager.Instance.LoadAssetBundleConfig();
        ResourceItem item = AssetBundleManager.Instance.LoadResourceAssetBundle(1330568117);
        GameObject obj = GameObject.Instantiate(item.m_AssetBundle.LoadAsset<GameObject>("attack"));
    }
}
