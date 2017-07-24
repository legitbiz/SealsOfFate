namespace Combat {
    /// <summary>
    /// An attackable thing!
    /// </summary>
    public interface IAttackable {
        /// <summary>
        /// Convert this thing to a CombatDataa
        /// </summary>
        /// <returns>A CombatData representing a thing</returns>
        CombatData ToCombatData();

        void Attack(IAttackable defender);
        void TakeDamage(Damage damage);
    }

    
}