using System;

namespace Combat {
    public enum DamageType {
        Blunt,
        Slashing,
        Piercing,
        Fire,
        Cold
    }

    [Serializable]
    public class AttackInfo {
        /// <summary>
        ///     The damage done by this SealieAttack
        /// </summary>
        public int Damage;

        /// <summary>
        ///     The type of damage done by this SealieAttack
        /// </summary>
        public DamageType DamageType;

        /// <summary>
        ///     A description of the SealieAttack (e.g. sword, tentacle, burning fart)
        /// </summary>
        public string Description;

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
    }
}