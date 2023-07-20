using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditUI : MonoBehaviour
{
    [SerializeField] private Button exitBtn;

    void Start()
    {
        SingletonManager.GetSingleton<ScreenChangerUI>().ActiveUI(false);

        exitBtn?.onClick.AddListener(() =>
        {
            SingletonManager.GetSingleton<ScreenChangerUI>().ActiveUI(true);
            SoundManager.Instance.PlaySound(Sound.OpeningCan);
            SingletonManager.GetSingleton<BackgroundSound>().Fade(SoundFadeType.Out, 2);
            this.Invoke(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Title"), SingletonManager.GetSingleton<ScreenChangerUI>().AnimationTime + 1);
        });
    }
}
