using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilitySelectUI : MonoBehaviour
{
    public static AbilitySelectUI Instance { get; private set; }

    [SerializeField] private GameObject bg;
    [SerializeField] private AbilitySlotUI[] slots;
    [Header("Presets")]
    [SerializeField] private AbilityPreset[] gunUpgradePresets;
    [SerializeField] private AbilityPreset[] abilityPresets;

    private int curIndex = -1;
    private bool isDisplayingUI;

    public bool IsDisplayingUI => isDisplayingUI;

    private void Awake() 
    {
        Instance = this;
    }

    public void DisplayUI()
    {
        Time.timeScale = 0; 
        isDisplayingUI = true;
        bg.SetActive(true);
        
        for(int i = 0; i < slots.Length; i++)
        {
            if(i == 0)
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
