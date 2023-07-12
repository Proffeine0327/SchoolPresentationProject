using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sickel : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float damage;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(SickelMovement());
    }

    private void Update() 
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, range);
        foreach(var hit in hits)
        {
            if(hit.CompareTag("Enemy"))
                hit.GetComponent<Enemy>().Damage(damage);
        }
    }

    private IEnumerator SickelMovement()
    {
        while (true)
        {
            var rot = Random.Range(0, 360);
            var randpos1 = new Vector2(Mathf.Sin(rot * Mathf.Deg2Rad), Mathf.Cos(rot * Mathf.Deg2Rad)) * 13f;
            rot += (Random.Range(0, 2) > 0 ? -1 : 1) * 90;
            var randpos2 = new Vector2(Mathf.Sin(rot * Mathf.Deg2Rad), Mathf.Cos(rot * Mathf.Deg2Rad)) * 13f;

            var isLeft = Random.Range(0, 2);
            for (float t = 0; t < 3; t += Time.deltaTime)
            {
                transform.localPosition = Bezier.GetBezier(t / 3, Vector2.zero, randpos1, randpos2, Vector2.zero);
                transform.Rotate(new Vector3(0, 0, (isLeft > 0 ? -1 : 1) * 720 * Time.deltaTime));
                yield return null;
            }

            sr.enabled = false;
            yield return new WaitForSeconds(1f);
            sr.enabled = true;
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
