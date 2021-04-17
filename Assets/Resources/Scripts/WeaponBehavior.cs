using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class WeaponBehavior : MonoBehaviour
{
    public PlayerController playerController;

    public MeshRenderer Mesh => mesh;
    [SerializeField] private MeshRenderer mesh;

    private Weapon _weapon;

    public Weapon Weapon
    {
        get => _weapon;

        set
        {
            if (value == null)
            {
                mesh.enabled = false;
                mesh.material.mainTexture = null;
                return;
            }

            _weapon = value;
            mesh.enabled = true;
            mesh.material.SetTexture("_MainTex", _weapon.Texture);
        }
    } // = maybe pistol

    public Buff<float> BulletLifeTimeBuff { get; set; }
    public Buff<int> BulletNumberBuff { get; set; }
    public Buff<float> BulletVelocityBuff { get; set; }
    public Buff<float> DamageBuff { get; set; }
    public Buff<float> FireRateBuff { get; set; }
    public Buff<int> SpreadDegreeBuff { get; set; }

    // public float BulletLifeTime => BulletLifeTimeBuff.ApplyBuff(Weapon.BulletLifeTime);
    // public int BulletNumber => BulletNumberBuff.ApplyBuff(Weapon.BulletNumber);
    // public float BulletVelocity => BulletVelocityBuff.ApplyBuff(Weapon.BulletVelocity);
    // public float Damage => DamageBuff.ApplyBuff(Weapon.Damage);
    // public float FireRate => FireRateBuff.ApplyBuff(Weapon.FireRate);
    // public float Range => RangeBuff.ApplyBuff(Weapon.Range);
    // public int SpreadDegree => SpreadDegreeBuff.ApplyBuff(Weapon.SpreadDegree);

    // test code
    public float BulletLifeTime => Weapon.BulletLifeTime;
    public int BulletNumber => Weapon.BulletNumber;
    public float BulletVelocity => Weapon.BulletVelocity;
    public float Damage => Weapon.Damage;
    public float FireRate => Weapon.FireRate;
    public int SpreadDegree => Weapon.SpreadDegree;

    // Start is called before the first frame update
    void Start()
    {
        mesh.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public abstract void OnShoot();
}