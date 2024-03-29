using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public Enemy enemy;
    public static EnemyHpBar instance;
    Image hpBar;
    float hpRatio;      // 체력 비율

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
        float endRatio = enemy.enemy_CurHP / enemy.enemy_MaxHP; // 목표 hp 비율

        while (Time.time - startTime <= lerpDuration)
        {
            float timeElapsed = (Time.time - startTime) / lerpDuration; // 시간에 따라 0에서 1로 변화
            hpRatio = Mathf.Lerp(startRatio, endRatio, timeElapsed);
            hpBar.fillAmount = hpRatio;
            yield return null;
        }

        // 보간 완료 후 최종 값을 확실히 적용
        hpRatio = endRatio;
        hpBar.fillAmount = hpRatio;
    }
}
