using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpController : Controller
{
    public override void Execute(object data)
    {
        // Register Controller

        // Register Model
        RegisterModel(new GameModel());
        // Init

        
    }
}
