using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        Instance = this;
    }

    private void Start()
    {
        CameraManager.Instance.SetTarget(player.transform);
        ScreenChangerUI.Instance.ActiveUI(false);
        StartCoroutine(StartAnimation());
    }

    private void Update()
    {
        if (!isChangedSound && GamePlayTime > 540f)
        {
            isChangedSound = true;
            BackgroundSound.Instance.Fade(SoundFadeType.Out, 2);

            this.Invoke(() =>
            {
                var sound = BackgroundSound.Instance.ChangeSound(Sound.IngameWarning);
                sound.Pause();
                BackgroundSound.Instance.Fade(SoundFadeType.In, 2);
            }, 2);
        }
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
