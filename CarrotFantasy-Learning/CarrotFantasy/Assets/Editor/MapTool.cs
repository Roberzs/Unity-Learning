/****************************************************
    文件：MapTool.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

/** 地图编辑与游戏运行两种状态由宏定义区分 */

#if MapEditing

[CustomEditor(typeof(MapMaker))]
public class MapTool : Editor 
{
    private MapMaker mapMaker;

    // 关卡文件列表
    private List<FileInfo> fileList = new List<FileInfo>();
    private string[] fileNameList;

    // 当前编辑关卡的索引
    private int selectIndex = -1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
        {
            mapMaker = MapMaker.Instrance;

            EditorGUILayout.BeginHorizontal();
            // 获取文件名
            fileNameList = GetNames(fileList);

            int currentIndex = EditorGUILayout.Popup(selectIndex, fileNameList);
            if (currentIndex != selectIndex)
            {
                selectIndex = currentIndex;

                // 实例化地图
                mapMaker.InitMap();
                // 加载当前选择的关卡文件
                mapMaker.LoadLevelFile(mapMaker.LoadLevelInfoFile(fileNameList[selectIndex]));
            }

            if (GUILayout.Button("读取关卡列表"))
            {
                LoadLevelFile();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("回复地图编辑器默认状态"))
            {
                mapMaker.RecoverTowerPoint();
            }

            if (GUILayout.Button("清除怪物路点"))
            {
                mapMaker.ClearMonsterPath();
            }

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("保存当前关卡数据文件"))
            {
                mapMaker.SaveLevelFileByJson();
            }
        }
    }

    // 加载关卡数据文件
    private void LoadLevelFile()
    {
        ClearList();
        fileList = GetLevelFiles();
    }


    // 清空文件列表
    private void ClearList()
    {
        fileList.Clear();
        selectIndex = -1;
    }

    // 读取关卡列表
    private List<FileInfo> GetLevelFiles()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Json/Level/", "*.json");
        List<FileInfo> list = new List<FileInfo>();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = new FileInfo(files[i]);
            list.Add(file);
        }
        return list;
    }

    // 获取关卡文件的名字
    private string[] GetNames(List<FileInfo> files)
    {
        List<string> names = new List<string>();
        foreach (var file in files)
        {
            names.Add(file.Name);
        }
        return names.ToArray();
    }
}

#endif

#if GameRuning

#endif

