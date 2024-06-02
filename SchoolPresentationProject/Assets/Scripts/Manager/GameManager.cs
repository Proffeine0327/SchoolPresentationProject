using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CameraManager cameraManager => SingletonManager.GetSingleton<CameraManager>();
    private BackgroundSound backgroundSound => SingletonManager.GetSingleton<BackgroundSound>();
    private ScreenChangerUI screenChangerUI => SingletonManager.GetSingleton<ScreenChangerUI>();
    private Player player => SingletonManager.GetSingleton<Player>();
    private EnemySpawnManager enemySpawnManager => SingletonManager.GetSingleton<EnemySpawnManager>();

    [SerializeField] private PlayerPresets playerPresets;

    private bool isGameStart;
    private bool isChangedSound;
    private float startTime;

    public bool IsGameStart => isGameStart;
    public float GamePlayTime { get { return isGameStart ? Time.time - startTime : 0; } }

    private void Awake()
    {
        Instantiate(playerPresets.Presets[DataManager.Instance.playerIndex].Prefeb);
        SingletonManager.RegisterSingleton(this);
    }

    private void Start()
    {
        cameraManager.SetTarget(player.transform);
        SingletonManager.GetSingleton<ScreenChangerUI>().ActiveUI(false);
        StartCoroutine(StartAnimation());
    }

    private void Update()
    {
        if (!isChangedSound && GamePlayTime > 540f)
        {
            isChangedSound = true;
            backgroundSound.Fade(SoundFadeType.Out, 2);

            this.Invoke(() =>
            {
                var sound = backgroundSound.ChangeSound(Sound.IngameWarning);
                sound.volume = 0;
                backgroundSound.Fade(SoundFadeType.In, 2);
            }, 2);
        }
    }

    private IEnumerator StartAnimation()
    {
        screenChangerUI.ActiveUI(false);
        yield return new WaitForSeconds(screenChangerUI.AnimationTime + 1);
        yield return StartCoroutine(player.StartAnimation());
        enemySpawnManager.StartSpawning();

        isGameStart = true;
        startTime = Time.time;
    }
}
