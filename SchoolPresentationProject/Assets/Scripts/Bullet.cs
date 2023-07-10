using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private Vector2 dir;

    private void Start() 
    {
        Destroy(gameObject, 3);
    }

    public void Init(float speed, Vector2 dir)
    {
        this.speed = speed;
        this.dir = dir;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }

    private void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }
}
