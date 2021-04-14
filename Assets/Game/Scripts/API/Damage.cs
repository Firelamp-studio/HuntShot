public class Damage
{
    public readonly float damage;
    public readonly PlayerController owner;

    public Damage(float damage, PlayerController owner)
    {
        this.damage = damage;
        this.owner = owner;
    }
}