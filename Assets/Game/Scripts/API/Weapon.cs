public class Weapon : Item
{
    public readonly int BulletNumber;
    public readonly float BulletVelocity;
    public readonly float Damage;
    public readonly float FireRate;
    public readonly float Range;
    public readonly int SpreadDegree;

    public Weapon(int bulletNumber, float bulletVelocity, float damage, float fireRate, float range, int spreadDegree)
    {
        BulletNumber = bulletNumber;
        BulletVelocity = bulletVelocity;
        Damage = damage;
        FireRate = fireRate;
        Range = range;
        SpreadDegree = spreadDegree;
    }
}