using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSixEnemy : Enemy
{
    [SerializeField] private float attackRange;

    protected override IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.3f);
        
        if(Vector2.Distance(transform.position, Player.Instance.transform.position) < attackRange)
            Player.Instance.Damage(damage);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
