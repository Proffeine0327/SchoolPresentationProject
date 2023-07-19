using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum AbilityType { bomb, sickel, armor_piercing }

public class Player : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed;
    [Header("Hp")]
    [SerializeField] private float maxHp;
    [SerializeField] private GameObject hitParticlePrefeb;
    [Header("Gun")]
    [SerializeField] private Transform gunPos;
    [SerializeField] private GameObject startRifle;
    [Header("Ability")]
    [SerializeField] private GameObject bombPrefeb;
    [SerializeField] private float bombWaitTime;
    [SerializeField] private GameObject sickelPrefeb;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Gun curGun;
    private Sickel sickelWeapon = null;
    private int curLvl;
    private int curGunLvl;
    private int killAmount;
    private int[] abilityLvls;
    private float maxExp;
    private float curExp;
    private float curHp;
    private float preTimeScale;
    private float curBombWaitTime;
    private bool isEnd;

    public Gun CurGun => curGun;
    public int CurLvl => curLvl;
    public int CurGunLvl => curGunLvl;
    public int KillAmount => killAmount;
    public int[] AbilityLvls => abilityLvls;
    public float CurHp => curHp;
    public float MaxHp => maxHp;
    public float MaxExp => maxExp;
    public float CurExp => curExp;
    public bool IsEnd => isEnd;

    public void KilledEnemy() => killAmount++;

    public IEnumerator StartAnimation()
    {
        curGun.gameObject.SetActive(true);
        gunPos.localPosition = new Vector3(0, 0.2f, 0);
        gunPos.DOLocalMoveY(0.35f, 1).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(2f);
    }

    public void UpgradeAbility(AbilityType type)
    {
        abilityLvls[(int)type]++;
    }

    public void UpgradeGun(GameObject prefeb)
    {
        if (curGun != null) Destroy(curGun.gameObject);
        curGun = Instantiate(prefeb, gunPos).GetComponent<Gun>();
        curGun.transform.localPosition = Vector3.zero;
        curGunLvl++;
    }

    public void GetExp(float amount)
    {
        curExp += amount;
    }

    public void Damage(float amount)
    {
        curHp -= amount;
        SoundManager.Instance.PlaySound(Sound.Hurt);
        Instantiate(hitParticlePrefeb, transform.position, Quaternion.identity);
    }

    private void Awake()
    {
        SingletonManager.RegisterSingleton(this);
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        curHp = maxHp;
        maxExp = 100;
        curLvl = 1;
        curBombWaitTime = bombWaitTime;
        UpgradeGun(startRifle);
        curGun.SetRotation(20);
        curGun.gameObject.SetActive(false);
        abilityLvls = new int[System.Enum.GetValues(typeof(AbilityType)).Length];
    }

    private void Update()
    {
        if (SingletonManager.GetSingleton<AbilitySelectUI>().IsDisplayingUI) return;
        if (!SingletonManager.GetSingleton<GameManager>().IsGameStart) return;

        if (CurHp <= 0)
        {
            curHp = 0;
            if (!isEnd)
            {
                isEnd = true;
                Time.timeScale = 0;
                BackgroundSound.Instance.Fade(SoundFadeType.Out, 2.5f);
                this.InvokeRealTime(() => SingletonManager.GetSingleton<ScoreUI>().DisplayUI(), 3);
            }
            return;
        }

        //move
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var isMove = h != 0 || v != 0;

        rb.velocity = new Vector2(h, v).normalized * moveSpeed;

        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, -14.45f, 14.45f),
            Mathf.Clamp(transform.position.y, -9.673f, 8.72f),
            0
        );

        //mouse
        var mouseRelativePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - curGun.transform.position;
        var mouseXNormalized = mouseRelativePos.x > 0 ? 1 : -1;

        if (!SingletonManager.GetSingleton<SettingUI>().IsDisplayingUI)
            sr.flipX = mouseXNormalized < 0;

        //animator
        anim.SetBool("isMove", isMove);
        anim.SetFloat("moveDir", (h != 0 ? h : mouseXNormalized) * mouseXNormalized);

        //gun
        if (!SingletonManager.GetSingleton<SettingUI>().IsDisplayingUI)
        {
            curGun.SetRotation(Mathf.Atan2(mouseRelativePos.y, mouseRelativePos.x) * Mathf.Rad2Deg);
            if (Input.GetMouseButton(0)) curGun.Shoot();
            if (Input.GetKeyDown(KeyCode.R)) curGun.Reload();
        }

        //exp
        if (curExp >= maxExp)
        {
            curExp -= maxExp;
            curLvl++;
            maxExp = curLvl * 100;

            SingletonManager.GetSingleton<AbilitySelectUI>().DisplayUI();
        }

        //ability
        if (abilityLvls[(int)AbilityType.bomb] > 0)
        {
            if (curBombWaitTime > 0)
            {
                curBombWaitTime -= Time.deltaTime;
            }
            else
            {
                for (int i = 0; i < 2 + abilityLvls[(int)AbilityType.bomb]; i++)
                {
                    var endpos = (Vector2)transform.position + (Random.insideUnitCircle * 2);
                    var middlepos = (((Vector2)transform.position + endpos) / 2) + (Vector2.up * 2f);

                    var bomb = Instantiate(bombPrefeb, transform.position, Quaternion.identity).GetComponent<Bomb>();
                    bomb.ThrowBomb(transform.position, middlepos, endpos, 1f);
                }
                curBombWaitTime = bombWaitTime;
            }
        }

        if (abilityLvls[(int)AbilityType.sickel] > 0)
        {
            if (sickelWeapon == null) sickelWeapon = Instantiate(sickelPrefeb, transform).GetComponent<Sickel>();
            sickelWeapon.transform.localScale = Vector3.one + (Vector3.one * abilityLvls[(int)AbilityType.sickel] * 0.05f);
        }

        //esc
        if (Input.GetKeyDown(KeyCode.Escape) && !SingletonManager.GetSingleton<SettingUI>().IsPlayingAnimation)
        {
            SingletonManager.GetSingleton<SettingUI>().DisplayUI(!SingletonManager.GetSingleton<SettingUI>().IsDisplayingUI);
            if (SingletonManager.GetSingleton<SettingUI>().IsDisplayingUI)
            {
                preTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = preTimeScale;
            }
        }
    }
}
