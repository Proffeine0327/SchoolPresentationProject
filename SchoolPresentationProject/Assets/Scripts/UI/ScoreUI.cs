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
    private ScreenChangerUI screenChangerUI => SingletonManager.GetSingleton<ScreenChangerUI>();
    private GameManager gameManager => SingletonManager.GetSingleton<GameManager>();
    private Player player => SingletonManager.GetSingleton<Player>();
    
    [SerializeField] private Image bg;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI detail;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        SingletonManager.RegisterSingleton(this);

        exitButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(Sound.OpeningCan);
            screenChangerUI.ActiveUI(true);
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

        //mission complete/failed
        title.text =
            gameManager.GamePlayTime / 60 > 10
                ? "mission <color=yellow>complete</color>"
                : "mission <color=red>failed</color>";
        title.rectTransform.localScale = new Vector3(2, 2, 1);
        title.rectTransform.DOScale(Vector3.one, 2f).SetEase(Ease.InQuart).SetUpdate(true);
        yield return new WaitForSecondsRealtime(4);

        var sb = new StringBuilder();
        //time
        sb.Append("Time : ");
        detail.text = sb.ToString();
        SoundManager.Instance.PlaySound(Sound.Text);
        yield return new WaitForSecondsRealtime(1f);

        var timetext = $"{Mathf.FloorToInt(gameManager.GamePlayTime / 60)}:{string.Format("{0:0,0}", Mathf.FloorToInt(gameManager.GamePlayTime % 60))}";
        for (int i = 0; i < timetext.Length; i++)
        {
            sb.Append(timetext[i]);
            detail.text = sb.ToString();
            yield return new WaitForSecondsRealtime(0.1f);
            SoundManager.Instance.PlaySound(Sound.Text);
        }
        yield return new WaitForSecondsRealtime(1f);

        //kill
        sb.Append("\nKill : ");
        detail.text = sb.ToString();
        SoundManager.Instance.PlaySound(Sound.Text);

        yield return new WaitForSecondsRealtime(1f);
        var killtext = string.Format("{0:#,0}", player.KillAmount);
        for (int i = 0; i < killtext.Length; i++)
        {
            sb.Append(killtext[i]);
            detail.text = sb.ToString();
            SoundManager.Instance.PlaySound(Sound.Text);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        yield return new WaitForSecondsRealtime(1f);

        //level
        sb.Append("\nLevel : ");
        SoundManager.Instance.PlaySound(Sound.Text);
        detail.text = sb.ToString();

        yield return new WaitForSecondsRealtime(1f);
        var lvltext = player.CurLvl.ToString();
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
