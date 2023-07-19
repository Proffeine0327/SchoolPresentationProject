using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleButtonUI : MonoBehaviour
{
    [SerializeField] private Button start;
    [SerializeField] private Button howto;
    [SerializeField] private Button exit;
    [SerializeField] private Button setting;

    private void Start()
    {
        Time.timeScale = 1;

        start.onClick.AddListener(() => SingletonManager.GetSingleton<StageCharacterSelectUI>().DisplayUI(true));
        howto.onClick.AddListener(() => SingletonManager.GetSingleton<HowToUI>().DisplayUI(true));
        exit.onClick.AddListener(() => SingletonManager.GetSingleton<ExitUI>().DisplayUI(true));
        setting.onClick.AddListener(() => SingletonManager.GetSingleton<SettingUI>().DisplayUI(true));

        SingletonManager.GetSingleton<ScreenChangerUI>().ActiveUI(false);
    }
}
