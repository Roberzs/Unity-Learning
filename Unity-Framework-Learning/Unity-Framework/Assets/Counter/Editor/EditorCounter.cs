/****************************************************
    文件：EditorCounter.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：2021/7/7 14:13:9
    功能：Nothing
*****************************************************/

using UnityEditor;
using UnityEngine;

namespace Counter
{
    public class EditorCounter : EditorWindow
    {
        [MenuItem("EditorCounter/Open")]
        private static void OpenMenu()
        {
            var editorCounter = GetWindow<EditorCounter>();
            editorCounter.name = nameof(EditorCounter);
            editorCounter.position = new Rect(100, 100, 400, 600);
            editorCounter.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("+"))
            {
                new AddCountCommand().Execute();
            }

            GUILayout.Label(CounterModel.Count.Value.ToString());

            if (GUILayout.Button("-"))
            {
                new SubCountCommand().Execute();
            }
        }
    }
}

