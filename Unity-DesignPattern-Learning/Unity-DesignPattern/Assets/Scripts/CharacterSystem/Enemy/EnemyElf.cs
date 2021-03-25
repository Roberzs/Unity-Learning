using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EnemyElf : IEnemy
{
    public override void PlayEffect()
    {
        DoPlayEffect("ElfHitEffect");
    }
}

