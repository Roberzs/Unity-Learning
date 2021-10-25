/****************************************************
    文件：ReopenProjectEditor.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEditor;

public class ReopenProjectEditor
{
    [MenuItem ("编辑器扩展/ReopenProject &r")]
    static void DoReopenProject()
    {
        EditorApplication.OpenProject(Application.dataPath.Replace("Assets", string.Empty));
    }
}
