namespace Assets.Scripts.Combat {
    /// <summary>
    /// Abstract base class for Attack Effects
    /// </summary>
    public abstract class AttackEffect {
        /// <summary>
        /// Applies this an attack effect to an instance of CombatData
        /// </summary>
        /// <param name="cd">The combat data to modify</param>
        public abstract void Apply(ref CombatData cd);
    }

    /// <summary>
    /// Sample implementation for AttackEffect of an Unblockable Attack
    /// </summary>
    internal class Unblockable : AttackEffect {
        /// <summary>
        /// Applies the unblockable modifier to this attack - removes the defender's Blocking modifier.
        /// </summary>
        /// <param name="cd">The defender's combat data</param>
        public override void Apply(ref CombatData cd) {
            cd.Blocking = false;
        }
    }
}