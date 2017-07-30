using UnityEngine;

namespace Combat {
    public abstract class DefenseEffect : Object {
        public abstract void Apply(ref CombatData cd);
    }
}