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

    protected float curHp;
    protected bool isAttacking;

    public float CurHp => curHp;
    public float MaxHp => maxHp;

    public void Damage(float amount)
    {
        curHp -= amount;
    }

    protected abstract IEnumerator Attack();

    protected virtual void OnDie()
    {
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
    }

    protected virtual void Update()
    {
        if (curHp <= 0)
        {
            OnDie();
            Destroy(gameObject);
            return;
        }

        if (!isAttacking)
        {
            var dir = Player.Instance.transform.position - transform.position;
            dir = dir.normalized;
            transform.Translate(dir * moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, Player.Instance.transform.position) < recognizeRange)
            {
                isAttacking = true;
                StartCoroutine(AttackRoutine());
            }
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, recognizeRange);
    }
}
