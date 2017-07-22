using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Entity {
    /// <summary>
    ///     CombatData object.
    ///     Both an attacker and defender have CombatData. To prepare for combat, call ToCombatData() on each. Once you have a
    ///     CombatData for each, then use ComputeDamage(attacker, defender) to determine the damage. Once that's done, you'll
    ///     have an integer that you can subtract from someone's health.
    /// </summary>
    public class CombatData {
        private readonly List<AttackEffect> _attackEffects = new List<AttackEffect>();
        private readonly List<AttackTag> _attackTags = new List<AttackTag>();
        private readonly List<DefenseInfo> _defenseEffects = new List<DefenseInfo>();
        private readonly List<DefenseTag> _defenseTags = new List<DefenseTag>();

        /// <summary>
        /// Create CombatData from a player
        /// </summary>
        /// <param name="p">The player</param>
        public CombatData(Player p) {
            Health = GameManager.instance.playerHealth;
            // copy in defense effects
            // copy in attack effects
            // copy in attack tags
        }

        /// <summary>
        /// Create CombatData from an enemy
        /// </summary>
        /// <param name="e">An enemy</param>
        public CombatData(Enemy e) {
            Health = e.Health;
            // copy in defense effects
            // copy in attack effects
            // copy in attack tags
        }

        /// <summary>
        /// The health of this object
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Mana from combat
        /// </summary>
        public int Mana { get; set; }

        /// <summary>
        /// Someone's armor
        /// </summary>
        public int Armor { get; set; }

        /// <summary>
        ///  The attack info
        /// </summary>
        public AttackInfo AttackInfo { get; set; }

        /// <summary>
        /// The defense info
        /// </summary>
        public DefenseInfo DefenseInfo { get; set; }

        /// <summary>
        /// Is this combatant blocking?
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
            var damage = defender._defenseEffects.Where(effect => effect.DamageType == attacker.AttackInfo.DamageType)
                .Aggregate(attacker.AttackInfo.Damage, (current, effect) => current - effect.DamageMitigation);

            // return the calcuated damage
            return damage - defender.Armor;
        }
    }
}