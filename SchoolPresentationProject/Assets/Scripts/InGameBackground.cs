using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameBackground : MonoBehaviour
{
    [SerializeField] private Sprite[] bgs;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = bgs[DataManager.Instance.stage];
    }
}
