﻿using Assets.Scripts;

/// <summary>
///     A food consumable that applies some manner of health to a player
/// </summary>
public class Food : Consumable {
    /// <summary>
    ///     Default constructor
    /// </summary>
    public Food() {
        Multiplier = 1;
        HealthMod = 5;
    }

    /// <summary>
    ///     The health modifier
    /// </summary>
    public ushort HealthMod { get; set; }

    /// <summary>
    ///     The health multiplier
    /// </summary>
    public ushort Multiplier { get; set; }

    /// <summary>
    ///     Increases the player's health by HealthMod * Multiplier.
    ///     Set up this way for ease of generating different "food/fish" consumables that will provide
    ///     different amounts of health.
    /// </summary>
    public override void Consume() {
        var newHealth = GameManager.Instance.PlayerHealth + HealthMod * Multiplier;

        if (newHealth > ushort.MaxValue) {
            newHealth = ushort.MaxValue;
        }

        GameManager.Instance.PlayerHealth = (short) newHealth;
    }
}