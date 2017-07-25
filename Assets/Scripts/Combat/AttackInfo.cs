namespace Combat {
    public enum DamageType {
        Blunt,
        Slashing,
        Piercing,
        Fire,
        Cold
    }

    public class AttackInfo : UnityEngine.Object {
        /// <summary>
        ///     Creates a new SealieAttack
        /// </summary>
        /// <param name="damage">The amount of pain to inflict</param>
        /// <param name="damageType">The manner in which it is inflicted</param>
        /// <param name="description">A human readable description (such as "laserjet printer")</param>
        public AttackInfo(int damage, DamageType damageType, string description) {
            Damage = damage;
            DamageType = damageType;
            Description = description;
        }

        /// <summary>
        ///     The damage done by this SealieAttack
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        ///     The type of damage done by this SealieAttack
        /// </summary>
        public DamageType DamageType { get; set; }

        /// <summary>
        ///     A description of the SealieAttack (e.g. sword, tentacle, burning fart)
        /// </summary>
        public string Description { get; set; }
    }
}