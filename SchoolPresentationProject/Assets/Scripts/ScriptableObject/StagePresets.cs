using UnityEngine;

[CreateAssetMenu(fileName = "StagePreset", menuName = "SchoolPresentationProject/StagePreset", order = 0)]
public class StagePresets : ScriptableObject
{
    [SerializeField] private StagePreset[] presets;
    public StagePreset[] Presets => presets;
}

[System.Serializable]
public class StagePreset
{
    [SerializeField] private Sprite background;
    [SerializeField] private GameObject[] enemies;

    public Sprite Background => background;
    public GameObject[] Enemies => enemies;
}