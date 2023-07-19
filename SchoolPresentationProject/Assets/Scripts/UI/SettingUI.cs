using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private RectTransform border;
    [SerializeField] private Button exitButton;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_InputField volumeInput;
    [Header("Vars")]
    [SerializeField] private float hideVPos;

    private bool isPlayingAnimation;
    private bool isDisplayUI;

    public bool IsPlayingAnimation => isPlayingAnimation;
    public bool IsDisplayingUI => isDisplayUI;

    public void DisplayUI(bool active)
    {
        if (isPlayingAnimation) return;
        if (active) bg.gameObject.SetActive(active);

        isDisplayUI = active;

        border.DOAnchorPosY(active ? 0 : hideVPos, 1).SetEase(active ? Ease.OutBack : Ease.InBack).SetUpdate(true);
        bg.DOColor(active ? new Color(0, 0, 0, 0.545f) : Vector4.zero, 1).SetUpdate(true);
        this.InvokeRealTime(() =>
        {
            isPlayingAnimation = false;
            bg.gameObject.SetActive(active);
        }, 1);

        isPlayingAnimation = true;
    }

    private void Awake()
    {
        SingletonManager.RegisterSingleton(this);

        bg.gameObject.SetActive(false);
        exitButton?.onClick.AddListener(() => DisplayUI(false));
        volumeInput.onSubmit.AddListener((value) =>
        {
            if (float.TryParse(value, out var v))
            {
                v = Mathf.Clamp(v, 0, 100);
                v = Mathf.RoundToInt(v);
                DataManager.Instance.soundRatio = v / 100f;
                volumeInput.text = $"{v}%";
                volumeSlider.value = v;
            }
        });
    }

    private void Start()
    {
        volumeSlider.value = DataManager.Instance.soundRatio * 100f;
        volumeInput.text = $"{Mathf.RoundToInt(DataManager.Instance.soundRatio * 100)}%";
    }

    private void Update()
    {
        if (!volumeInput.isFocused)
        {
            DataManager.Instance.soundRatio = volumeSlider.value / 100f;
            volumeInput.text = $"{volumeSlider.value}%";
        }
    }
}
