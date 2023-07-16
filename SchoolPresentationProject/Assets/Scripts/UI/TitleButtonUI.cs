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

        start.onClick.AddListener(() => StageCharacterSelectUI.Instance.DisplayUI(true));
        howto.onClick.AddListener(() => HowToUI.Instance.DisplayUI(true));
        exit.onClick.AddListener(() => ExitUI.Instance.DisplayUI(true));

        ScreenChangerUI.Instance.ActiveUI(false);
    }
}
