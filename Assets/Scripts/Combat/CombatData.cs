using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Combat {
    /// <summary>
    ///     CombatData object.
    ///     Both an attacker and defender have CombatData. To prepare for combat, call ToCombatData() on each. Once you have a
    ///     CombatData for each, then use ComputeDamage(attacker, defender) to determine the damage. Once that's done, you'll
    ///     have an integer that you can subtract from someone's health.
    /// </summary>
    public class CombatData {
        private const byte MaxEvasion = 70;
        private readonly List<AttackEffect> _attackEffects = new List<AttackEffect>();
        private readonly List<AttackTag> _attackTags = new List<AttackTag>();
        private readonly List<DefenseInfo> _defenseEffects = new List<DefenseInfo>();
        private readonly List<DefenseTag> _defenseTags = new List<DefenseTag>();


        private byte _evasion;

        /// <summary>
        ///     The health of this object
        /// </summary>
        public int HealthPoints { get; set; }

        /// <summary>
        ///     ManaPoints from combat
        /// </summary>
        public int ManaPoints { get; set; }

        /// <summary>
        ///     Someone's armor
        /// </summary>
        public int Armor { get; set; }

        /// <summary>
        ///     The sealie (melee) attack info
        /// </summary>
        public AttackInfo SealieAttack { get; set; }

        /// <summary>
        ///     The unsealie (magic) attack info
        /// </summary>
        public AttackInfo UnsealieAttack { get; set; }

        /// <summary>
        ///     The defense info
        /// </summary>
        public DefenseInfo DefenseInfo { get; set; }

        /// <summary>
        ///     Chance to evade (between 0 and 100)
        /// </summary>
        public byte Evasion {
            get { return _evasion; }
            set { _evasion = value > MaxEvasion ? MaxEvasion : value; }
        }

        /// <summary>
        ///     The player's default damage reduction rating.
        ///     Incoming damage is reduced by this percent.
        /// </summary>
        public byte DamageReduction { get; set; }

        /// <summary>
        ///     Is this combatant blocking?
        /// </summary>
        public bool Blocking { get; set; }

        /// <summary>
        ///     Computes the damage inflicted by an attacker and defender
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static int ComputeDamage(CombatData attacker, CombatData defender) {
            // for each effect or tag in the attack
            attacker._attackTags.ForEach(tag => tag.Apply(ref defender));
            attacker._attackEffects.ForEach(effect => effect.Apply(ref defender));

            // consult the DefenseEffects and apply any defense modifiers
            defender._defenseTags.ForEach(t => t.Apply(ref attacker));

            // Calculate damage
            var damage = defender._defenseEffects.Where(effect => effect.DamageType == attacker.SealieAttack.DamageType)
                .Aggregate(attacker.SealieAttack.Damage, (current, effect) => current - effect.DamageMitigation);

            // return the calcuated damage
            damage -= defender.Armor;

            return damage < 0 ? 0 : damage;
        }
    }
}