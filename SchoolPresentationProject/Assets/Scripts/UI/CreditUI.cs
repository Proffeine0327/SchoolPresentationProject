using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditUI : MonoBehaviour
{
    private ScreenChangerUI screenChangerUI => SingletonManager.GetSingleton<ScreenChangerUI>();
    private BackgroundSound backgroundSound => SingletonManager.GetSingleton<BackgroundSound>();

    [SerializeField] private Button exitBtn;

    void Start()
    {
        screenChangerUI.ActiveUI(false);

        exitBtn?.onClick.AddListener(() =>
        {
            screenChangerUI.ActiveUI(true);
            SoundManager.Instance.PlaySound(Sound.OpeningCan);
            backgroundSound.Fade(SoundFadeType.Out, 2);
            this.Invoke(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Title"), screenChangerUI.AnimationTime + 1);
        });
    }
}
