using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuState: ISceneState
{
    public MainMenuState(SceneStateController controller) : base("MainMenuScene", controller)
    {

    }

    public override void StateStart()
    {
        GameObject.Find("StartButton").GetComponent<Button>().onClick.AddListener(OnStartButtonClick);
    }

    private void OnStartButtonClick()
    {
        mController.SetState(new BattleState(mController));
    }
}
