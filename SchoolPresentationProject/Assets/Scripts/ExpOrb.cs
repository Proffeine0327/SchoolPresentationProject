using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    [SerializeField] private float animationTime;

    private void Start()
    {
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        var player = Player.Instance;
        var startpos = transform.position;
        var rot = Random.Range(0, 360);
        var secondpos = transform.position + (new Vector3(Mathf.Cos(rot * Mathf.Deg2Rad), Mathf.Sin(rot * Mathf.Deg2Rad)) * 2);

        for (float t = 0; t < animationTime; t += Time.deltaTime)
        {
            transform.position =
                Bezier.GetBezier(t / animationTime, startpos, secondpos, player.transform.position);
            yield return null;
        }

        Player.Instance.GetExp(10);
        Destroy(gameObject);
    }
}
