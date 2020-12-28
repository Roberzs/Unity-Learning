using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



/** 状态拥有者 */
public class SceneStateController 
{
    private ISceneState mState;
    private AsyncOperation mAO;

    public void SetState(ISceneState state, bool isLoadScene = true)
    {
        if (mState != null)
        {
            mState.StateEnd();      // 对上一个场景做清理工作
        }

        mState = state;

        // 如果需要加载该场景(默认true)， 进行加载事件。 否则直接执行 StateStart() 方法。
        if (isLoadScene)
        {
            mAO = SceneManager.LoadSceneAsync(mState.SceneName);
            
        }
        else
        {
            mState.StateStart();
        }
        
    }

    public void StartUpdate()
    {
        // 如果场景还没加载完 说明目前场景属于过度阶段 则不需要进行更新 直接return
        if (mAO != null && mAO.isDone == false) return;

        // 如果场景刚刚加载完 将mAO置空 执行 StateStart() 事件
        if (mAO != null && mAO.isDone == true)
        {
            mAO = null;
            mState.StateStart();
        }

        // 如果目前 mState 不为空 执行 StateUpdate()
        if (mState != null)
        {
            mState.StateUpdate();
        }
    }
}
