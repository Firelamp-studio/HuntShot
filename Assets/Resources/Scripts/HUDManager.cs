using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HUDScreens
{
    public GameObject gameOverScreen;
}

public class HUDManager : MonoBehaviour
{
    [SerializeField] private HUDHeartsManager heartsManager;
    [SerializeField] private HUDWeaponManager weaponManager;
    
    [SerializeField] private HUDScreens screens;

    // Start is called before the first frame update
    void Start()
    {
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

    public void SetGameOverScreen()
    {
        screens.gameOverScreen.SetActive(true);
    }
}