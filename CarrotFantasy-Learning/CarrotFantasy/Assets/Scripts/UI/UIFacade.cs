/****************************************************
    文件：UIFacade.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/3/11 22:35:35
    功能：UI中介 上层 - UI管理者    下层 - UI面板
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIFacade
{
    // 上层管理者
    public UIManager mUIManager;
    public GameManager mGameManager;
    public AudioSourceManager mAudioSourceManager;
    public PlayerManager mPlayerManager;
    // UI面板
    public Dictionary<string, IBasePanel> currentScenePanelDict = new Dictionary<string, IBasePanel>();
    // 其他成员变量
    private GameObject mask;            // 遮罩层物体
    private Image maskImage;            // 遮罩层图片
    public Transform canvasTransform;

    public IBaseSceneState currentSceneState;
    public IBaseSceneState lastScnenState;

    public UIFacade(UIManager uIManager)
    {
        mGameManager = GameManager.Instance;
        mPlayerManager = mGameManager.playerManager;
        mUIManager = uIManager;
        mAudioSourceManager = mGameManager.audioSourceManager;
        InitMask();
    }

    // 遮罩层初始化
    public void InitMask()
    {
        canvasTransform = GameObject.Find("Canvas").transform;
        mask = CreateUIAndSetUIPosition("Img_Mask");
        maskImage = mask.GetComponent<Image>();
    }

    // 显示遮罩层
    public void ShowMask()
    {
        mask.transform.SetSiblingIndex(10);     // 设置遮罩层渲染层级 

        Tween t = DOTween.To(
            () => maskImage.color,
            toColor => maskImage.color = toColor,
            new Color(0, 0, 0, 1),
            1.0f
            );
        t.OnComplete(ExitSceneComplete);
    }

    // 隐藏遮罩层
    public void HideMask()
    {
        mask.transform.SetSiblingIndex(10);     // 设置遮罩层渲染层级 

        Tween t = DOTween.To(
            () => maskImage.color,
            toColor => maskImage.color = toColor,
            new Color(0, 0, 0, 0),
            1.0f
            );
    }

    // 离开当前场景回调事件
    private void ExitSceneComplete()
    {
        lastScnenState.ExitScene();
        currentSceneState.EnterScene();
        HideMask();
    }

    // 改变当前场景的状态
    public void ChangeSceneState(IBaseSceneState baseSceneState)
    {
        lastScnenState = currentSceneState;
        ShowMask();
        currentSceneState = baseSceneState;
    }

    // 实例化UI
    public GameObject CreateUIAndSetUIPosition(string uiName)
    {
        GameObject itemGo = GetGameObjectResource(FactoryType.UIFactory, uiName);
        itemGo.transform.SetParent(canvasTransform);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        return itemGo;
    }

    // 添加UIPanel到UIManager字典
    public void AddPanelToDict(string uiPanelName)
    {
        mUIManager.currentScenePanelDict.Add(uiPanelName, GetGameObjectResource(FactoryType.UIPanelFactory, uiPanelName));
    }

    // 实例化当前场景所有面板 并存入字典
    public void InitDict()
    {
        foreach (var item in mUIManager.currentScenePanelDict)
        {
            item.Value.transform.SetParent(canvasTransform);
            item.Value.transform.localPosition = Vector3.zero;
            item.Value.transform.localScale = Vector3.one;

            IBasePanel basePanel = item.Value.GetComponent<IBasePanel>();
            if(basePanel == null)
            {
                Debug.LogError("获取面板脚本失败");
            }
            basePanel.InitPanel();
            currentScenePanelDict.Add(item.Key, basePanel);
        }
    }

    // 清空字典
    public void ClearDict()
    {
        currentScenePanelDict.Clear();
        mUIManager.ClearDict();
    }

    /** 对Factory方法进一步封装 */

    // 获取Sprite资源
    public Sprite GetSprite(string resourcePath)
    {
        return mGameManager.GetSprite(resourcePath);
    }

    // 获取AudioClip资源
    public AudioClip GetAudioClip(string resourcePath)
    {
        return mGameManager.GetAudioClip(resourcePath);
    }

    // 获取动画控制器资源
    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return mGameManager.GetRuntimeAnimatorController(resourcePath);
    }

    // 获取游戏物体
    public GameObject GetGameObjectResource(FactoryType factoryType, string resourcePath)
    {
        return mGameManager.GetGameObjectResource(factoryType, resourcePath);
    }

    // 将游戏物体放回对象池
    public void PushGameObjectToFactory(FactoryType factoryType, string resourcePath, GameObject itemGo)
    {
        mGameManager.PushGameObjectToFactory(factoryType, resourcePath, itemGo);
    }

    // 开关音乐
    public void CloseOrOpenEffectMusic()
    {
        mAudioSourceManager.CloseOrOpenEffectMusic();
    }

    public void CloseOrOpenBGMusic()
    {
        mAudioSourceManager.CloseOrOpenBGMusic();
    }

    // 播放音效
    public void PlayButtonAudioClip()
    {
        mAudioSourceManager.PlayButtonAudioClip();
    }

    public void PlayPagingAudioClip()
    {
        mAudioSourceManager.PlayPagingAudioClip();
    }
}
