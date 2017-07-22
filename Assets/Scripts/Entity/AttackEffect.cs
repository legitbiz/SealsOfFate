using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Entity
{
    public abstract class AttackEffect
    {
        public abstract void Apply(ref CombatData cd);
    }

    class Unblockable : AttackEffect
    {
        public override void Apply(ref CombatData cd)
        {
            cd.Blocking = false;
        }
    }
}
