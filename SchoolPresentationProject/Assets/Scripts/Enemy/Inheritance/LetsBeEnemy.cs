using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetsBeEnemy : Enemy
{
    [SerializeField] private float attackRange;

    protected override void Attack()
    {
        if(Vector2.Distance(transform.position, Player.Instance.transform.position) < attackRange)
            Player.Instance.Damage(damage);
    }

    protected override void EndAttack()
    {
        isAttacking = false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
