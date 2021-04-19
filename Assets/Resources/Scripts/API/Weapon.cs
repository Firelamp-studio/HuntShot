using System;
using FMODUnity;
using UnityEngine;

[Serializable]
public class Weapon : Item
{
    public float BulletLifeTime;
    public int ShotsNumber;
    public float BulletVelocity;
    public int Damage;
    public float ShotDelay;
    public int SpreadDegree;
    public int DefaultMagazine;
    public int Reload;
    public float ReloadTime;
    public int Capacitor;
    public int Magazine;
    
    [Header("SFX"), EventRef]
    public string shotSFX;
    [EventRef] public string reloadSFX;


    public Weapon(Weapon weapon) : base(weapon.Name, weapon.Texture)
    {
        BulletLifeTime = weapon.BulletLifeTime;
        ShotsNumber = weapon.ShotsNumber;
        BulletVelocity = weapon.BulletVelocity;
        Damage = weapon.Damage;
        ShotDelay = weapon.ShotDelay;
        SpreadDegree = weapon.SpreadDegree;
        DefaultMagazine = weapon.DefaultMagazine;
        Reload = weapon.Reload;
        ReloadTime = weapon.ReloadTime;

        Magazine = DefaultMagazine;
        Capacitor = 0;

        shotSFX = weapon.shotSFX;
        reloadSFX = weapon.reloadSFX;
    }


    public Weapon()
    {
        Magazine = DefaultMagazine;
        Capacitor = 0;
    }

    public Weapon(string name, Texture2D texture, float bulletLifeTime, int shotsNumber, float bulletVelocity, int damage, float shotDelay, int spreadDegree, int defaultMagazine, int reload, float reloadTime) : base(name, texture)
    {
        BulletLifeTime = bulletLifeTime;
        ShotsNumber = shotsNumber;
        BulletVelocity = bulletVelocity;
        Damage = damage;
        ShotDelay = shotDelay;
        SpreadDegree = spreadDegree;
        DefaultMagazine = defaultMagazine;
        Reload = reload;
        ReloadTime = reloadTime;
        
        Capacitor = 0;
        Magazine = DefaultMagazine;
    }
}