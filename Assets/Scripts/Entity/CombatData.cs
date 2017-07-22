using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Entity
{
    public class CombatData
    {
        private Queue<AttackEffect> _attackEffects = new Queue<AttackEffect>();
        private readonly Queue<AttackTag> _attackTags = new Queue<AttackTag>();

        private List<DefenseInfo> _defenseEffects = new List<DefenseInfo>();
        private readonly List<DefenseTag> _defenseTags = new List<DefenseTag>();

        /// <summary>
        ///     Used when the Player is attacking an Enemy
        /// </summary>
        /// <param name="p">The player</param>
        public CombatData(Player p)
        {
            Health = GameManager.instance.playerHealth;
            // copy in defense effects
            // copy in attack effects
            // copy in attack tags
        }

        /// <summary>
        ///     Used when an Enemy attacks the Player
        /// </summary>
        /// <param name="e">An enemy</param>
        /// <param name="p">The player, the target of the enemy's wrath</param>
        public CombatData(Enemy e)
        {
            Health = e.Health;
            // copy in defense effects
            // copy in attack effects
            // copy in attack tags
        }

        public int Health { get; set; }
        public int Mana { get; set; }
        public int Armor { get; set; }
        public AttackInfo AttackInfo { get; set; }
        public bool Blocking { get; set; }

        /// <summary>
        ///     Computes the damage inflicted by an attacker and defender
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static int ComputeDamage(CombatData attacker, CombatData defender)
        {
            // for each effect or tag in the attack
            foreach (var tag in attacker._attackTags)
            {
                tag.Apply(ref defender);
            }

            foreach (var effect in attacker._attackEffects)
            {
                effect.Apply(ref defender);
            }

            // consult the DefenseEffects and apply any defense modifiers
            foreach (var tag in defender._defenseTags)
            {
                tag.Apply(ref attacker);
            }

            var damage = defender._defenseEffects.Where(effect => effect.DamageType == attacker.AttackInfo.DamageType)
                                 .Aggregate(attacker.AttackInfo.Damage, (current, effect) => current - effect.DamageMitigation);

            // return the calcuated damage
            return damage - defender.Armor;
        }
    }

    
}