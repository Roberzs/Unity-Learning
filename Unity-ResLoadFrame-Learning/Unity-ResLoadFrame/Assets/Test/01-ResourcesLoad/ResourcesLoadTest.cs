/****************************************************
    文件：ResourcesLoadTest.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

#if UNITY_EDITOR
namespace ResLoadFrame.Test
{
    public class ResourcesLoadTest : MonoBehaviour
    {
        private void Start()
        {
            // AB资源加载 
            //LoadAssetBundleRes();

            // 在做编辑器扩展时可使用此资源加载方式
            LoadEditorLoadRes();
        }

        /// <summary>
        /// 加载AB包资源
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="resName">资源名</param>
        private void LoadAssetBundleRes()
        {
            AssetBundle configAB = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/assetbundleconfig");
            TextAsset textAsset = configAB.LoadAsset<TextAsset>("AssetBundleConfig");

            MemoryStream stream = new MemoryStream(textAsset.bytes);
            BinaryFormatter bf = new BinaryFormatter();
            AssetBundleConfig data = bf.Deserialize(stream) as AssetBundleConfig;

            string path = "Assets/GameData/Prefabs/Attack.prefab";
            uint crc = CRC32.GetCRC32(path);
            ABBase abBase = null;
            for (int i = 0; i < data.ABList.Count; i++)
            {
                if (data.ABList[i].Crc == crc)
                {
                    abBase = data.ABList[i];
                    break;
                }
            }
            foreach (var item in abBase.ABDependance)
            {
                AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + item);
            }
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + abBase.ABName);
            GameObject obj = GameObject.Instantiate(assetBundle.LoadAsset<GameObject>(abBase.AssetName));
        }

        private void LoadEditorLoadRes()
        {
            GameObject obj = GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameData/Prefabs/Attack.prefab"));
        }
    }
}
#endif

