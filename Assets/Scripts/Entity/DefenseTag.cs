using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Entity
{
    public abstract class DefenseTag
    {
        public abstract void Apply(ref CombatData attack);
    }

    class Invincible : DefenseTag
    {
        public override void Apply(ref CombatData attack)
        {
            attack.Damage = 0;
        }
    }
}
