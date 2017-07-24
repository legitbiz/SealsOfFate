using System;
using System.Runtime.CompilerServices;
using Combat;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using Random = UnityEngine.Random;

/// <summary>
///     This class is the general enemy class. It extends MovingObject and is expected to be extended by more specific
///     classes for particular enemy behavior. It defines general functions that most enemies will need.
/// </summary>
public class Enemy : MovingObject, IAttackable {
    private readonly CombatData _combatData;

    /// <summary> The state machine that handles state transitions. </summary>
    private readonly StateMachine<Enemy> _stateMachine;

    /// <summary> The maximum attack range for an enemy. Enemies will try to stay below this range </summary>
    public int max_range;

    /// <summary> The minimum attack range for an enemy. Enemies will try to stay above this range</summary>
    public int min_range;

    /// <summary>The normal movement speed of the enemy</summary>
    public int speed;

    private Enemy() {
        _stateMachine = new StateMachine<Enemy>(this);
        _stateMachine.CurrentState = StateAlert.getInstance();

        // TODO this needs to be filled with appropriate information for an Enemy
        // TODO Should wire up properties to data stored inside of CombatData so we can directly control these variables through the Unity UI
        _combatData = new CombatData();
        _combatData.SealieAttack = new AttackInfo(5, DamageType.Blunt, "A harsh truth, told cruelly");
    }

    /// <summary>The enemy's health points</summary>
    public short Health {
        get { return _combatData.HealthPoints; }
        set { _combatData.HealthPoints = value; }
    }

    /// <summary>
    ///     The primary weapon of all enemies is the truth
    /// </summary>
    public AttackInfo Weapon {
        get { return _combatData.SealieAttack; }
    }

    /// <summary>
    ///     Returns the state machine instance
    /// </summary>
    /// <returns>The State Machine</returns>
    public StateMachine<Enemy> StateMachine {
        get { return _stateMachine; }
    }

    /// <summary>
    ///     Creates a CombatData object this particular enemy
    /// </summary>
    /// <returns>A CombatData representing the enemy</returns>
    /// <remarks>
    ///     CombatData's deep clone is used to avoid combat changing actual state through an attacker or defender's
    ///     effects/tags. The CombatResult can be extended to include changes to state or long-term effects for the
    ///     player/enemy to suffer.
    /// </remarks>
    public CombatData ToCombatData() {
        return _combatData.DeepClone();
    }

    /// <summary>
    ///     Attack something
    /// </summary>
    /// <param name="defender">The thing that may or may not defend itself</param>
    public void Attack(IAttackable defender) {
        // TODO this code is currently copypasta from the Player. That definitely needs to be changed.
        var damage = CombatData.ComputeDamage(_combatData, defender.ToCombatData());
        Debug.Log(String.Format("penguin inflicts {0} damage on player", damage.DefenderDamage.HealthDamage));
        defender.TakeDamage(damage.DefenderDamage);
        TakeDamage(damage.AttackerDamage);
    }

    /// <summary>
    ///     Oh noes! I have been hit.
    /// </summary>
    /// <param name="damage">Damage to be dealt</param>
    public void TakeDamage(Damage damage) {
        // TODO this code is currently copypasta from the Player. That definitely needs to be changed.
        _combatData.HealthPoints -= damage.HealthDamage;
        _combatData.ManaPoints -= damage.ManaDamage;

        if (_combatData.HealthPoints <= 0) {
            Debug.Log("In theory, this penguin is dead");
        }
    }

    /// <summary>
    ///     Sets up the enemy on load and registers it with the game manager.
    /// </summary>
    private void Awake() {
        GameManager.instance.RegisterEnemy(this);
    }

    /// <summary>
    ///     Attempts to move this enemy towards the player.
    /// </summary>
    public void SeekPlayer() {
        int horizontal, vertical;

        //Find the player
        var playerObj = FindObjectOfType<Player>();

        //Calculate a vector pointing from this enemy to the player
        Vector2 playerDir = playerObj.transform.position - transform.position;

        //Normalize the vector to a magnitude of 1
        playerDir.Normalize();

        //***Decompose to pure horizontal and vertical***
        //Travel in the direction of whichever component is larger. In case of a tie, do a coin flip.
        float coinFlip;
        if (Math.Abs(Math.Abs(playerDir.x) - Math.Abs(playerDir.y)) < FloatComparer.kEpsilon) {
            coinFlip = Random.value;
            if (coinFlip >= 0.51) //Random.value returns a number between 0.0 and 1.0 inclusively
            {
                horizontal = 1;
                vertical = 0;
            } else {
                vertical = 1;
                horizontal = 0;
            }
        } else if (Math.Abs(playerDir.x) > Math.Abs(playerDir.y)) {
            horizontal = 1;
            vertical = 0;
        } else {
            horizontal = 0;
            vertical = 1;
        }

        //If the original vector was negative, flip our movement direction.
        if (playerDir.x < 0) {
            horizontal *= -1;
        }
        if (playerDir.y < 0) {
            vertical *= -1;
        }

        //***Simple and stupid obstacle avoidance***
        //Raycast in the direction of travel, if it hits a non-player blocking object, randomly generate a new location
        //to move to. Placeholder for actual pathfinding.
        RaycastHit2D hit;
        if (RaycastInDirection(horizontal, vertical, out hit)) {
            while (RaycastInDirection(horizontal, vertical, out hit) && hit.transform != playerObj.transform) {
                horizontal = (int) Random.Range(0, 1.99f);
                if (horizontal == 0) {
                    vertical = 1;
                }

                coinFlip = Random.value;
                if (coinFlip >= 0.51) {
                    horizontal *= -1;
                    vertical *= -1;
                }
            }
        }

        //move in the direction given
        AttemptMove<Component>(horizontal, vertical);
    }

    protected override void OnCantMove<T>(T component) {
        if (component.CompareTag("Player")) {
            Debug.Log("Penguin attacks player");
            var player = FindObjectOfType<Player>();
            Attack(player);
        }
    }
}