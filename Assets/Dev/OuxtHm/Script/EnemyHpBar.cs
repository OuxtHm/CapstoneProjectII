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
        StartCoroutine(HpUpdate());
    }
    public IEnumerator GetEnemyInfo()
    {
        yield return new WaitForSeconds(0.2f);
        hpRatio = enemy.enemy_CurHP / enemy.enemy_CurHP;
    }
    public IEnumerator HpUpdate()
    {
        yield return new WaitForSeconds(0.3f);
        while(Mathf.Abs(hpRatio - (enemy.enemy_CurHP / enemy.enemy_MaxHP)) >= 0.1f) 
        { 
            hpRatio = Mathf.Lerp(hpRatio, enemy.enemy_CurHP / enemy.enemy_MaxHP, Time.deltaTime * 2);
            hpBar.fillAmount = hpRatio;
            yield return null;
        }
        hpRatio = enemy.enemy_CurHP / enemy.enemy_MaxHP;
        hpBar.fillAmount = hpRatio; 
    }

}
