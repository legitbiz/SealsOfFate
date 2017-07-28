namespace Combat
{
    public abstract class DefenseEffect : UnityEngine.Object {
        public abstract void Apply(ref CombatData cd);
    }
}
