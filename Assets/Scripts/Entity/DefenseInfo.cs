using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Entity
{
    public class DefenseInfo
    {
        /// <summary>
        /// The type of damage blocked
        /// </summary>
        public DamageType DamageType { get; set; }

        /// <summary>
        /// The amount of damage mitigated
        /// </summary>
        public int DamageMitigation { get; set; }

        public DefenseInfo(DamageType damageType, int damageMitigation)
        {
            DamageType = damageType;
            DamageMitigation = damageMitigation;
        }
    }
}
