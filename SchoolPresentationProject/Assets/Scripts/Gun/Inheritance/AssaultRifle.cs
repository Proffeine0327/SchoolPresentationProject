using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Gun
{
    //singleton
    private ReloadUI reloadUI => SingletonManager.GetSingleton<ReloadUI>();

    [Header("Animation")]
    [SerializeField] private Sprite[] sprites;

    private int animationIndex;
    private float curAnimationTime;

    private void Start() 
    {
        sr.sprite = sprites[0];
    }
    
    public override void Reload()
    {
        if(isReloading) return;
        if(curAmmo == maxAmmo) return;

        SoundManager.Instance.PlaySound(Sound.GunReload); 
        isReloading = true;
        reloadUI.SetTime(reloadTime);
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

        SoundManager.Instance.PlaySound(Sound.GunShot);
        var bullet = Instantiate(bulletPrefeb, bulletSpawnPos.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.Init
        (
            bulletSpeed,
            new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad)),
            damage
        );

        curAmmo--;
        curShootTime = shootTime;

        //flag fire animation
        animationIndex = 1;
    }
    
    protected override void Update()
    {
        base.Update();
        if(curShootTime > 0) curShootTime -= Time.deltaTime;

        //when fire
        if(animationIndex > 0)
        {
            if (curAnimationTime > 0)
            {
                curAnimationTime -= Time.deltaTime;
            }
            else
            {
                animationIndex++;
                if (animationIndex < sprites.Length) curAnimationTime = shootTime / sprites.Length;
                else animationIndex = 0;
                sr.sprite = sprites[animationIndex];
            }
        }
    }
}
