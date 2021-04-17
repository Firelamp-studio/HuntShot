using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public PlayerController owner;

    [SerializeField] private float _damage;
    public float Damage
    {
        get => _damage;
        set => _damage = value;
    }
    
    [SerializeField] private float _lifeTime;
    public float LifeTime
    {
        get => _lifeTime;
        set => _lifeTime = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(DestroyAfterLifeTime());
    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.OnDamage(new Damage(_damage, owner));
            Destroy(gameObject);
        }

        if(!other.CompareTag("MapObject") || other.GetComponent<BulletBehavior>() != null)
            return;
        
        Destroy(gameObject);
    }

    IEnumerator DestroyAfterLifeTime()
    {
        yield return new WaitForSeconds(_lifeTime);

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