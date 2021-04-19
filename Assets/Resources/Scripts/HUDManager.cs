using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HUDScreens
{
    public GameObject winScreen;
}

public class HUDManager : MonoBehaviour
{
    [SerializeField] private HUDHeartsManager heartsManager;
    [SerializeField] private HUDWeaponManager weaponManager;

    [SerializeField] private HUDScreens screens;

    // Start is called before the first frame update
    void Start()
    {
        screens.winScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RefreshHealthBar(int health)
    {
        heartsManager.RefreshHealthBar(health);
    }

    public void RefreshWeaponBar(WeaponBehavior weapon)
    {
        weaponManager.RefreshWeaponBar(weapon);
    }

    public void SetWinScreen()
    {
        screens.winScreen.SetActive(true);
    }
}