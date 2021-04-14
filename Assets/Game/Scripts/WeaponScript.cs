using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class WeaponScript : MonoBehaviour
{
    public PlayerController playerController;
    public Weapon Weapon { get; set; }// = maybe pistol
    
    public Buff<int> BulletNumberBuff { get; set; }
    public Buff<float> BulletVelocityBuff { get; set; }
    public Buff<float> DamageBuff { get; set; }
    public Buff<float> FireRateBuff { get; set; }
    public Buff<float> RangeBuff { get; set; }
    public Buff<int> SpreadDegreeBuff { get; set; }

    // public int BulletNumber => BulletNumberBuff.ApplyBuff(Weapon.BulletNumber);
    // public float BulletVelocity => BulletVelocityBuff.ApplyBuff(Weapon.BulletVelocity);
    // public float Damage => DamageBuff.ApplyBuff(Weapon.Damage);
    // public float FireRate => FireRateBuff.ApplyBuff(Weapon.FireRate);
    // public float Range => RangeBuff.ApplyBuff(Weapon.Range);
    // public int SpreadDegree => SpreadDegreeBuff.ApplyBuff(Weapon.SpreadDegree);
    
    // test code
    public int BulletNumber => Weapon.BulletNumber;
    public float BulletVelocity => Weapon.BulletVelocity;
    public float Damage => Weapon.Damage;
    public float FireRate => Weapon.FireRate;
    public float Range => Weapon.Range;
    public int SpreadDegree => Weapon.SpreadDegree;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void OnShoot();
}
