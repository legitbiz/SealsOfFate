using Assets.Scripts;

public class HealthPotion : Consumable {
    // public int Modifier;
    // public int HealthMod;

    public override void Consume() {
        //Sets the Player's health to 100. This is currently the max, but is hard coded since there was not a
        //Variable which indicates the max health of the player.
        GameManager.Instance.PlayerHealth = 100;
    }
}