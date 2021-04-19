using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDWeaponManager : MonoBehaviour
{
    private TextMeshProUGUI weaponText;
    private WeaponBehavior currentWeapon;

    void Start()
    {
        weaponText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // if(currentWeapon == null || currentWeapon.CurrentReloadTime <= 0)
        //     return;

        // weaponText.text = "<size=20> R:" + currentWeapon.CurrentReloadTime + "/" + currentWeapon.ReloadTime + " | " + currentWeapon.Weapon.Name + "</size> " + currentWeapon.Weapon.Capacitor + "/" + currentWeapon.Weapon.Magazine + " <size=15>[" + currentWeapon.Weapon + "]</size>";
    }

    public void RefreshWeaponBar(WeaponBehavior weapon)
    {
        if (weaponText == null)
            return;

        currentWeapon = weapon;

        if (weapon.Weapon != null)
            weaponText.text = "<size=20>" + weapon.Weapon.Name + "</size> " +
                              (weapon.BulletNumber > 1 ? "<size=15>(" + weapon.BulletNumber + ")</size>" : "") +
                              weapon.Weapon.Capacitor + "/" + weapon.Weapon.Magazine +
                              (weapon.Reload > 1 ? "<size=15>[" + weapon.Reload + "]</size>" : "");
        else
            weaponText.text = "<size=20>Empty</size>";
    }
}