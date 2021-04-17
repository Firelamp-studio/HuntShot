using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteAlways]
public class PickableScript : MonoBehaviour
{
    private Item _item;

    public Item Item
    {
        get => _item;
        set
        {
            mesh.enabled = value != null;
            if (value != null)
                mesh.material.SetTexture("_MainTex", value.Texture);

            _item = value;
        }
    }

    [SerializeField] private MeshRenderer mesh;


    [SerializeField] private string itemName;
    [SerializeField] private Texture2D itemTexture;

    //Test code
    [SerializeField] private float BulletLifeTime;
    [SerializeField] private int BulletNumber;
    [SerializeField] private float BulletVelocity;
    [SerializeField] private float Damage;
    [SerializeField] private float FireRate;
    [SerializeField] private int SpreadDegree;

    private void OnValidate()
    {
        var tex = itemTexture == null ? Resources.Load<Texture2D>("Textures/PickableIcon") : itemTexture;

        // var tempMaterial = new Material(mesh.sharedMaterial);
        // tempMaterial.SetTexture("_MainTex", itemTexture);
        // mesh.sharedMaterial = tempMaterial;
        mesh
            .sharedMaterial
            .SetTexture("_MainTex",
                tex);
    }

    void Start()
    {
        if (!Application.isPlaying) return;

        mesh.material.SetTexture("_MainTex", itemTexture);
        Item = new Weapon(itemName, itemTexture, BulletLifeTime, BulletNumber, BulletVelocity, Damage, FireRate,
            SpreadDegree);
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
        if (weapon == null)
            return;

        Item = playerController.Weapon;
        playerController.Weapon = weapon;
    }
}