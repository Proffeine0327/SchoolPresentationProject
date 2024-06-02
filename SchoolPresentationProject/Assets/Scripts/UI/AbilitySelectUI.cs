using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class AbilitySelectUI : MonoBehaviour
{
    private Player player => SingletonManager.GetSingleton<Player>();
    private SettingUI settingUI => SingletonManager.GetSingleton<SettingUI>();

    [SerializeField] private GameObject bg;
    [SerializeField] private AbilitySlotUI[] slots;
    [Header("Presets")]
    [SerializeField] private GunPreset[] gunUpgradePresets;
    [SerializeField] private AbilityPreset[] abilityPresets;
    [Header("Vars")]
    [SerializeField] private float hideXPos;
    [SerializeField] private float showXPos;

    private int selectedIndex = -1;
    private bool isDisplayingUI;
    private bool isPlayingAcitveAnimation;

    public bool IsDisplayingUI => isDisplayingUI;

    private void Awake()
    {
        SingletonManager.RegisterSingleton(this);
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

        var abilityList = abilityPresets.ToList();
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == 0) //gun upgrade ui
            {
                if (player.CurGunLvl < gunUpgradePresets.Length)
                {
                    var p = gunUpgradePresets[player.CurGunLvl];
                    slots[i].SetSlot(p.Image, $"{p.Name} Lv.{player.CurGunLvl % 5 + 1}", p.Explain, p.GunPrefeb);
                }
                else
                {
                    //gun level is max
                    var p = gunUpgradePresets[^1];
                    slots[i].SetSlot(p.Image, $"{p.Name} Lv.MAX", "", p.GunPrefeb, false);
                }
            }
            else
            {
                //random ability upgrade
                var rand = Random.Range(0, abilityList.Count);
                var abilityLvl = player.AbilityLvls[(int)abilityList[rand].AbilityType];
                var type = abilityList[rand].AbilityType;

                slots[i].SetSlot(
                    abilityList[rand].Image,
                    $"{abilityList[rand].Name} Lv.{abilityLvl + 1}",
                    abilityLvl == 0 ? abilityList[rand].GetExplain : abilityList[rand].LvlUpExplain,
                    type
                );

                abilityList.RemoveAt(rand);
            }
        }
        isPlayingAcitveAnimation = true;
        for (int i = 0; i < slots.Length; i++)
        {
            var slotTransform = slots[i].RectTransform;
            slotTransform.anchoredPosition = new Vector2(hideXPos, slotTransform.anchoredPosition.y);
        }

        //appeare animation
        StartCoroutine(ActiveAnimation());
    }

    private IEnumerator ActiveAnimation()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RectTransform.DOAnchorPosX(showXPos, 0.75f).SetEase(Ease.OutBack).SetUpdate(true);
            yield return new WaitForSecondsRealtime(0.2f);
        }
        isPlayingAcitveAnimation = false;
    }

    private void Update()
    {
        if (bg.activeSelf && !isPlayingAcitveAnimation)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                //if mouse enter
                if (RectTransformUtility.RectangleContainsScreenPoint(slots[i].RectTransform, Input.mousePosition) && selectedIndex == -1 && slots[i].CanClick && !settingUI.IsDisplayingUI)
                {
                    //change scale
                    slots[i].RectTransform.localScale =
                        Vector3.Lerp(slots[i].RectTransform.localScale, new Vector3(1.025f, 1.025f, 1), Time.unscaledDeltaTime * 5f);

                    //player sound
                    if (!slots[i].IsPlayedSound)
                    {
                        SoundManager.Instance.PlaySound(Sound.Clicky);
                        slots[i].IsPlayedSound = true;
                    }

                    //if click
                    if (Input.GetMouseButtonDown(0))
                    {
                        selectedIndex = i;

                        SoundManager.Instance.PlaySound(Sound.Select);
                        slots[selectedIndex].RectTransform.DOScale(new Vector3(1.07f, 1.07f, 1), 0.5f).SetEase(Ease.OutQuad).SetUpdate(true);

                        //if clicked slot is first gun upgrade, else selected type upgrade
                        if (selectedIndex == 0) player.UpgradeGun(slots[selectedIndex].GunPrefeb);
                        else player.UpgradeAbility(slots[selectedIndex].AbilityType);
                        
                        slots[selectedIndex].Flicking(0.7f);
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
                    if (selectedIndex == i) return;

                    slots[i].IsPlayedSound = false;
                    slots[i].RectTransform.localScale =
                        Vector3.Lerp(slots[i].RectTransform.localScale, Vector3.one, Time.unscaledDeltaTime * 5f);
                }
            }
        }
    }
}

[System.Serializable]
public class GunPreset
{
    [SerializeField] private Sprite image;
    [SerializeField] private string name;
    [TextArea(3, 4)]
    [SerializeField] private string explain;
    [SerializeField] private GameObject gunPrefeb;

    public Sprite Image => image;
    public string Name => name;
    public string Explain => explain;
    public GameObject GunPrefeb => gunPrefeb;
}

[System.Serializable]
public class AbilityPreset
{
    [SerializeField] private Sprite image;
    [SerializeField] private string name;
    [TextArea(1, 5)]
    [SerializeField] private string getExplain;
    [TextArea(1, 5)]
    [SerializeField] private string lvlUpExplain;
    [SerializeField] private AbilityType abilityType;

    public Sprite Image => image;
    public string Name => name;
    public string GetExplain => getExplain;
    public string LvlUpExplain => lvlUpExplain;
    public AbilityType AbilityType => abilityType;
}
