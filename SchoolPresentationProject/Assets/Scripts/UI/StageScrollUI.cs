using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class StageScrollUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Ref")]
    [SerializeField] private RectTransform content;
    [SerializeField] private Scrollbar scrollBar;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private Button btn;
    [Header("Var")]
    [TextArea(1,3)]
    [SerializeField] private string[] stageNames;
    [SerializeField] private float imageHSize;

    private int index;
    private bool isDragging = true;

    public int Index => index;
    public UnityEvent ButtonEvent => btn.onClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        const float delta = 10;

        if (eventData.delta.x is < delta and > -delta)
        {
            index = Mathf.RoundToInt(-content.anchoredPosition.x / imageHSize);
        }
        else
        {
            if (eventData.delta.x < -delta) index++;
            if (eventData.delta.x > delta) index--;
        }
        index = Mathf.Clamp(index, 0, content.childCount - 1);
    }

    private void Update()
    {
        if (!isDragging)
            scrollBar.value = Mathf.Lerp(scrollBar.value, (float)index / (content.childCount - 1), Time.deltaTime * 10f);

        stageText.text = stageNames[index];
    }
}
