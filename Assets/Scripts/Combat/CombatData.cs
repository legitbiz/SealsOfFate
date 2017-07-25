using System;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

namespace Combat {
    public enum AttackType {
        Sealie,
        Unsealie
    }

    /// <summary>
    ///     CombatData object.
    ///     Both an attacker and defender have CombatData. To prepare for combat, call ToCombatData() on each. Once you have a
    ///     CombatData for each, then use ComputeDamage(attacker, defender) to determine the damage. Once that's done, you'll
    ///     have an integer that you can subtract from someone's health.
    /// </summary>
    [Serializable]
    public class CombatData : MonoBehaviour, IDeepCloneable<CombatData> {
        [SerializeField] private List<AttackEffect> _attackEffects;

        [SerializeField] private List<AttackTag> _attackTags;

        [SerializeField] private List<DefenseEffect> _defenseEffects;

        [SerializeField] private List<DefenseTag> _defenseTags;

        /// <summary>
        ///     Someone's armor
        /// </summary>
        [Range(0, 16384)] public int Armor;

        /// <summary>
        ///     Chance to evade (between 0 and 70)
        /// </summary>
        [Range(0, 70)] public int Evasion;

        /// <summary>
        ///     The health of this object
        /// </summary>
        [Range(0, 32768)] public int HealthPoints;

        /// <summary>
        ///     ManaPoints from combat
        /// </summary>
        [Range(0, 32768)] public int ManaPoints;

        /// <summary>
        ///     A list of attack tags
        /// </summary>
        public List<AttackTag> AttackTags {
            get { return _attackTags; }
        }

        /// <summary>
        ///     All them defense tags
        /// </summary>
        public List<DefenseTag> DefenseTags {
            get { return _defenseTags; }
        }

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
        ///     The player's default damage reduction rating.
        ///     Incoming damage is reduced by this percent.
        /// </summary>
        public byte DamageReduction { get; set; }

        /// <summary>
        ///     Is this combatant blocking?
        /// </summary>
        public bool Blocking { get; set; }

        public CombatData DeepClone() {
            var cd = new CombatData {
                Evasion = Evasion,
                DefenseInfo = DefenseInfo,
                HealthPoints = HealthPoints,
                ManaPoints = ManaPoints,
                Armor = Armor,
                Blocking = Blocking,
                DamageReduction = DamageReduction,
                SealieAttack = SealieAttack,
                UnsealieAttack = UnsealieAttack
            };

            AttackTags.ForEach(t => cd.AttackTags.Add(t));
            DefenseTags.ForEach(t => cd.DefenseTags.Add(t));

            return cd;
        }

        object IDeepCloneable.DeepClone() {
            return DeepClone();
        }

        /// <summary>
        ///     Computes the damage inflicted by an attacker and defender
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static CombatResult ComputeDamage(CombatData attacker, CombatData defender) {
            // for each effect or tag in the attack
            attacker._attackTags.ForEach(tag => tag.Apply(ref defender));

            // consult the DefenseEffects and apply any defense modifiers
            defender._defenseTags.ForEach(t => t.Apply(ref attacker));

            // Calculate damage
            // TODO: combat needs to be modified to send in current AttackInfo. For now we assume combat is all melee.
            var damage = attacker.SealieAttack.Damage;

            if (defender.DefenseInfo != null && defender.DefenseInfo.DamageType == attacker.SealieAttack.DamageType) {
                damage = (int) Math.Floor(damage * 0.01m * defender.DefenseInfo.DamageMitigation);
            }

            // return the calcuated damage
            damage -= defender.Armor;

            return new CombatResult {
                DefenderDamage = {
                    HealthDamage = (short) (damage < 0 ? 0 : damage),
                    ManaDamage = 0
                },
                AttackerDamage = {
                    HealthDamage = 0,
                    ManaDamage = 0
                }
            };
        }
    }
}