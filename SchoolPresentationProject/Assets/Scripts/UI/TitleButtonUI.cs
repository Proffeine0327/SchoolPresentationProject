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

    private bool isStartBtnPressed;

    private void Start()
    {
        start.onClick.AddListener(() =>
        {
            if(isStartBtnPressed) return;
            isStartBtnPressed = true;
            StartCoroutine(StartAnimation());
        });
        
        ScreenChangerUI.Instance.ActiveUI(false);
    }

    private IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        ScreenChangerUI.Instance.ActiveUI(true);

        yield return new WaitForSeconds(ScreenChangerUI.Instance.AnimationTime + 1);

        SceneManager.LoadScene("Stage");
    }
}
