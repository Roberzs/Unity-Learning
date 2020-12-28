/****************************************************
    文件：GameLoop.cs
	作者：zhyStay
    邮箱: zhy18125@163.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

/**
 *    [             GameLoop            ]          [                      ]             [             ]
 *    | ------------------------------- |  ------> | SceneStateController | ----------> | ISceneState |
 *    [ +controller:SceneStateController]          [                      ]             [             ]
 *                                                                                             ^
 *                                                                                             |
 *                                                                                             |
 *                                                                           ---------------------------------------
 *                                                                           |                 |                   |
 *                                                                           |                 |                   |
 *                                                                           |                 |                   |
 *                                                                  [                ] [                ]  [                ]           [               ]
 *                                                                  |   StartState   ] |  MainMenuState ]  |   BattleState  | --------> |   GameFacade  |
 *                                                                  [                ] [                ]  [                ]           [               ]
 */

public class GameLoop : MonoBehaviour 
{
    private SceneStateController controller = null;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        controller = new SceneStateController();
        controller.SetState(new StartState(controller), false);
    }

    private void Update()
    {
        controller.StartUpdate();
    }
}