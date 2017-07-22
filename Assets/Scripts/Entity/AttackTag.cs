using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Entity
{
    internal abstract class AttackTag
    {
        public abstract void Apply(ref CombatData cd);
    }
    
    internal class UnblockableAttack : AttackTag
    {
        public override void Apply(ref CombatData cd)
        {
            cd.Armor = 0;
        }
    }
}
