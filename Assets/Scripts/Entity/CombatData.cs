using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Entity
{
    public class CombatData
    {
        private int _health = 0;
        private int _mana = 0;

        internal List<string> DefenseEffects = new List<string>();
        internal Queue<string> AttackEffects = new Queue<string>();
        internal Queue<string> AttackTags = new Queue<string>();

        /// <summary>
        /// Used when the Player is attacking an Enemy
        /// </summary>
        /// <param name="p">The player</param>
        public CombatData(Player p)
        {
            _health = GameManager.instance.playerHealth;
            // copy in defense effects
            // copy in attack effects
            // copy in attack tags
        }

        /// <summary>
        /// Used when an Enemy attacks the Player
        /// </summary>
        /// <param name="e">An enemy</param>
        /// <param name="p">The player, the target of the enemy's wrath</param>
        public CombatData(Enemy e)
        {
            _health = e.Health;
            // copy in defense effects
            // copy in attack effects
            // copy in attack tags
        }

        /// <summary>
        /// Computes the damage inflicted by an attacker and defender
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static int ComputeDamage(CombatData attacker, CombatData defender)
        {
            // for each effect or tag in the attack
            // consult the DefenseEffects and apply any defense modifiers
            // return the calcuated damage
            throw new NotImplementedException();
        }
    }
}
