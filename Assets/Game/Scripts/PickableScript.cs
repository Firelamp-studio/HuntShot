using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableScript : MonoBehaviour
{
    public Item Item;
    
    [SerializeField] private string itemName;
    [SerializeField] private Texture itemTexture;
    
    //Test code
    [SerializeField] private int BulletNumber;
    [SerializeField] private float BulletVelocity;
    [SerializeField] private float Damage;
    [SerializeField] private float FireRate;
    [SerializeField] private float Range;
    [SerializeField] private int SpreadDegree;
    
    void Start()
    {
        Item = new Weapon(itemName, itemTexture, BulletNumber, BulletVelocity, Damage, FireRate, Range, SpreadDegree);
    }

    void Update()
    {
        
    }
}
