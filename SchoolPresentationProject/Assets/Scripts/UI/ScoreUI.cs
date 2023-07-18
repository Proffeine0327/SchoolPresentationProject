using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI Instance { get; private set; }

    [SerializeField] private Image bg;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI detail;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        Instance = this;

        exitButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(Sound.OpeningCan);
            ScreenChangerUI.Instance.ActiveUI(true);
            this.InvokeRealTime(() => SceneManager.LoadScene("Title"), 3);
        });
    }

    public void DisplayUI()
    {
        StartCoroutine(UIAnimation());
    }

    public IEnumerator UIAnimation()
    {
        bg.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(false);

        //mission <color=yellow>complete</color>
        //mission <color=red>failed</color>

        title.text =
            GameManager.Instance.GamePlayTime / 60 > 10
                ? "mission <color=yellow>complete</color>"
                : "mission <color=red>failed</color>";
        title.rectTransform.localScale = new Vector3(2, 2, 1);
        title.rectTransform.DOScale(Vector3.one, 2f).SetEase(Ease.InQuart).SetUpdate(true);
        yield return new WaitForSecondsRealtime(4);

        //time, kill, lv
        var sb = new StringBuilder();
        sb.Append("Time : ");
        detail.text = sb.ToString();
        SoundManager.Instance.PlaySound(Sound.Text);
        yield return new WaitForSecondsRealtime(1f);

        var timetext = $"{Mathf.FloorToInt(GameManager.Instance.GamePlayTime / 60)}:{string.Format("{0:0,0}", Mathf.FloorToInt(GameManager.Instance.GamePlayTime % 60))}";
        for (int i = 0; i < timetext.Length; i++)
        {
            sb.Append(timetext[i]);
            detail.text = sb.ToString();
            yield return new WaitForSecondsRealtime(0.1f);
            SoundManager.Instance.PlaySound(Sound.Text);
        }
        yield return new WaitForSecondsRealtime(1f);

        sb.Append("\nKill : ");
        detail.text = sb.ToString();
        SoundManager.Instance.PlaySound(Sound.Text);

        yield return new WaitForSecondsRealtime(1f);
        var killtext = string.Format("{0:#,0}", Player.Instance.KillAmount);
        for (int i = 0; i < killtext.Length; i++)
        {
            sb.Append(killtext[i]);
            detail.text = sb.ToString();
            SoundManager.Instance.PlaySound(Sound.Text);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        yield return new WaitForSecondsRealtime(1f);

        sb.Append("\nLevel : ");
        SoundManager.Instance.PlaySound(Sound.Text);
        detail.text = sb.ToString();

        yield return new WaitForSecondsRealtime(1f);
        var lvltext = Player.Instance.CurLvl.ToString();
        for (int i = 0; i < lvltext.Length; i++)
        {
            sb.Append(lvltext[i]);
            detail.text = sb.ToString();
            yield return new WaitForSecondsRealtime(0.1f);
            SoundManager.Instance.PlaySound(Sound.Text);
        }

        yield return new WaitForSecondsRealtime(1f);
        exitButton.gameObject.SetActive(true);
    }
}
