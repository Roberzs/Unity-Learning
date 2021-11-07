using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ABConfig", menuName ="CreateABConfig", order = 0)]
public class ABConfig : ScriptableObject
{
    // 注意: 这里会将所有文件夹中的Prefab资源逐一打包, 所以要求所有Prefab不能重名
    public List<string> m_AllPrefabPath = new List<string>();
    public List<FileDirABName> m_AllFileDirAB = new List<FileDirABName>();

    [System.Serializable]
    public struct FileDirABName
    {
        public string ABName;
        public string Path;
    }
}
