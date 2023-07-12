using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Header("Move")]
    [SerializeField] private float moveSpeed;
    [Header("Hp")]
    [SerializeField] private float maxHp;
    [Header("Gun")]
    [SerializeField] private Transform gunPos;
    [SerializeField] private GameObject startRifle;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Gun curGun;
    private int curLvl;
    private int curGunLvl;
    private float maxExp;
    private float curExp;
    private float curHp;

    public Gun CurGun => curGun;
    public int CurLvl => curLvl;
    public int CurGunLvl => curGunLvl;
    public float Curhp => curHp;
    public float MaxHp => maxHp;
    public float MaxExp => maxExp;
    public float CurExp => curExp;

    public void UpgradeGun(GameObject prefeb)
    {
        if(curGun != null) Destroy(curGun.gameObject);
        curGun = Instantiate(prefeb, gunPos).GetComponent<Gun>();
        curGun.transform.localPosition = Vector3.zero;
        curGunLvl++;
    }

    public void GetExp(float amount)
    {
        curExp += amount;
    }

    public void Damage(float amount)
    {
        curHp -= amount;
    }

    private void Awake()
    {
        Instance = this;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        curHp = maxHp;
        maxExp = 100;
        curLvl = 1;
        UpgradeGun(startRifle);
    }

    private void Update()
    {
        if(AbilitySelectUI.Instance.IsDisplayingUI) return;

        //move
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var isMove = h != 0 || v != 0;

        rb.velocity = new Vector2(h, v).normalized * moveSpeed;

        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, -14.45f, 14.45f),
            Mathf.Clamp(transform.position.y, -9.673f, 8.97f),
            0
        );

        //mouse
        var mouseRelativePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - curGun.transform.position;
        var mouseXNormalized = mouseRelativePos.x > 0 ? 1 : -1;

        sr.flipX = mouseXNormalized < 0;

        //animator
        anim.SetBool("isMove", isMove);
        anim.SetFloat("moveDir", (h != 0 ? h : mouseXNormalized) * mouseXNormalized);

        //gun
        curGun.SetRotation(Mathf.Atan2(mouseRelativePos.y, mouseRelativePos.x) * Mathf.Rad2Deg);

        if (Input.GetMouseButton(0)) curGun.Shoot();
        if (Input.GetKeyDown(KeyCode.R)) curGun.Reload();

        //exp
        if(curExp >= maxExp)
        {
            curExp -= maxExp;
            curLvl++;
            maxExp = curLvl * 100;

            AbilitySelectUI.Instance.DisplayUI();
        }
    }
}
