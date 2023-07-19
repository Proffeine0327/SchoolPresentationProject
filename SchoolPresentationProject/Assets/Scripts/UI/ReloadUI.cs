using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadUI : MonoBehaviour
{
    [SerializeField] private GameObject bg;
    [SerializeField] private Image bar;

    private float time;
    private float curTime;

    private void Awake() 
    {
        SingletonManager.RegisterSingleton(this);
    }

    public void SetTime(float t)
    {
        this.time = t;
        this.curTime = t;
    }

    private void LateUpdate() 
    {
        bg.transform.position = 
            Camera.main.WorldToScreenPoint(SingletonManager.GetSingleton<Player>().transform.position + new Vector3(0.5f, 1, 0));
    }

    private void Update() 
    {
        if(curTime > 0)
        {
            bg.SetActive(true);
            curTime -= Time.deltaTime;
            bar.fillAmount = 1 - curTime / time;
        }
        else bg.SetActive(false);
    }
}
