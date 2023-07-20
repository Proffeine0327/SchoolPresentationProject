using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageCharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private RectTransform border;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject stageGroup;
    [SerializeField] private StageScrollUI stageScrollUI;
    [SerializeField] private GameObject characterGroup;
    [SerializeField] private CharacterScrollUI characterScrollUI;
    [Header("Vars")]
    [SerializeField] private float hideVPos;

    private bool isPlayingAnimation;

    public void DisplayUI(bool active)
    {
        if (isPlayingAnimation) return;

        if (active) bg.gameObject.SetActive(active);

        if (active)
        {
            stageGroup.SetActive(true);
            characterGroup.SetActive(false);
        }

        border.DOAnchorPosY(active ? 0 : hideVPos, 1).SetEase(active ? Ease.OutBack : Ease.InBack);
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
        SingletonManager.RegisterSingleton(this);
    }

    private void Start()
    {
        bg.gameObject.SetActive(false);
        exitButton.onClick.AddListener(() => DisplayUI(false));

        stageScrollUI.ButtonEvent.AddListener(() =>
        {
            stageGroup.SetActive(false);
            characterGroup.SetActive(true);
        });

        characterScrollUI.ButtonEvent.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(Sound.OpeningCan);
            DataManager.Instance.stageIndex = stageScrollUI.Index;
            DataManager.Instance.playerIndex = characterScrollUI.Index;
            SingletonManager.GetSingleton<BackgroundSound>().Fade(SoundFadeType.Out, SingletonManager.GetSingleton<ScreenChangerUI>().AnimationTime);

            SingletonManager.GetSingleton<ScreenChangerUI>().ActiveUI(true);
            this.Invoke(() => UnityEngine.SceneManagement.SceneManager.LoadScene("InGame"), SingletonManager.GetSingleton<ScreenChangerUI>().AnimationTime + 1);
        });

        stageGroup.SetActive(false);
        characterGroup.SetActive(false);
    }
}
