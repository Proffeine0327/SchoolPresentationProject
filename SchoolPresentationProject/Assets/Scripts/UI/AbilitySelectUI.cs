using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class AbilitySelectUI : MonoBehaviour
{
    public static AbilitySelectUI Instance { get; private set; }

    [SerializeField] private GameObject bg;
    [SerializeField] private AbilitySlotUI[] slots;
    [Header("Presets")]
    [SerializeField] private AbilityPreset[] gunUpgradePresets;
    [SerializeField] private AbilityPreset[] abilityPresets;

    private int selectedIndex = -1;
    private bool isDisplayingUI;
    private bool isPlayingAcitveAnimation;

    public bool IsDisplayingUI => isDisplayingUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        bg.SetActive(false);
    }

    public void DisplayUI()
    {
        Time.timeScale = 0;
        isDisplayingUI = true;
        bg.SetActive(true);

        for (int i = 0; i < slots.Length; i++)
        {
            if (i == 0)
            {
                var rand = Random.Range(0, gunUpgradePresets.Length);
                var p = gunUpgradePresets[rand];
                slots[i].SetSlot(p.Image, p.Name, p.Explain, rand);
            }
            else
            {
                var rand = Random.Range(0, abilityPresets.Length);
                var p = abilityPresets[rand];
                slots[i].SetSlot(p.Image, p.Name, p.Explain, rand);
            }
        }
        isPlayingAcitveAnimation = true;
        for(int i = 0; i < slots.Length; i++)
        {
            var slotTransform = slots[i].RectTransform;
            slotTransform.anchoredPosition = new Vector2(-2000, slotTransform.anchoredPosition.y);
        }
        StartCoroutine(ActiveAnimation());
    }

    private IEnumerator ActiveAnimation()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].RectTransform.DOAnchorPosX(172, 0.75f).SetEase(Ease.OutBack).SetUpdate(true);
            yield return new WaitForSecondsRealtime(0.2f);
        }
        isPlayingAcitveAnimation = false;
    }

    private void Update()
    {
        if (bg.activeSelf && !isPlayingAcitveAnimation)
        {
            for(int i = 0; i < slots.Length; i++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(slots[i].RectTransform, Input.mousePosition) && selectedIndex == -1)
                {
                    slots[i].RectTransform.localScale =
                        Vector3.Lerp(slots[i].RectTransform.localScale, new Vector3(1.025f, 1.025f, 1), Time.unscaledDeltaTime * 5f);

                    if(Input.GetMouseButtonDown(0))
                    {
                        selectedIndex = i;

                        slots[i].RectTransform.DOScale(new Vector3(1.07f, 1.07f, 1), 0.5f).SetEase(Ease.OutQuad).SetUpdate(true);
                        slots[i].Flicking(0.7f);
                        this.InvokeRealTime(() =>
                        {
                            slots[selectedIndex].RectTransform.localScale = Vector3.one;
                            selectedIndex = -1;
                            bg.SetActive(false);
                            isDisplayingUI = false;
                            Time.timeScale = 1;
                        }, 1f);
                    }
                }
                else
                {
                    if(selectedIndex == i) continue;

                    slots[i].RectTransform.localScale =
                        Vector3.Lerp(slots[i].RectTransform.localScale, Vector3.one, Time.unscaledDeltaTime * 5f);
                }
            }
        }
    }
}

[System.Serializable]
public class AbilityPreset
{
    [SerializeField] private Sprite image;
    [SerializeField] private string name;
    [TextArea(3, 4)]
    [SerializeField] private string explain;
    [SerializeField] private UnityEvent action;

    public Sprite Image => image;
    public string Name => name;
    public string Explain => explain;
    public UnityEvent Action => action;
}
