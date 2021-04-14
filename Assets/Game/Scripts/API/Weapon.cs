using UnityEngine;

public class Weapon : Item
{
    public readonly float BulletLifeTime;
    public readonly int BulletNumber;
    public readonly float BulletVelocity;
    public readonly float Damage;
    public readonly float FireRate;
    public readonly int SpreadDegree;

    public Weapon(string name, Texture texture,
        float bulletLifeTime, int bulletNumber, float bulletVelocity, float damage, float fireRate, int spreadDegree)
        : base(name, texture)
    {
        BulletNumber = bulletNumber;
        BulletVelocity = bulletVelocity;
        Damage = damage;
        FireRate = fireRate;
        BulletLifeTime = bulletLifeTime;
        SpreadDegree = spreadDegree;
    }
}