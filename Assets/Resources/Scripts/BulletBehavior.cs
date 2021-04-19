using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public PlayerController owner;
    
    [SerializeField] protected MeshRenderer bodyMesh;
    [SerializeField] protected ParticleSystem contactParticlePrefab;

    [SerializeField] private int damage;
    
    [SerializeField, Header("SFX"), EventRef]
    private string hitSFX;
    public int Damage
    {
        get => damage;
        set => damage = value;
    }
    
    [SerializeField] private float lifeTime;
    public float LifeTime
    {
        get => lifeTime;
        set => lifeTime = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        bodyMesh.material.SetColor("_Color", owner.Color);
        StartCoroutine(DestroyAfterLifeTime());
    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if((!other.CompareTag("MapObject") && !other.CompareTag("Player")) || other.GetComponent<BulletBehavior>() != null)
            return;

        RuntimeManager.PlayOneShot(hitSFX, transform.position);
        
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.OnDamage(new Damage(damage, owner));
        }
        
        var contactParticle = Instantiate(contactParticlePrefab, transform.position, Quaternion.identity);
        var particleMain = contactParticle.main;
        particleMain.startColor = owner.Color;
        contactParticle.Play();

        Destroy(gameObject);
    }

    IEnumerator DestroyAfterLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);

        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _rigidbody.useGravity = true;

        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        //TODO: DESTROY ANIMATION
    }
}