using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SoldierRookie : ISoldier
{
    protected override void PlayEffect()
    {
        DoPlayEffect("RookieDeadEffect");
    }

    protected override void PlaySound()
    {
        DoPlaySound("RookieDeath");
    }
}
