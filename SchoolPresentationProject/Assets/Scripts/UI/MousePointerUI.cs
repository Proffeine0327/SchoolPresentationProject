using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MousePointerUI : MonoBehaviour
{
    [SerializeField] private RectTransform cursor;

    void Update()
    {
        Cursor.visible = false;
        cursor.position = Input.mousePosition;
    }
}
