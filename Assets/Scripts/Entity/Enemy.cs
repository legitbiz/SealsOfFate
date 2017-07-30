using Assets.Scripts;
using Combat;
using UnityEngine;

/// <summary>
///     This class is the general enemy class. It extends MovingObject and is expected to be extended by more specific
///     classes for particular enemy behavior. It defines general functions that most enemies will need.
/// </summary>
public class Enemy : MovingObject, IAttackable {
    /// <summary> The state machine that handles state transitions. </summary>
    private readonly StateMachine<Enemy> _stateMachine;

    [SerializeField] private CombatData _combatData;

    /// <summary> The maximum attack range for an enemy. Enemies will try to stay below this range </summary>
    public int MaxRange;

    /// <summary> The minimum attack range for an enemy. Enemies will try to stay above this range</summary>
    public int MinRange;

    /// <summary>The normal movement speed of the enemy</summary>
    public int Speed;

    private Enemy() {
        _stateMachine = new StateMachine<Enemy>(this);
        _stateMachine.CurrentState = StateAlert.getInstance();
    }

    /// <summary>
    ///     The enemy's health points
    /// </summary>
    public int Health {
        get {
            _combatData = GetComponent<CombatData>();
            return _combatData.HealthPoints;
        }
        set {
            _combatData = GetComponent<CombatData>();
            _combatData.HealthPoints = value;
        }
    }

    /// <summary>
    ///     The primary weapon of all enemies is the truth
    /// </summary>
    public AttackInfo Weapon {
        get {
            _combatData = GetComponent<CombatData>();
            return _combatData.SealieAttack;
        }
    }

    /// <summary>
    ///     Returns the state machine instance
    /// </summary>
    /// <returns>The State Machine</returns>
    public StateMachine<Enemy> StateMachine {
        get { return _stateMachine; }
    }

    /// <summary>
    ///     Convert the enemy to a TemporaryCombatData object for damage resolution
    /// </summary>
    /// <returns>A TemporaryCombatData object</returns>
    public TemporaryCombatData ToTemporaryCombatData() {
        _combatData = GetComponent<CombatData>();
        return _combatData.ToTemporaryCombatData();
    }

    /// <summary>
    ///     Attack something
    /// </summary>
    /// <param name="defender">The thing that may or may not defend itself</param>
    public void Attack(IAttackable defender) {
        // TODO this code is currently copypasta from the Player. That definitely needs to be changed.
        _combatData = GetComponent<CombatData>();
        var damage = CombatData.ComputeDamage(_combatData.ToTemporaryCombatData(), defender.ToTemporaryCombatData());
        Debug.Log(string.Format("penguin inflicts {0} damage on player", damage.DefenderDamage.HealthDamage));
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
            // TODO Add a death animation
            var mo = gameObject.GetComponent<MovingObject>();
            GameManager.GetInstance().UnregisterEnemy(mo);
            Destroy(gameObject);
        }
    }

    /// <summary>
    ///     Sets up the enemy on load and registers it with the game manager.
    /// </summary>
    private void Awake() {
        GameManager.Instance.RegisterEnemy(this);
    }

    /// <summary>
    ///     Attempts to move this enemy towards the player.
    /// </summary>
    public void SeekPlayer() {
        var playerObj = FindObjectOfType<Player>();

        var pathFinder = new SearchAStar(this,transform.position,playerObj.transform.position,
            new ManhattanDistance(playerObj.transform.position));
        var destination = pathFinder.Search();

        if (destination == null) {
            Debug.Log("Pathfinding: Enemy cannot find valid path to target! " + transform);
            return;
        }

        var direction = destination[0].Destination - (Vector2) transform.position;

        AttemptMove<Component>((int) direction.x, (int) direction.y);
    }

    /// <summary>
    ///     If this enemy can't move, see if the reason it can't move is because of the player.
    /// </summary>
    /// <typeparam name="T">The type of the component</typeparam>
    /// <param name="component">The component</param>
    protected override void OnCantMove<T>(T component) {
        if (component.CompareTag("Player")) {
            Debug.Log("Penguin attacks player");
            var player = FindObjectOfType<Player>();
            Attack(player);
        }
    }

    /// <summary>
    ///     Checks if the enemy is asleep
    /// </summary>
    /// <returns>True if asleep, false otherwise.</returns>
    public bool IsAsleep() {
        return StateMachine.IsInState(StateAsleep.getInstance());
    }
}