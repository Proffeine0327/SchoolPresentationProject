using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundFadeType { In, Out }

public class BackgroundSound : MonoBehaviour
{
    public static BackgroundSound Instance { get; private set; }

    [SerializeField] private Sound backgroundSound;
    [SerializeField] private float waitTime;

    private AudioSource currentSound;
    private Coroutine fadeCoroutine;

    private void Start()
    {
        Instance = this;
        this.Invoke(() => currentSound = SoundManager.Instance.PlaySound(backgroundSound, false, true), waitTime);
    }

    private void Update()
    {
        if(currentSound != null)
        {
            currentSound.volume = DataManager.Instance.soundRatio;
        }
    }

    public AudioSource ChangeSound(Sound sound)
    {
        if(currentSound != null) Destroy(currentSound.gameObject);
        currentSound = SoundManager.Instance.PlaySound(sound, false, true);
        return currentSound;
    }

    public void Fade(SoundFadeType type, float fadeTime)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(SoundFade(type, fadeTime));
    }

    private IEnumerator SoundFade(SoundFadeType type, float fadeTime)
    {
        if (currentSound == null) yield break;

        var startVolume = currentSound.volume;
        if (type == SoundFadeType.In)
        {
            if(!currentSound.isPlaying) currentSound.UnPause();
            for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
            {
                currentSound.volume = Mathf.Lerp(startVolume, DataManager.Instance.soundRatio, t / fadeTime);
                yield return new WaitForSecondsRealtime(0);
            }
            currentSound.volume = DataManager.Instance.soundRatio;
        }

        if (type == SoundFadeType.Out)
        {
            for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
            {
                currentSound.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
                yield return new WaitForSecondsRealtime(0);
            }
            currentSound.volume = 0;
            if(currentSound.isPlaying) currentSound.Pause();
        }
    }
}
