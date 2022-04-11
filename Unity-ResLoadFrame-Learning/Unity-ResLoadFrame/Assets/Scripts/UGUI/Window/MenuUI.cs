﻿/****************************************************
	文件：MenuUI.cs
	作者：Zhang
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class MenuUI : Window
{
	private MenuPanel m_MainPanel;

    public override void Awake(params object[] paramList)
    {
		m_MainPanel = GameObject.GetComponent<MenuPanel>();
        AddButtonClickListener(m_MainPanel.m_StartBtn, OnClickStart);
        AddButtonClickListener(m_MainPanel.m_LoadBtn, OnClickLoad);
        AddButtonClickListener(m_MainPanel.m_ExitBtn, OnClickExit);

        ReadMonsterData();
    }

    private void ReadMonsterData()
    {
        MonsterData monsterData = ConfigManager.Instance.FindData<MonsterData>("Assets/GameData/Data/Binary/MonsterData.bytes");
        for (int i = 0; i < monsterData.AllMonster.Count; i++)
        {
            MonsterBase data = monsterData.AllMonster[i];
            Debug.Log($"ID:{data.Id}  Name:{data.Name}  Level:{data.Level}");
        }
    }

    #region 按钮事件示例

	void OnClickStart()
    {
        Debug.Log("点击了开始游戏");
    }

	void OnClickLoad()
    {
        Debug.Log("点击了加载游戏");
    }

    void OnClickExit()
    {
        Debug.Log("点击了结束游戏");
    }

    #endregion
}

