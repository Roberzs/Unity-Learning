using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpController : Controller
{
    public override void Execute(object data)
    {
        // Register Controller
        RegisterController(StringDefine.E_EnterScene, typeof(EnterSceneController));
        RegisterController(StringDefine.E_EndGame, typeof(EndGameController));
        RegisterController(StringDefine.E_PauseGame, typeof(PauseGameController));
        RegisterController(StringDefine.E_ResumeGame, typeof(ResumeGameController));
        RegisterController(StringDefine.E_HitItem, typeof(HitItemController));
        // Register Model
        RegisterModel(new GameModel());
        // Init
        GameModel gm = GetModel<GameModel>();
        gm.Init();
    }
}
