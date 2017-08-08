using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
    public enum AttackType {
        Sealie,
        Unsealie
    }

    /// <summary>
    ///     DTO used for resolving combat
    /// </summary>
    [Serializable]
    public class TemporaryCombatData {
        public readonly List<AttackEffect> AttackEffects;
        public readonly List<AttackTag> AttackTags;
        public readonly List<DefenseEffect> DefenseEffects;
        public readonly List<DefenseTag> DefenseTags;

        public int Armor;
        public bool Blocking;
        public int DamageReduction;
        public DefenseInfo DefenseInfo;
        public int Evasion;
        public int HealthPoints;
        public int ManaPoints;
        public AttackInfo SealieAttack;
        public AttackInfo UnsealieAttack;

        public TemporaryCombatData(CombatData cd) {
            if (cd.AttackEffects != null) {
                AttackEffects = new List<AttackEffect>(cd.AttackTags.Count);
                AttackEffects.AddRange(cd.AttackEffects);
            } else {
                AttackEffects = new List<AttackEffect>();
            }

            if (cd.AttackTags != null) {
                AttackTags = new List<AttackTag>(cd.AttackTags.Count);
                AttackTags.AddRange(cd.AttackTags);
            } else {
                AttackTags = new List<AttackTag>();
            }

            if (cd.DefenseEffects != null) {
                DefenseEffects = new List<DefenseEffect>(cd.DefenseEffects.Count);
                DefenseEffects.AddRange(cd.DefenseEffects);
            } else {
                DefenseEffects = new List<DefenseEffect>();
            }

            if (cd.DefenseTags != null) {
                DefenseTags = new List<DefenseTag>(cd.DefenseTags.Count);
                DefenseTags.AddRange(cd.DefenseTags);
            } else {
                DefenseTags = new List<DefenseTag>();
            }

            Armor = cd.Armor;
            Blocking = cd.Blocking;
            Evasion = cd.Evasion;
            DefenseInfo = cd.DefenseInfo;
            HealthPoints = cd.HealthPoints;
            ManaPoints = cd.ManaPoints;
            DamageReduction = cd.DamageReduction;
            SealieAttack = cd.SealieAttack;
            UnsealieAttack = cd.UnsealieAttack;
        }
    }

    /// <summary>
    ///     CombatData object.
    ///     Both an attacker and defender have CombatData. To prepare for combat, call ToCombatData() on each. Once you have a
    ///     CombatData for each, then use ComputeDamage(attacker, defender) to determine the damage. Once that's done, you'll
    ///     have an integer that you can subtract from someone's health.
    /// </summary>
    [Serializable]
    public class CombatData : MonoBehaviour {
        [SerializeField] private List<AttackEffect> _attackEffects;

        [SerializeField] private List<AttackTag> _attackTags;

        [SerializeField] private List<DefenseEffect> _defenseEffects;

        [SerializeField] private List<DefenseTag> _defenseTags;

        /// <summary>
        ///     Someone's armor
        /// </summary>
        [Range(0, 80)] public int Armor;

        /// <summary>
        ///     Is this combatant blocking?
        /// </summary>
        public bool Blocking;


        /// <summary>
        ///     The player's default damage reduction rating.
        ///     Incoming damage is reduced by this percent.
        /// </summary>
        [Range(0, 70)] public int DamageReduction;

        /// <summary>
        ///     The defense info
        /// </summary>
        public DefenseInfo DefenseInfo;

        /// <summary>
        ///     Chance to evade (between 0 and 70)
        /// </summary>
        [Range(0, 70)] public int Evasion;

        /// <summary>
        ///     The health of this object
        /// </summary>
        [Range(0, 200)] public int HealthPoints;
        public int MaxHealthPoints { get; set; }

        /// <summary>
        ///     ManaPoints from combat
        /// </summary>
        [Range(0, 200)] public int ManaPoints;
        public int MaxManaPoints { get; set; }
        /// <summary>
        ///     The sealie (melee) attack info
        /// </summary>
        public AttackInfo SealieAttack;

        /// <summary>
        ///     The unsealie (magic) attack info
        /// </summary>
        public AttackInfo UnsealieAttack;

        /// <summary>
        ///     A list of attack tags
        /// </summary>
        public List<AttackTag> AttackTags {
            get { return _attackTags; }
        }

        /// <summary>
        ///     A list of AttackEffects
        /// </summary>
        public List<AttackEffect> AttackEffects {
            get { return _attackEffects; }
        }

        /// <summary>
        ///     All them defense tags
        /// </summary>
        public List<DefenseTag> DefenseTags {
            get { return _defenseTags; }
        }

        /// <summary>
        ///     A list of DefenseEffects
        /// </summary>
        public List<DefenseEffect> DefenseEffects {
            get { return _defenseEffects; }
        }

        void Awake() {
            MaxHealthPoints = HealthPoints;
            MaxManaPoints = ManaPoints;
        }

        public TemporaryCombatData ToTemporaryCombatData() {
            return new TemporaryCombatData(this);
        }

        /// <summary>
        /// Heals the player, discarding any over heal.
        /// </summary>
        /// <param name="amount">
        /// The amount of hit points the player regenerates.
        /// </param>
        public void Heal(int amount) {
            if (HealthPoints + amount >= MaxHealthPoints) {
                HealthPoints = MaxHealthPoints;
            } else {
                HealthPoints += amount;
            }
        }

        /// <summary>
        /// Recharges the player's mana, discarding any over generation.
        /// </summary>
        /// <param name="amount">
        /// The amount of mana points the player regenerates.
        /// </param>
        public void RechargeMana(int amount) {
            if (ManaPoints + amount >= MaxManaPoints) {
                ManaPoints = MaxManaPoints;
            } else {
                ManaPoints += amount;
            }
        }

        /// <summary>
        ///     Computes the damage inflicted by an attacker and defender
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <returns></returns>
        public static CombatResult ComputeDamage(TemporaryCombatData attacker, TemporaryCombatData defender) {
            // for each effect or tag in the attack
            if (attacker.AttackTags != null) {
                attacker.AttackTags.ForEach(tag => tag.Apply(ref defender));
            }

            // consult the DefenseEffects and apply any defense modifiers
            if (defender.DefenseTags != null) {
                defender.DefenseTags.ForEach(t => t.Apply(ref attacker));
            }

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