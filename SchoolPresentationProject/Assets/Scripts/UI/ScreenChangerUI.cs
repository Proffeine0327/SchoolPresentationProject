using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenChangerUI : MonoBehaviour
{
    public static ScreenChangerUI Instance { get; private set; }

    [SerializeField] private GameObject screen;
    [SerializeField] private Image left;
    [SerializeField] private Image right;
    [SerializeField] private float animationTime;

    public float AnimationTime => animationTime;

    private void Awake()
    {
        Instance = this;
    }

    public void ActiveUI(bool active)
    {
        screen.SetActive(active);
        left.gameObject.SetActive(true);
        right.gameObject.SetActive(true);
        if (active)
        {
            left.rectTransform.anchoredPosition = new Vector2(-3500, 0);
            right.rectTransform.anchoredPosition = new Vector2(3500, 0);

            left.rectTransform.DOAnchorPosX(0, animationTime).SetEase(Ease.InOutQuad).SetUpdate(true);
            right.rectTransform.DOAnchorPosX(0, animationTime).SetEase(Ease.InOutQuad).SetUpdate(true);
        }
        else
        {
            left.rectTransform.anchoredPosition = new Vector2(0, 0);
            right.rectTransform.anchoredPosition = new Vector2(0, 0);

            left.rectTransform.DOAnchorPosX(-3500, animationTime).SetEase(Ease.InOutQuad).SetUpdate(true);
            right.rectTransform.DOAnchorPosX(3500, animationTime).SetEase(Ease.InOutQuad).SetUpdate(true);
        }
        this.Invoke(() =>
        {
            if(!active)
            {
                left.gameObject.SetActive(false);
                right.gameObject.SetActive(false);
            }
        }, animationTime);
    }
}
