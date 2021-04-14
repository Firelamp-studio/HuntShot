using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableScript : MonoBehaviour
{
    private Item _item;

    public Item Item
    {
        get => _item;
        set
        {
            mesh.enabled = value != null;
            if(value != null)
                mesh.material.SetTexture("_MainTex", value.Texture);
            
            _item = value;
        }
    }

    [SerializeField] private MeshRenderer mesh;


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
        mesh.material.SetTexture("_MainTex", itemTexture);
        Item = new Weapon(itemName, itemTexture, BulletNumber, BulletVelocity, Damage, FireRate, Range, SpreadDegree);
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        var playerController = other.gameObject.GetComponent<PlayerController>();
        if (playerController == null || Item == null)
            return;

        var weapon = (Weapon) Item;
        if(weapon == null)
            return;

        Item = playerController.Weapon;
        playerController.Weapon = weapon;
    }
}