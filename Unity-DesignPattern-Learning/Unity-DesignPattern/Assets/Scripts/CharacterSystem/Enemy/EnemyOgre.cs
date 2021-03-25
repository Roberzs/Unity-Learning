using System;
using System.Collections.Generic;


public class EnemyOgre : IEnemy
{
    public override void PlayEffect()
    {
        DoPlayEffect("OgreHitEffect");
    }
}

