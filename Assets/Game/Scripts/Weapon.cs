using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    public PlayerController playerController;
    
    public Buff<float> FireRateBuff { get; set; }
    public Buff<float> RangeBuff { get; set; }
    
    [SerializeField]
    private float fireRate;
    public float FireRate => FireRateBuff.ApplyBuff(fireRate);
    
    private 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnShoot()
    {
        var bulletInstance = Instantiate(gameObject, transform);
        bulletInstance.GetComponent<Rigidbody>().velocity =
            transform.forward * fireRate + GetComponent<Rigidbody>().velocity;
        bulletInstance.transform.rotation = transform.rotation;
        bulletInstance.GetComponent<Bullet>().owner = playerController;
        
        Physics.IgnoreCollision(GetComponent<Collider>(), bulletInstance.GetComponent<Collider>(), true);
    }
    
    
}
