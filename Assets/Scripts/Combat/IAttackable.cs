namespace Assets.Scripts.Combat {
    /// <summary>
    /// An attackable thing!
    /// </summary>
    internal interface IAttackable {
        /// <summary>
        /// Convert this thing to a CombatDataa
        /// </summary>
        /// <returns>A CombatData representing a thing</returns>
        CombatData ToCombatData();
    }
}