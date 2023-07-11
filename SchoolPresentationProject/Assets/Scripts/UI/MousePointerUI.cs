using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MousePointerUI : MonoBehaviour
{
    [SerializeField] private RectTransform cursor;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        cursor.position = Input.mousePosition;
    }
}
