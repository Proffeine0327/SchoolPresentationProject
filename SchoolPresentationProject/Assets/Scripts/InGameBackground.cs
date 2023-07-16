using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameBackground : MonoBehaviour
{
    [SerializeField] private StagePresets stagePresets;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = stagePresets.Presets[DataManager.Instance.stageIndex].Background;
    }
}
