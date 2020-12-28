using System;

public class BattleState: ISceneState
{
    public BattleState(SceneStateController controller) : base("BattleScene", controller)
    {

    }


    public override void StateStart()
    {
        GameFacade.Instance.Init();
    }

    public override void StateUpdate()
    {
        if (GameFacade.Instance.IsGameOver)
        {
            mController.SetState(new MainMenuState(mController));
        }

        GameFacade.Instance.Update();
    }

    public override void StateEnd()
    {
        GameFacade.Instance.Release();
    }
}
