using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetText", menuName = "CreateAsset",order = 0)]
public class AssetsSerialize : ScriptableObject
{
    public int Id;

    public string Name;

    public AssetsScoreTemplate Score;

    public List<string> Hobby;
}

[System.Serializable]
public class AssetsScoreTemplate
{
    public int Mathematics;

    public int English;
}