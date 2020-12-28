using System;
using UnityEngine;
using UnityEngine.UI;

public class StartState : ISceneState
{

    public StartState(SceneStateController controller) : base("StartScene", controller)
    {

    }

    private Image mLogo;
    private float mSmoothingSpeed = 1;
    private float mWaitTime = 3;        // 跳转倒计时

    public override void StateStart()
    {
        mLogo = GameObject.Find("Logo").GetComponent<Image>();
        mLogo.color = Color.black;
    }

    public override void StateUpdate()
    {
        mLogo.color = Color.Lerp(mLogo.color, Color.white, mSmoothingSpeed * Time.deltaTime);
        mWaitTime -= Time.deltaTime;

        if (mWaitTime <= 0)
        {
            mController.SetState(new MainMenuState(mController));
        }
    }
}
