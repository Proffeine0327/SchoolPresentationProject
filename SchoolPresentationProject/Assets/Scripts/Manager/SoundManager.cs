using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound { Main, Ingame, IngameWarning, opening_can,  }

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClip[] sounds;

    public AudioSource PlaySound(Sound sound, bool isDestory = true, bool isLoop = false)
    {
        var clip = sounds[(int)sound];
        var comp = new GameObject(clip.name).AddComponent<AudioSource>();
        comp.clip = clip;
        comp.Play();
        if(isDestory) Destroy(comp, clip.length);
        if(isLoop) comp.loop = true;
        return comp;
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
