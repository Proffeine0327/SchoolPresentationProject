using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Gun
{
    public override void Reload()
    {
        if(isReloading) return;

        isReloading = true;
        this.Invoke(() =>
        {
            isReloading = false;
            curAmmo = maxAmmo;
        }, reloadTime);
    }

    public override void Shoot()
    {
        if(isReloading) return;
        if(curShootTime > 0) return;

        if (curAmmo <= 0)
        {
            Reload();
            return;
        }

        var bullet = Instantiate(bulletPrefeb, bulletSpawnPos.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.Init
        (
            bulletSpeed,
            new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad))
        );

        curAmmo--;
        curShootTime = shootTime;
    }
    
    protected override void Update()
    {
        base.Update();
        if(curShootTime > 0) curShootTime -= Time.deltaTime;
    }
}
