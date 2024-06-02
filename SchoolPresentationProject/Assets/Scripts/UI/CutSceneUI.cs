using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneUI : MonoBehaviour
{
    private ScreenChangerUI screenChangerUI => SingletonManager.GetSingleton<ScreenChangerUI>();

    [SerializeField] private Image[] blinders;

    private void Start()
    {
        screenChangerUI.ActiveUI(false);
        StartCoroutine(CutSceneDisplay());
    }

    private IEnumerator CutSceneDisplay()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < blinders.Length; i++)
        {
            for (float t = 0; t < 2f; t += Time.deltaTime)
            {
                blinders[i].color = new Color(0, 0, 0, 1 - (t / 2f));

                if(Input.GetMouseButtonDown(0)) break;
                yield return null;
            }
            blinders[i].color = new Color(0, 0, 0, 0);
            yield return null;

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            yield return null;
        }
        screenChangerUI.ActiveUI(true);
        yield return new WaitForSeconds(screenChangerUI.AnimationTime + 1);
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGame");
    }
}