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

    private void Start()
    {
        var camera = GetComponent<Camera>();
        var r = camera.rect;
        var scaleheight = ((float)Screen.width / Screen.height) / (16f / 9f);
        var scalewidth = 1f / scaleheight;

        if (scaleheight < 1f)
        {
            r.height = scaleheight;
            r.y = (1f - scaleheight) / 2f;
        }
        else
        {
            r.width = scalewidth;
            r.x = (1f - scalewidth) / 2f;
        }

        camera.rect = r;
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3
            (
                Mathf.Clamp(target.position.x, -clamp.x, clamp.x),
                Mathf.Clamp(target.position.y + 0.5f, -clamp.y, clamp.y),
                -10
            );
        }
    }

    private void OnPreCull() => GL.Clear(true, true, Color.black);
}
