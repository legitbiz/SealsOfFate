using System;
using Combat;

[Serializable]
public class IgnoresArmorAttackTag : AttackTag {
    // Use this for initialization
    private void Start() { }

    // Update is called once per frame
    private void Update() { }

    /// <summary>
    ///     Set the defender's armor to 0.
    /// </summary>
    /// <param name="cd">The defender's combat data</param>
    public override void Apply(ref TemporaryCombatData cd) {
        cd.Armor = 0;
    }
}