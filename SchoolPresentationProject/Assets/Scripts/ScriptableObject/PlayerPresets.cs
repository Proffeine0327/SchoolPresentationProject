using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPresets", menuName = "SchoolPresentationProject/PlayerPresets", order = 0)]
public class PlayerPresets : ScriptableObject
{
    [SerializeField] private PlayerPreset[] presets;
    public PlayerPreset[] Presets => presets;
}

[System.Serializable]
public class PlayerPreset
{
    [SerializeField] private string name;
    [TextArea(1, 3)]
    [SerializeField] private string explain;
    [SerializeField] private Sprite selectUISprite;
    [SerializeField] private GameObject prefeb;

    public string Name => name;
    public string Explain => explain;
    public Sprite SelectUISprite => selectUISprite;
    public GameObject Prefeb => prefeb;
}