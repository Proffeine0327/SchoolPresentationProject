using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerPresets playerPresets;

    private bool isGameStart;
    private bool isChangedSound;
    private float startTime;
    private GameObject player;

    public bool IsGameStart => isGameStart;
    public float GamePlayTime { get { return isGameStart ? Time.time - startTime : 0; } }

    private void Awake()
    {
        player = Instantiate(playerPresets.Presets[DataManager.Instance.playerIndex].Prefeb);
        SingletonManager.RegisterSingleton(this);
    }

    private void Start()
    {
        SingletonManager.GetSingleton<CameraManager>().SetTarget(player.transform);
        SingletonManager.GetSingleton<ScreenChangerUI>().ActiveUI(false);
        StartCoroutine(StartAnimation());
    }

    private void Update()
    {
        if (!isChangedSound && GamePlayTime > 540f)
        {
            isChangedSound = true;
            SingletonManager.GetSingleton<BackgroundSound>().Fade(SoundFadeType.Out, 2);

            this.Invoke(() =>
            {
                var sound = SingletonManager.GetSingleton<BackgroundSound>().ChangeSound(Sound.IngameWarning);
                sound.volume = 0;
                SingletonManager.GetSingleton<BackgroundSound>().Fade(SoundFadeType.In, 2);
            }, 2);
        }
    }

    private IEnumerator StartAnimation()
    {
        SingletonManager.GetSingleton<ScreenChangerUI>().ActiveUI(false);
        yield return new WaitForSeconds(SingletonManager.GetSingleton<ScreenChangerUI>().AnimationTime + 1);
        yield return StartCoroutine(SingletonManager.GetSingleton<Player>().StartAnimation());
        SingletonManager.GetSingleton<EnemySpawnManager>().StartSpawning();

        isGameStart = true;
        startTime = Time.time;
    }
}
