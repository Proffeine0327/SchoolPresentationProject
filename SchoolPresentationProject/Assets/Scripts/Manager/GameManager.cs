using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isGameStart;
    private float startTime;

    public bool IsGameStart => isGameStart;
    public float GamePlayTime { get {  return isGameStart ? Time.time - startTime : 0; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ScreenChangerUI.Instance.ActiveUI(false);
        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        ScreenChangerUI.Instance.ActiveUI(false);
        yield return new WaitForSeconds(ScreenChangerUI.Instance.AnimationTime + 1);
        yield return StartCoroutine(Player.Instance.StartAnimation());
        EnemySpawnManager.Instance.StartSpawning();

        isGameStart = true;
        startTime = Time.time;
    }
}
