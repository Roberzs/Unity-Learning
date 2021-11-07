/****************************************************
    文件：ResourcesLoadTest.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

#if UNITY_EDITOR
namespace ResLoadFrame.Test
{
    public class ResourcesLoadTest : MonoBehaviour
    {
        private void Start()
        {
            // AB资源加载 
            //LoadAssetBundleRes("attack", "attack");

            // 在做编辑器扩展时可使用此资源加载方式
            LoadEditorLoadRes();
        }

        /// <summary>
        /// 加载AB包资源
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="resName">资源名</param>
        private void LoadAssetBundleRes(string fileName, string resName)
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + fileName);
            GameObject obj = GameObject.Instantiate(assetBundle.LoadAsset<GameObject>(resName));
        }

        private void LoadEditorLoadRes()
        {
            GameObject obj = GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Prefabs/Attack.prefab"));
        }
    }
}
#endif

