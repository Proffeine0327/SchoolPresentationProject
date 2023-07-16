using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExitUI : MonoBehaviour
{
    public static ExitUI Instance { get; private set; }

    [SerializeField] private Image bg;
    [SerializeField] private RectTransform border;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button gameExitButton;

    private bool isPlayingAnimation;

    public void DisplayUI(bool active)
    {
        if (isPlayingAnimation) return;

        if (active) bg.gameObject.SetActive(active);

        border.DOAnchorPosY(active ? 0 : -1757, 1).SetEase(active ? Ease.OutBack : Ease.InBack);
        bg.DOColor(active ? new Color(0, 0, 0, 0.545f) : Vector4.zero, 1);
        this.Invoke(() =>
        {
            isPlayingAnimation = false;
            bg.gameObject.SetActive(active);
        }, 1);

        isPlayingAnimation = true;
    }

    private void Awake()
    {
        Instance = this;
        bg.gameObject.SetActive(false);

        exitButton.onClick.AddListener(() => DisplayUI(false));
        gameExitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        });
    }
}
