namespace Assets.Scripts.Entity {
    /// <summary>
    /// Attack tag descriptor
    /// </summary>
    internal abstract class AttackTag {
        /// <summary>
        /// Applies the attack tag to a defender's combat data
        /// </summary>
        /// <param name="cd">The defender's combat data</param>
        public abstract void Apply(ref CombatData cd);
    }

    /// <summary>
    /// An attack that ignores armor
    /// </summary>
    internal class IgnoresArmorAttackTag : AttackTag {
        /// <summary>
        /// Set the defender's armor to 0.
        /// </summary>
        /// <param name="cd">The defender's combat data</param>
        public override void Apply(ref CombatData cd) {
            cd.Armor = 0;
        }
    }
}