using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill2 : MonoBehaviour
{
    public SkillScriptable skillInfo;
    public float damage = 30;
    Enemy enemy;
    Boss boss;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemy = collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                StartCoroutine(enemy.Hurt(this.transform, damage));
            }
        }


        if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            boss = collider.GetComponent<Boss>();

            if (boss != null)
            {
                StartCoroutine(boss.Hurt(this.transform, damage));
            }
        }
    }
}

