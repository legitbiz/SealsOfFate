using System;
using UnityEngine;
using System.Collections;
using Combat;

[Serializable]
public class IgnoresArmorAttackTag : AttackTag
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Set the defender's armor to 0.
    /// </summary>
    /// <param name="cd">The defender's combat data</param>
    public override void Apply(ref TemporaryCombatData cd)
    {
        cd.Armor = 0;
    }
}
