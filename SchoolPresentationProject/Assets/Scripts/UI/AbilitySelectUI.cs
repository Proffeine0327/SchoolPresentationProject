using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySelectUI : MonoBehaviour
{
    public static AbilitySelectUI Instance { get; private set; }

    [SerializeField] private GameObject bg;
    [SerializeField] private AbilitySelectUI[] slots;
    
    private int curIndex;
}
