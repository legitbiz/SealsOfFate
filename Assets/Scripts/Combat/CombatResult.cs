namespace Combat
{
    /// <summary>
    ///     The result of agiven combat
    /// </summary>
    public class CombatResult {
        /// <summary>
        ///     Default constructor - no damage occured
        /// </summary>
        public CombatResult() {
            DefenderDamage = new Damage {HealthDamage = 0, ManaDamage = 0};
            AttackerDamage = new Damage {HealthDamage = 0, ManaDamage = 0};
        }

        /// <summary>
        ///     A moderately better constructor
        /// </summary>
        /// <param name="defenderDamage">Damage taken by the defender</param>
        /// <param name="attackerDamage">Damage taken by the attacker</param>
        public CombatResult(Damage defenderDamage, Damage attackerDamage) {
            DefenderDamage = defenderDamage;
            AttackerDamage = attackerDamage;
        }

        /// <summary>
        ///     Damage taken by the defender
        /// </summary>
        public Damage DefenderDamage { get; set; }

        /// <summary>
        ///     Damage taken by the attacker
        /// </summary>
        public Damage AttackerDamage { get; set; }
    }
}