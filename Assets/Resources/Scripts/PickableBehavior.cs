using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteAlways]
public class PickableBehavior : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private HUDReloadIcon reloadIcon;
    [SerializeField] private float rechargeTime;
    private float _currentRechargeTime;

    //TEST VAR
    [SerializeField] private Weapon weapon;

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

    private void OnValidate()
    {
        if(mesh == null)
            return;
        
        var tex =
            weapon.Texture == null ? Resources.Load<Texture2D>("Textures/PickableIcon") : weapon.Texture;

        var tempMaterial = new Material(mesh.sharedMaterial);
        tempMaterial.SetTexture("_MainTex", tex);
        mesh.sharedMaterial = tempMaterial;
    }

    void Start()
    {
        if (!Application.isPlaying) return;

        _currentRechargeTime = 0;
        reloadIcon.DisableReloadIcon();
        Item = new Weapon(weapon);
    }

    void Update()
    {
        if (_currentRechargeTime > 0)
            _currentRechargeTime -= Time.deltaTime;
        else if (_currentRechargeTime < 0)
            OnRecharge();
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerController = other.gameObject.GetComponent<PlayerController>();
        if (playerController == null || Item == null)
            return;

        var weapon = (Weapon) Item;
        if (weapon == null)
            return;

        Item = playerController.Weapon;
        playerController.Weapon = weapon;

        if (Item == null)
        {
            _currentRechargeTime = rechargeTime;
            reloadIcon.EnableReloadIcon(() => _currentRechargeTime, rechargeTime);
        }
    }

    private void OnRecharge()
    {
        _currentRechargeTime = 0;
        reloadIcon.DisableReloadIcon();

        Item = new Weapon(weapon);
    }
}