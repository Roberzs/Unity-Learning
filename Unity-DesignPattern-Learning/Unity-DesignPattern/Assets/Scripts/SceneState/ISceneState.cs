using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/** 抽象状态角色 */
public class ISceneState 
{
    private string mSceneName;
    protected SceneStateController mController;

    public ISceneState(string sceneName,SceneStateController controller)
    {
        mSceneName = sceneName;
        mController = controller;
    }

    public string SceneName
    {
        get { return mSceneName; }
    }

    public virtual void StateStart() { }
    public virtual void StateEnd() { }
    public virtual void StateUpdate() { }
}
