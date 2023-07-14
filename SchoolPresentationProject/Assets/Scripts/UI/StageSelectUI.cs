using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StageSelectUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform content;
    [SerializeField] private Scrollbar scrollBar;
    [SerializeField] private Button enterbtn;
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private string[] stageNames;

    private int index;
    private int contentCount;
    private bool isDragging = true;
    private bool isStageSelected = false;

    private void Start()
    {
        ScreenChangerUI.Instance.ActiveUI(false);
        contentCount = content.childCount;
        enterbtn.onClick.AddListener(() =>
        {
            if(isStageSelected) return;
            isStageSelected = true;
            StartCoroutine(EnterAnimation());
        });
    }

    private IEnumerator EnterAnimation()
    {
        ScreenChangerUI.Instance.ActiveUI(true);
        yield return new WaitForSeconds(ScreenChangerUI.Instance.AnimationTime + 1);

        DataManager.Instance.stage = index;
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGame");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        if (eventData.delta.x is < 20 and > -20)
        {
            index = Mathf.RoundToInt(-content.anchoredPosition.x / 3840);
        }
        else
        {
            if (eventData.delta.x < -20) index++;
            if (eventData.delta.x > 20) index--;
        }
        index = Mathf.Clamp(index, 0, contentCount - 1);
    }

    private void Update()
    {
        if (!isDragging)
            scrollBar.value = Mathf.Lerp(scrollBar.value, (float)index / (contentCount - 1), Time.deltaTime * 10f);

        foreach(var text in texts)
            text.text = stageNames[index];
    }
}
