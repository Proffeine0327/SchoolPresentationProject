using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class AbilitySlotUI : MonoBehaviour
{
    [SerializeField] private Image abilityImage;
    [SerializeField] private TextMeshProUGUI abilityName;
    [SerializeField] private TextMeshProUGUI abilityExplain;

    private bool canClick;
    private RectTransform rectTransform;
    private Image img;
    private UnityEvent action;

    public bool CanClick => canClick;
    public RectTransform RectTransform => rectTransform;
    public UnityEvent Action => action;

    private void Awake()
    {
        rectTransform = (transform as RectTransform);
        img = GetComponent<Image>();
    }

    public void Flicking(float t)
    {
        StartCoroutine(FlickingAnimation(t));
    }

    private IEnumerator FlickingAnimation(float time)
    {
        var waitFrameRealTime = new WaitForSecondsRealtime(0);
        float flickingTime = 0f;
        bool colored = false;

        for(float t = 0; t < time; t += Time.unscaledDeltaTime)
        {
            if (flickingTime <= 0)
            {
                colored = !colored;
                img.color = colored ? Color.white : Color.yellow;
                flickingTime = 0.05f;
            }
            else
            {
                flickingTime -= Time.unscaledDeltaTime;
            }

            yield return waitFrameRealTime;
        }
        img.color = Color.white;
    }

    public void SetSlot(Sprite image, string name, string explain, UnityEvent action, bool canClick = true)
    {
        abilityImage.sprite = image;
        abilityName.text = name;
        abilityExplain.text = explain;
        this.action = action;
        this.canClick = canClick;
    }
}
