using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bullet : MonoBehaviour
{
    private int throughCount;
    private float damage;
    private float speed;
    private Vector2 dir;

    public float Damage => damage;

    private void Start()
    {
        Destroy(gameObject, 2);
    }

    public void Hitted()
    {
        throughCount--;
    }

    public void Init(float speed, Vector2 dir, float damage)
    {
        this.speed = speed;
        this.dir = dir;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        this.damage = damage;
        throughCount = Player.Instance.AbilityLvls[(int)AbilityType.armor_piercing] + 1;
    }

    private void Update()
    {
        if(throughCount <= 0)
        {
            Destroy(gameObject);
            return;
        }

        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }
}
