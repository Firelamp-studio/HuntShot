using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BulletWeaponScript : WeaponScript
{
    [SerializeField] private GameObject bullet;

    public override void OnShoot()
    {
        if (Weapon is null) return;
        
        var bulletInstances = new List<GameObject>();

        if (SpreadDegree > 0)
        {
            if (BulletNumber == 1)
            {
                Debug.LogError(
                    $"Sviluppatore cane, mi stai sparando un proiettile mentre hai lo spread a {SpreadDegree} gradi");
                return;
            }

            var shotSpacingDegree = SpreadDegree / (BulletNumber - 1);
            var currentSpacingDegree = shotSpacingDegree - SpreadDegree / 2;

            for (var i = 0; BulletNumber > i; i++)
            {
                var bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
                bulletInstance.GetComponent<Rigidbody>().velocity =
                    Quaternion.AngleAxis(currentSpacingDegree, Vector3.up) * transform.forward * BulletVelocity +
                    playerController.Velocity;
                bulletInstance.transform.rotation = transform.rotation;

                bulletInstances.Add(bulletInstance);
                currentSpacingDegree += shotSpacingDegree;
            }
        }
        else
        {
            if (BulletNumber > 1)
            {
                Debug.LogError(
                    $"Sviluppatore cane, mi stai sparando {BulletNumber} proiettili insieme mentre hai lo spread 0");
                return;
            }

            var bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletInstance.GetComponent<Rigidbody>().velocity =
                transform.forward * BulletVelocity + playerController.Velocity;
            bulletInstance.transform.rotation = transform.rotation;
            
            bulletInstances.Add(bulletInstance);
        }


        bulletInstances.ForEach(go =>
        {
            go.GetComponent<BulletScript>().owner = playerController;

            Physics.IgnoreCollision(playerController.GetComponent<Collider>(), go.GetComponent<Collider>(), true);
        });
    }
}