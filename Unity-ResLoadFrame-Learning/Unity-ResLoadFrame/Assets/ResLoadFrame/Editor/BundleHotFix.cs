using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;
using UnityEditor.PackageManager.UI;
using System.Threading;

public class BundleHotFix : EditorWindow
{
    [MenuItem("TTT/打包热更新")]
    static void ShowWnd()
    {
        var window = (BundleHotFix)EditorWindow.GetWindow(typeof(BundleHotFix), false, "热更新", true);
        window.Show();
    }

    string md5Path = "";
    string hotfixCount = "1";
    OpenFileName m_OpenFileName = null;

    private void OnGUI()
    {
        
        GUILayout.BeginHorizontal();
        md5Path = EditorGUILayout.TextField("ABMD5文件路径:", md5Path, GUILayout.Width(300), GUILayout.Height(20));
        if (GUILayout.Button("选择文件", GUILayout.Width(100), GUILayout.Height(20)))
        {
            //m_OpenFileName = new OpenFileName();
            //m_OpenFileName.structSize = Marshal.SizeOf(m_OpenFileName);
            //m_OpenFileName.filter = "ABMD5 File(*.bytes)\0*.bytes";
            //m_OpenFileName.file = new string(new char[256]);
            //m_OpenFileName.maxFile = m_OpenFileName.file.Length;
            //m_OpenFileName.fileTitle = new string(new char[64]);
            //m_OpenFileName.maxFileTitle = m_OpenFileName.fileTitle.Length;
            //m_OpenFileName.initialDir = (Application.dataPath + "/../Version").Replace("/", "\\");
            //m_OpenFileName.title = "选择Md5文件";
            //m_OpenFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
            //if (LocalDialog.GetSaveFileName(m_OpenFileName))
            //{
            //    md5Path = m_OpenFileName.file;
            //    Debug.Log(md5Path);
            //}

            ThreadStart childRef = new ThreadStart(ThreadTest1);
            Thread childThread = new Thread(childRef);
            childThread.Start();

        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        hotfixCount = EditorGUILayout.TextField("热更版本:", hotfixCount, GUILayout.Width(300), GUILayout.Height(20));
        GUILayout.EndHorizontal();

        if (GUILayout.Button("开始打包", GUILayout.Width(100), GUILayout.Height(20)))
        {
            if (!string.IsNullOrEmpty(md5Path) && md5Path.EndsWith(".bytes"))
            {
                BundleEditor.Build(true, md5Path, hotfixCount);
            }
        }
    }


    public void ThreadTest1()
    {
        md5Path = WindowsControl.ShowOpen("bytes");
        Debug.Log(md5Path);
    }

}
