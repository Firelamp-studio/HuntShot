using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class BulletWeaponBehavior : WeaponBehavior
{
    private bool _canShoot = true;

    [SerializeField] private GameObject bullet;

    protected override void Update()
    {
        base.Update();
    }

    public override bool Shoot()
    {
        if (Weapon is null || !_canShoot || BulletNumber == 0) return false;

        var bulletInstances = new List<GameObject>();

        if (SpreadDegree > 0)
        {
            if (BulletNumber == 1)
            {
                Debug.LogError(
                    $"Sviluppatore cane, mi stai sparando un proiettile mentre hai lo spread a {SpreadDegree} gradi");
                return false;
            }

            var shotSpacingDegree = SpreadDegree / (BulletNumber - 1);
            var currentSpacingDegree = SpreadDegree / 2 * -1;

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
                return false;
            }

            var bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletInstance.GetComponent<Rigidbody>().velocity =
                transform.forward * BulletVelocity + playerController.Velocity;
            bulletInstance.transform.rotation = transform.rotation;

            bulletInstances.Add(bulletInstance);
        }


        bulletInstances.ForEach(go =>
        {
            var bulletScript = go.GetComponent<BulletBehavior>();

            bulletScript.owner = playerController;
            bulletScript.Damage = Damage;
            bulletScript.LifeTime = BulletLifeTime;

            Physics.IgnoreCollision(playerController.GetComponent<Collider>(), go.GetComponent<Collider>(), true);
        });

        _canShoot = false;
        StartCoroutine(EnableShootingAfterDelay());
        return true;
    }

    private IEnumerator EnableShootingAfterDelay()
    {
        yield return new WaitForSeconds(FireRate);

        _canShoot = true;
    }
}