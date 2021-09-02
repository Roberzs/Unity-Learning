/****************************************************
    文件：EditorCounter.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/7 14:13:9
    功能：Nothing
*****************************************************/


using UnityEditor;
using UnityEngine;
using FrameworkDesign;

namespace Counter
{    
    public class EditorCounter : EditorWindow, IController
    {
        [MenuItem("EditorCounter/Open")]
        private static void OpenMenu()
        {
            Counter.OnRegisterPatch += architecture =>
            {
                architecture.RegisterUtility<IStorage>(new EditorPrefsStorage());
            };

            var editorCounter = GetWindow<EditorCounter>();
            editorCounter.name = nameof(EditorCounter);
            editorCounter.position = new Rect(100, 100, 400, 600);
            editorCounter.Show();
        }

        
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return Counter.Interface;
        }

        private void OnGUI()
        {
            if (GUILayout.Button("+"))
            {
                this.SendCommand<AddCountCommand>();
            }

            GUILayout.Label(Counter.Get<ICountModel>().Count.Value.ToString());

            if (GUILayout.Button("-"))
            {
                this.SendCommand<SubCountCommand>();
            }
        }
    }
}

