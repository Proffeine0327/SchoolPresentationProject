using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bullet : MonoBehaviour
{
    private float damage;
    private float speed;
    private Vector2 dir;

    private void Start() 
    {
        Destroy(gameObject, 2);
    }

    public void Init(float speed, Vector2 dir, float damage)
    {
        this.speed = speed;
        this.dir = dir;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        this.damage = damage;
    }

    private void Update()
    {
        var beforePos = transform.position;
        transform.Translate(dir * speed * Time.deltaTime, Space.World);

        var hits = 
        Physics2D.LinecastAll(beforePos, transform.position).
        OrderBy(item => Vector2.Distance(item.transform.position, transform.position)).
        ToArray();

        foreach(var hit in hits)
        {
            if(hit.collider.CompareTag("Enemy"))
                hit.collider.GetComponent<Enemy>().Damage(damage);

            Destroy(gameObject);
            break;
        }
    }
}
