using UnityEngine;

namespace Combat {
    /// <summary>
    ///     Attack tag descriptor
    /// </summary>
    public abstract class AttackTag : MonoBehaviour {
        /// <summary>
        ///     Applies the attack tag to a defender's combat data
        /// </summary>
        /// <param name="cd">The defender's combat data</param>
        public abstract void Apply(ref TemporaryCombatData cd);
    }
}