/****************************************************
    文件：UIManager.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/4 11:16:41
    功能：UI管理
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public UIFacade mUIFacade;
    public Dictionary<string, GameObject> currentScenePanelDict;
    private GameManager mGameManager;

    public UIManager()
    {
        mGameManager = GameManager.Instance;
        currentScenePanelDict = new Dictionary<string, GameObject>();
        mUIFacade = new UIFacade(this);
        mUIFacade.currentSceneState = new StartLoadSceneState(mUIFacade);
    }

    // 清空字典
    public void ClearDict()
    {
        foreach (var item in currentScenePanelDict)
        {
            // 注意: 推栈时 物体名应该截掉(clone)后缀
            PushUIPanel(item.Value.name.Substring(0, item.Value.name.Length - 7), item.Value);
        }

        currentScenePanelDict.Clear();
    }

    // 将UIPanel放回对象池
    private void PushUIPanel(string uiPanelName, GameObject uiPanelFGo)
    {
        mGameManager.PushGameObjectToFactory(FactoryType.UIPanelFactory, uiPanelName, uiPanelFGo);
    }
}
