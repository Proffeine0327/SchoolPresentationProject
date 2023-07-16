using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [HideInInspector] public int stageIndex;
    [HideInInspector] public int playerIndex;
    [HideInInspector] public float soundRatio = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        soundRatio = 0.5f;
    }
}
