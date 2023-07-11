using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitySlotUI : MonoBehaviour
{
    [SerializeField] private Image abilityImage;
    [SerializeField] private TextMeshProUGUI abilityName;
    [SerializeField] private TextMeshProUGUI abilityExplain;

    public void SetSlot(Sprite image, string name, string explain)
    {
        abilityImage.sprite = image;
        abilityName.text = name;
        abilityExplain.text = explain;
    }
}
