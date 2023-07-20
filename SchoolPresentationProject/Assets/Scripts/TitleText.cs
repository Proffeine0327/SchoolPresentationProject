using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleText : MonoBehaviour
{
    [SerializeField] private float amplitudeMove;
    [SerializeField] private float amplitudeRotate;
    private float startY;
    private float startRot;

    private void Start()
    {
        startY = transform.localPosition.y;
        startRot = transform.eulerAngles.z;
    }

    private void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, startY + (Mathf.Sin(Time.time) * amplitudeMove), 0);
        transform.localRotation = Quaternion.Euler(0, 0, startRot + (Mathf.Cos(Time.time) * amplitudeRotate));
    }
}
