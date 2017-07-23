namespace Assets.Scripts.Entity {
    /// <summary>
    ///     Information about defense
    /// </summary>
    public class DefenseInfo {
        private const int MaxDamageMitigation = 100;
        private byte _damageMitigation;

        /// <summary>
        ///     Creates a new defense information object that describes the type of damage mitigated and what percentage is
        ///     mitigated
        /// </summary>
        /// <param name="damageType">The type of damage being mitigated</param>
        /// <param name="damageMitigation">A value from 0 to 100. Values over 100 are forced to 100.</param>
        public DefenseInfo(DamageType damageType, byte damageMitigation) {
            DamageType = damageType;

            DamageMitigation = damageMitigation;
        }

        /// <summary>
        ///     The type of damage mitigated
        /// </summary>
        public DamageType DamageType { get; set; }

        /// <summary>
        ///     The percentage (0 to 100) of damage mitigated
        /// </summary>
        public byte DamageMitigation {
            get { return _damageMitigation; }
            private set { _damageMitigation = (byte) (value > MaxDamageMitigation ? MaxDamageMitigation : value); }
        }
    }
}