using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound { }

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClip[] sounds;

    public void PlaySound(Sound sound)
    {
        var clip = sounds[(int)sound];
        var comp = new GameObject(clip.name).AddComponent<AudioSource>();
        comp.clip = clip;
        comp.Play();
        Destroy(comp, clip.length);
    }

    private void Awake()
    {
        Instance = this;
    }
}
