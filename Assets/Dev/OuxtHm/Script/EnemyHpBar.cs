using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public Enemy enemy;
    public static EnemyHpBar instance;
    Image hpBar;
    float hpRatio;      // ü�� ����

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        enemy = this.GetComponentInParent<Enemy>();
        hpBar = transform.GetChild(1).GetComponent<Image>();
        StartCoroutine(GetEnemyInfo());
    }
    public IEnumerator GetEnemyInfo()
    {
        yield return new WaitForSeconds(0.2f);
        hpRatio = enemy.enemy_CurHP / enemy.enemy_CurHP;
    }
    public IEnumerator HpUpdate()
    {
        float startTime = Time.time; 
        float lerpDuration = 0.3f; 
        float startRatio = hpRatio; 
        float endRatio = enemy.enemy_CurHP / enemy.enemy_MaxHP; // ��ǥ hp ����

        while (Time.time - startTime <= lerpDuration)
        {
            float timeElapsed = (Time.time - startTime) / lerpDuration; // �ð��� ���� 0���� 1�� ��ȭ
            hpRatio = Mathf.Lerp(startRatio, endRatio, timeElapsed);
            hpBar.fillAmount = hpRatio;
            yield return null;
        }

        // ���� �Ϸ� �� ���� ���� Ȯ���� ����
        hpRatio = endRatio;
        hpBar.fillAmount = hpRatio;
    }
}
