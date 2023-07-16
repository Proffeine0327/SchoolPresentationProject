using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance { get; private set; }

    [SerializeField] private GameObject xmark;
    [SerializeField] private StagePresets stagePresets;

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
    }

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            var spawnpos = new Vector2(Random.Range(-14f, 14f), Random.Range(-9f, 9f));
            StartCoroutine(SpawnEnemy(spawnpos));

            var randomwait = Random.Range(0.2f, 3f) * Mathf.Lerp(1, 0.05f, Mathf.FloorToInt(GameManager.Instance.GamePlayTime / 60) / 9f);
            yield return new WaitForSeconds(randomwait);
        }
    }

    private IEnumerator SpawnEnemy(Vector2 pos)
    {
        var x = Instantiate(xmark, pos, Quaternion.identity);
        x.transform.localScale = Vector3.zero;
        x.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack);
        Destroy(x, 2);

        yield return new WaitForSeconds(2);
        var rand = Random.Range(0, stagePresets.Presets[DataManager.Instance.stageIndex].Enemies.Length);
        Instantiate(stagePresets.Presets[DataManager.Instance.stageIndex].Enemies[rand], pos, Quaternion.identity);
    }
}
