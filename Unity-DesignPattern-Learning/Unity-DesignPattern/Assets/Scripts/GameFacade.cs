using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 
 * 外观模式 中介者模式 单例模式
 * 
 */

public class GameFacade
{
    private static GameFacade _instance = new GameFacade();
    public static GameFacade Instance { get { return _instance; } }
    
    private bool mIsGameOver = false;
    public bool IsGameOver { get { return mIsGameOver; } }

    private GameFacade() { }

    private AchievementSystem mAchievementSystem;
    private CampSystem mCampSystem;
    private CharacterSystem mCharacterSystem;
    private EnergySystem mEnergySystem;
    private GameEventSystem mGameEventSystem;
    private StageSystem mStageSystem;

    private GameStateInfoUI mGameStateInfoUI;
    private GamePauseUI mGamePauseUI;
    private CampInfoUI mCampInfoUI;
    private SoldierInfoUI mSoldierInfoUI;

    public void Init()
    {
        // 初始化
        mAchievementSystem = new AchievementSystem();
        mCampSystem = new CampSystem();
        mCharacterSystem = new CharacterSystem();
        mEnergySystem = new EnergySystem();
        mGameEventSystem = new GameEventSystem();
        mStageSystem = new StageSystem();

        mGameStateInfoUI = new GameStateInfoUI();
        mGamePauseUI = new GamePauseUI();
        mCampInfoUI = new CampInfoUI();
        mSoldierInfoUI = new SoldierInfoUI();

        mAchievementSystem.Init();
        mCampSystem.Init();
        mCharacterSystem.Init();
        mEnergySystem.Init();
        mGameEventSystem.Init();
        mStageSystem.Init();

        mGameStateInfoUI.Init();
        mGamePauseUI.Init();
        mCampInfoUI.Init();
        mSoldierInfoUI.Init();
    }

    public void Update()
    {
        mAchievementSystem.Update();
        mCampSystem.Update();
        mCharacterSystem.Update();
        mEnergySystem.Update();
        mGameEventSystem.Update();
        mStageSystem.Update();

        mGameStateInfoUI.Update();
        mGamePauseUI.Update();
        mCampInfoUI.Update();
        mSoldierInfoUI.Update();
    }

    public void Release()
    {
        mAchievementSystem.Release();
        mCampSystem.Release();
        mCharacterSystem.Release();
        mEnergySystem.Release();
        mGameEventSystem.Release();
        mStageSystem.Release();

        mGameStateInfoUI.Release();
        mGamePauseUI.Release();
        mCampInfoUI.Release();
        mSoldierInfoUI.Release();
    }
}
