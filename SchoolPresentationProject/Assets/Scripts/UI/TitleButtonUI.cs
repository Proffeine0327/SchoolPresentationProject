using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleButtonUI : MonoBehaviour
{
    private StageCharacterSelectUI stageCharacterSelectUI => SingletonManager.GetSingleton<StageCharacterSelectUI>();
    private HowToUI howToUI => SingletonManager.GetSingleton<HowToUI>();
    private ExitUI exitUI => SingletonManager.GetSingleton<ExitUI>();
    private SettingUI settingUI => SingletonManager.GetSingleton<SettingUI>();
    private ScreenChangerUI screenChangerUI => SingletonManager.GetSingleton<ScreenChangerUI>();

    [SerializeField] private Button start;
    [SerializeField] private Button howto;
    [SerializeField] private Button exit;
    [SerializeField] private Button setting;

    private void Start()
    {
        Time.timeScale = 1;

        start.onClick.AddListener(() => stageCharacterSelectUI.DisplayUI(true));
        howto.onClick.AddListener(() => howToUI.DisplayUI(true));
        exit.onClick.AddListener(() => exitUI.DisplayUI(true));
        setting.onClick.AddListener(() => settingUI.DisplayUI(true));

        screenChangerUI.ActiveUI(false);
    }
}
