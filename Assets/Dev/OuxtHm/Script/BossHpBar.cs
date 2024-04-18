using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    public static BossHpBar instance;
    public Boss boss;
    public RectTransform rect;
    public Animator anim;
    Image frontImg;     // ü�� �̹���
    Image backImg;      // ü�� �̹��� �׸���
    public float hpRatio;      // ü�� ����
    public float beforeHpRatio;     // ���� �� ü�� ����
    public bool runningCoroutine;        // �ڷ�ƾ ���ۿ���
    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
        rect = transform.GetChild(0).GetComponent<RectTransform>();
        frontImg = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        backImg = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    private void Start()
    {
        boss = Boss.Instance;
        anim.SetTrigger("Create");
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
        if (runningCoroutine)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(BackHpUpdate(0.3f));
        }
        else
        {
            StartCoroutine(BackHpUpdate(0.3f));
        }
    }
    
    public IEnumerator BackHpUpdate(float seconds)
    {
        runningCoroutine = false;
        yield return new WaitForSeconds(seconds);
        runningCoroutine = true;
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
        runningCoroutine = false;
    }

    public void OnDestroy()
    {
        Destroy(this.gameObject);
    }
}
