using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class CharacterScrollUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Ref")]
    [SerializeField] private RectTransform content;
    [SerializeField] private Scrollbar scrollBar;
    [SerializeField] private TextMeshProUGUI characterText;
    [SerializeField] private TextMeshProUGUI characterExplain;
    [SerializeField] private Button btn;
    [SerializeField] private PlayerPresets playerPresets;
    [Header("Var")]
    [SerializeField] private float imageHSize;

    private int index;
    private int contentCount;
    private bool isDragging = true;

    public int Index => index;
    public UnityEvent ButtonEvent => btn.onClick;

    private void Start()
    {
        for(int i = 0; i < playerPresets.Presets.Length; i++)
        {
            var image = new GameObject("Image").AddComponent<Image>();
            image.sprite = playerPresets.Presets[i].SelectUISprite;
            image.SetNativeSize();
            image.rectTransform.sizeDelta *= imageHSize / image.rectTransform.sizeDelta.x;

            image.transform.SetParent(content);
        }

        contentCount = content.childCount;
    }

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
        index = Mathf.Clamp(index, 0, contentCount - 1);
    }

    private void Update()
    {
        if (!isDragging)
            scrollBar.value = Mathf.Lerp(scrollBar.value, (float)index / (contentCount - 1), Time.deltaTime * 10f);

        characterText.text = playerPresets.Presets[index].Name;
        characterExplain.text = playerPresets.Presets[index].Explain;
    }
}
