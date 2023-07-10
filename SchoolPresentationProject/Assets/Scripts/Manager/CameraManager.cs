using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 clamp;
    
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
