using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInformationUI : MonoBehaviour
{
    [Header("Hp")]
    [SerializeField] private RectTransform hpBarBg;
    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI hpText;
    [Header("Gun")]
    [SerializeField] private Image gunImage;
    [SerializeField] private TextMeshProUGUI gunAmmo;
    [SerializeField] private TextMeshProUGUI gunName;
    [Header("Exp")]
    [SerializeField] private Image expBar;
    [Header("Time")]
    [SerializeField] private TextMeshProUGUI timeBgText;
    [SerializeField] private TextMeshProUGUI timeText;

    private void LateUpdate() 
    {
        var player = Player.Instance;

        //hp
        hpBarBg.position = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(0, 1.2f, 0));
        hpBar.fillAmount = player.Curhp / player.MaxHp;
        hpText.text = $"{Mathf.RoundToInt(player.Curhp)}/{Mathf.RoundToInt(player.MaxHp)}";

        //gun
        var gun = player.CurGun;
        gunImage.sprite = gun.GunUIImage;
        gunName.text = gun.GunName;
        gunAmmo.text = $"{gun.CurAmmo}/{gun.MaxAmmo}";

        //exp
        expBar.fillAmount = player.CurExp / player.MaxExp;

        //time
        timeText.text = $"{Mathf.FloorToInt(Time.time / 60)}:{string.Format("{0:0,0}", Mathf.FloorToInt(Time.time % 60))}";
        timeBgText.text = $"{Mathf.FloorToInt(Time.time / 60)}:{string.Format("{0:0,0}", Mathf.FloorToInt(Time.time % 60))}";
    }
}
