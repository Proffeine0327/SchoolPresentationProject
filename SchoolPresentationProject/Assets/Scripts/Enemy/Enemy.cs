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
    protected float curHp;
    protected float curInvincibleTime;
    protected bool isAttacking;

    public float CurHp => curHp;
    public float MaxHp => maxHp;

    public void Damage(float amount)
    {
        hitAnimationTime = 0.05f;
        curHp -= amount;
    }

    protected abstract IEnumerator Attack();

    protected virtual void OnDie()
    {
        Player.Instance.KilledEnemy();
        for (var rand = Random.Range(2, 5); rand >= 0; rand--)
            Instantiate(expOrbPrefeb, (Vector2)transform.position + (Random.insideUnitCircle * 0.7f), Quaternion.Euler(0, 0, 45));
    }

    private IEnumerator AttackRoutine()
    {
        yield return StartCoroutine(Attack());
        yield return new WaitForSeconds(attackCooltime);
        isAttacking = false;
    }

    protected virtual void Awake()
    {
        curHp = maxHp;

        sr = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (curHp <= 0)
        {
            OnDie();
            Destroy(gameObject);
            return;
        }

        if (hitAnimationTime > 0)
        {
            hitAnimationTime -= Time.deltaTime;
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }

        if (!isAttacking)
        {
            var dir = Player.Instance.transform.position - transform.position;
            dir = dir.normalized;
            rb2d.velocity = dir * moveSpeed;
            if (Vector2.Distance(transform.position, Player.Instance.transform.position) < recognizeRange)
            {
                isAttacking = true;
                StartCoroutine(AttackRoutine());
            }
        }
        else
        {
            rb2d.velocity = Vector2.zero;
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
