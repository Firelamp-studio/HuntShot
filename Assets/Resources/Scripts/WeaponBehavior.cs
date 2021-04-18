using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class WeaponBehavior : MonoBehaviour
{
    public HUDManager hudManager;
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected HUDReloadIcon reloadIcon;

    public MeshRenderer Mesh => mesh;
    [SerializeField] private MeshRenderer mesh;

    private Weapon _weapon;

    public Weapon Weapon
    {
        get => _weapon;

        set
        {
            StopReload();

            _weapon = value;

            if (value == null)
            {
                mesh.enabled = false;
                mesh.material.SetTexture("_MainTex", null);
            }
            else
            {
                mesh.enabled = true;
                mesh.material.SetTexture("_MainTex", _weapon.Texture);
                WeaponCapacitor = _weapon.Capacitor;

                if (_weapon.Capacitor <= 0 && _weapon.Magazine > 0)
                    StartReload();
            }

            hudManager.RefreshWeaponBar(this);
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
    public float BulletLifeTime => Weapon?.BulletLifeTime ?? 0;
    public int BulletNumber => Weapon?.ShotsNumber ?? 0;
    public float BulletVelocity => Weapon?.BulletVelocity ?? 0;
    public int Damage => Weapon?.Damage ?? 0;
    public float FireRate => Weapon?.ShotDelay ?? 0;
    public int SpreadDegree => Weapon?.SpreadDegree ?? 0;
    public int Reload => Weapon?.Reload ?? 0;
    public float ReloadTime => Weapon?.ReloadTime ?? 0;

    public int WeaponCapacitor
    {
        get => _weapon?.Capacitor ?? 0;
        set
        {
            _weapon.Capacitor = value;
            hudManager.RefreshWeaponBar(this);
        }
    }

    private float _currentReloadTime;
    public float CurrentReloadTime => _currentReloadTime;

    // Start is called before the first frame update
    void Start()
    {
        StopReload();
        Weapon = null;
    }

    protected virtual void Update()
    {
        if (_currentReloadTime > 0)
            _currentReloadTime -= Time.deltaTime;
        else if (_currentReloadTime < 0)
            DoReload();
    }

    public void OnShootStart()
    {
        if (WeaponCapacitor > 0 && _currentReloadTime <= 0)
        {
            if (Shoot())
                WeaponCapacitor -= 1;
        }
        else if (_currentReloadTime <= 0)
            StartReload();

        if (WeaponCapacitor <= 0 && Weapon?.Magazine <= 0)
        {
            Weapon = null;
        }
        else if (WeaponCapacitor <= 0)
        {
            StartReload();
        }
    }

    public void OnShootCancel()
    {
    }

    public abstract bool Shoot();

    public void StartReload()
    {
        if (WeaponCapacitor < Reload && Weapon.Magazine > 0 && _currentReloadTime <= 0)
        {
            _currentReloadTime = ReloadTime;
            transform.localRotation = Quaternion.AngleAxis(45, Vector3.up);
            hudManager.RefreshWeaponBar(this);
            reloadIcon.EnableReloadIcon(() => CurrentReloadTime, ReloadTime);
        }
    }

    private void DoReload()
    {
        if (Weapon.Magazine > 0)
        {
            Weapon.Magazine -= 1;
            WeaponCapacitor = Reload;
        }

        StopReload();
    }

    private void StopReload()
    {
        _currentReloadTime = 0;
        transform.localRotation = Quaternion.AngleAxis(0, Vector3.up);
        reloadIcon.DisableReloadIcon();
    }
}