using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] protected GameObject bulletPrefeb;
    [SerializeField] protected Transform bulletSpawnPos;
    [Header("Var")]
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected float shootTime;
    [SerializeField] protected float reloadTime;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float damage;
    [Header("UI")]
    [SerializeField] protected string gunName;
    [SerializeField] protected Sprite gunUIImage;

    protected SpriteRenderer sr;
    protected int curAmmo;
    protected float curShootTime;
    protected float rotation;
    protected bool isReloading;

    public string GunName => gunName;
    public int MaxAmmo => maxAmmo;
    public int CurAmmo => curAmmo;
    public bool IsReloading => isReloading;
    public Sprite GunUIImage => gunUIImage;

    public abstract void Shoot();
    public abstract void Reload();

    public void SetRotation(float rotation)
    {
        this.rotation = rotation;
    }

    protected virtual void Awake()
    {
        curAmmo = maxAmmo;
        sr = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        sr.flipY = rotation is >= 90 or <= -90;
    }
}
