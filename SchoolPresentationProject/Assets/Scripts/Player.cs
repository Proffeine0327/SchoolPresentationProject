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
    [SerializeField] private Gun gun;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float curHp;

    private void Awake()
    {
        Instance = this;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        curHp = maxHp;
    }

    private void Update()
    {
        //move
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var isMove = h != 0 || v != 0;

        rb.velocity = new Vector2(h, v).normalized * moveSpeed;

        //mouse
        var mouseRelativePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gun.transform.position;
        var mouseXNormalized = mouseRelativePos.x > 0 ? 1 : -1;

        sr.flipX = mouseXNormalized < 0;

        //animator
        anim.SetBool("isMove", isMove);
        anim.SetFloat("moveDir", (h != 0 ? h : mouseXNormalized) * mouseXNormalized);

        //gun
        gun.SetRotation(Mathf.Atan2(mouseRelativePos.y, mouseRelativePos.x) * Mathf.Rad2Deg);

        if(Input.GetMouseButton(0)) gun.Shoot();
    }
}
