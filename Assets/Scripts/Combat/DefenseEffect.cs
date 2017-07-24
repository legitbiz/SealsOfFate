using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Combat
{
    public abstract class DefenseEffect {
        public abstract void Apply(ref CombatData cd);
    }
}
