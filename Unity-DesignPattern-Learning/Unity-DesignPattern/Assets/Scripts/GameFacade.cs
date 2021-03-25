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

        LoadMemento();
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

        CreateMemento();
    }

    /** 关卡系统 */

    
    public Vector3 GetEnemyTargetPosition()     // 敌人最终目标的位置
    {
        return mStageSystem.GetEnemyTargetPosition;
    }

    /** 角色系统 */

    public void AddSoldier(ISoldier soldier)
    {
        mCharacterSystem.AddSoldier(soldier);
    }

    public void AddEnemy(IEnemy enemy)
    {
        mCharacterSystem.AddEnemy(enemy);
    }

    public void RemoveEnemy(IEnemy enemy)
    {
        mCharacterSystem.RemoveEnemy(enemy);
    }

    public void RunVisitor(ICharacterVisitor visitor)
    {
        mCharacterSystem.RunVisitor(visitor);
    }

    /** 能量系统 */

    public bool TakeEnergy(int value)
    {
        return mEnergySystem.TakeEnergy(value);
    }

    public void RecycleEnery(int value)
    {
        mEnergySystem.RecycleEnery(value);
    }

    /** UI系统 */

    public void ShowMsg(string msg)
    {
        mGameStateInfoUI.ShowMsg(msg);
    }

    public void UpdateEnergySlider(int nowEnergy, int maxEnergy)
    {
        mGameStateInfoUI.UpdateEnergySlider(nowEnergy, maxEnergy);
    }

    
    public void ShowCampInfo(ICamp camp)        // 设置显示兵营信息
    {
        mCampInfoUI.ShowCampInfo(camp);
    }

    public void UpdateCurrentStage(int stageCount)
    {
        mGameStateInfoUI.UpdateCurrentStage(stageCount);
    }

    /** 游戏事件系统 */

    public void RegisterObserver(GameEventType eventType, IGameEventObserver observer)
    {
        mGameEventSystem.RegisterObserver(eventType, observer);
    }

    public void RemoveObserver(GameEventType eventType, IGameEventObserver observer)
    {
        mGameEventSystem.RemoveObserver(eventType, observer);
    }

    public void NotifySubject(GameEventType eventType)
    {
        mGameEventSystem.NotifySubject(eventType);
    }

    /** 成就系统 */

    private void LoadMemento()
    {
        AchievementMemento memento = new AchievementMemento();
        memento.LoadData();
        mAchievementSystem.SetMemento(memento);
    }

    private void CreateMemento()
    {
        AchievementMemento memento = mAchievementSystem.CreateMemento();
        memento.SaveData();
    }
}
