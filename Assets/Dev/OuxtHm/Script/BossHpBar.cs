using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    public static BossHpBar instance;
    public Boss boss;
    Image frontImg;     // ü�� �̹���
    public Image backImg;      // ü�� �̹��� �׸���
    public float hpRatio;      // ü�� ����
    public float beforeHpRatio;     // ���� �� ü�� ����

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
        float endRatio = boss.boss_CurHP / boss.boss_MaxHP; // ��ǥ hp ����
        while (Time.time - startTime <= lerpDuration)
        {
            float timeElapsed = (Time.time - startTime) / lerpDuration; // �ð��� ���� 0���� 1�� ��ȭ
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
        float endRatio = boss.boss_CurHP / boss.boss_MaxHP; // ��ǥ hp ����
        while (Time.time - startTime <= lerpDuration)
        {
            float timeElapsed = (Time.time - startTime) / lerpDuration; // �ð��� ���� 0���� 1�� ��ȭ
            beforeHpRatio = Mathf.Lerp(startRatio, endRatio, timeElapsed);
            backImg.fillAmount = beforeHpRatio;
            yield return null;
        }

        beforeHpRatio = endRatio;
        backImg.fillAmount = beforeHpRatio;
    }
}
