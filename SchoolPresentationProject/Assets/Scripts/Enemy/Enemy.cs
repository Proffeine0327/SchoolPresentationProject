using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float maxHp;
    [SerializeField] protected float damage;
    [SerializeField] protected float attackCooltime;
    [SerializeField] protected float recognizeRange;
    [SerializeField] protected GameObject expOrbPrefeb;

    private SpriteRenderer sr;
    private float hitAnimationTime;

    protected Rigidbody2D rb2d;
    protected Animator anim;
    protected float curHp;
    protected float curInvincibleTime;
    protected bool isAttacking;
    protected bool isDie;

    public float CurHp => curHp;
    public float MaxHp => maxHp;

    public void Damage(float amount)
    {
        hitAnimationTime = 0.05f;
        curHp -= amount;
    }

    protected abstract void Attack();
    protected abstract void EndAttack();

    protected virtual void OnDie()
    {
        SingletonManager.GetSingleton<Player>().KilledEnemy();
        for (var rand = Random.Range(2, 5); rand >= 0; rand--)
            Instantiate(expOrbPrefeb, (Vector2)transform.position + (Random.insideUnitCircle * 0.7f), Quaternion.Euler(0, 0, 45));
    }

    protected virtual void Awake()
    {
        curHp = maxHp;

        sr = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if(isDie) return;

        if (curHp <= 0)
        {
            OnDie();
            Destroy(gameObject);
            return;
        }

        if (hitAnimationTime > 0)
        {
            hitAnimationTime -= Time.deltaTime;
            sr.color = Color.red;
        }
        else
        {
            sr.color = Color.white;
        }

        if (!isAttacking)
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            var dir = SingletonManager.GetSingleton<Player>().transform.position - transform.position;
            sr.flipX = dir.x <= 0;
            dir = dir.normalized;
            rb2d.velocity = dir * moveSpeed;
            if (Vector2.Distance(transform.position, SingletonManager.GetSingleton<Player>().transform.position) < recognizeRange)
            {
                isAttacking = true;
                anim.SetTrigger("attack");
            }
        }
        else
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (curInvincibleTime > 0) curInvincibleTime -= Time.deltaTime;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            var bullet = other.GetComponent<Bullet>();
            bullet.Hitted();
            Damage(bullet.Damage);
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, recognizeRange);
    }
}
