using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private GameObject explosionParticlePrefeb;

    public void ThrowBomb(Vector2 start, Vector2 middle, Vector2 end, float t)
    => StartCoroutine(BombAnimation(start, middle, end, t));

    private IEnumerator BombAnimation(Vector2 start, Vector2 middle, Vector2 end, float t)
    {
        var leftRotate = Random.Range(0, 2) > 0;
        var rotateSpeed = Random.Range(480, 720);
        for (float time = 0; time < t; time += Time.deltaTime)
        {
            transform.Rotate(new Vector3(0, 0, (leftRotate ? -1 : 1) * rotateSpeed) * Time.deltaTime);
            transform.position = Bezier.GetBezier(time / t, start, middle, end);
            yield return null;
        }

        var hits = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
                hit.GetComponent<Enemy>().Damage(damage);
        }
        Instantiate(explosionParticlePrefeb, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
