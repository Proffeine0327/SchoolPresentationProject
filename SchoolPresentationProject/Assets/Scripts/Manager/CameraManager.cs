using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private Vector2 clamp;
    private Transform target;

    public void SetTarget(Transform target) => this.target = target;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        transform.position = new Vector3
        (
            Mathf.Clamp(target.position.x, -clamp.x, clamp.x),
            Mathf.Clamp(target.position.y + 0.5f, -clamp.y, clamp.y),
            -10
        );
    }
}
