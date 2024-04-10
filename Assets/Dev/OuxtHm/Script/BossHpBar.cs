using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    public static BossHpBar instance;
    public Boss boss;
    Image frontImg;     // 체력 이미지
    public Image backImg;      // 체력 이미지 그림자
    public float hpRatio;      // 체력 비율
    public float beforeHpRatio;     // 변경 전 체력 비율

    private void Awake()
    {
        instance = this;
        frontImg = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        backImg = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    private void Start()
    {
        boss = Boss.Instance;
        hpRatio = 1;
        beforeHpRatio = hpRatio;
    }

    public IEnumerator FrontHpUpdate()
    {
        float startTime = Time.time;
        float lerpDuration = 0.3f;
        float startRatio = hpRatio;
        float endRatio = boss.boss_CurHP / boss.boss_MaxHP; // 목표 hp 비율
        while (Time.time - startTime <= lerpDuration)
        {
            float timeElapsed = (Time.time - startTime) / lerpDuration; // 시간에 따라 0에서 1로 변화
            hpRatio = Mathf.Lerp(startRatio, endRatio, timeElapsed);
            frontImg.fillAmount = hpRatio;
            yield return null;
        }
        
        hpRatio = endRatio;
        frontImg.fillAmount = hpRatio;
        StartCoroutine(BackHpUpdate(0.3f));
    }

    public IEnumerator BackHpUpdate(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        float startTime = Time.time;
        float lerpDuration = 0.3f;
        float startRatio = beforeHpRatio;
        float endRatio = boss.boss_CurHP / boss.boss_MaxHP; // 목표 hp 비율
        while (Time.time - startTime <= lerpDuration)
        {
            float timeElapsed = (Time.time - startTime) / lerpDuration; // 시간에 따라 0에서 1로 변화
            beforeHpRatio = Mathf.Lerp(startRatio, endRatio, timeElapsed);
            backImg.fillAmount = beforeHpRatio;
            yield return null;
        }

        beforeHpRatio = endRatio;
        backImg.fillAmount = beforeHpRatio;
    }
}
