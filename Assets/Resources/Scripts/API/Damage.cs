public class Damage
{
    public readonly int damage;
    public readonly PlayerController owner;

    public Damage(int damage, PlayerController owner)
    {
        this.damage = damage;
        this.owner = owner;
    }
}