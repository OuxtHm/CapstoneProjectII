using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public SkillScriptable skillInfo;
    BoxCollider2D boxCollider2D;
    public Enemy enemy;
    Boss boss;

    public int thunderDamage = 20;

    private void Awake()
    {
        boxCollider2D = this.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        StartCoroutine(ChangeBoxOffsetSize());
    }
    public IEnumerator ChangeBoxOffsetSize()
    {
        yield return new WaitForSeconds(0.5f);
        boxCollider2D.enabled = true;
        yield return new WaitForSeconds(0.4f);
        boxCollider2D.enabled = false;
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemy = collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                StartCoroutine(enemy.Hurt(this.transform, thunderDamage));
            }
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            boss = collider.GetComponent<Boss>();

            if (boss != null)
            {
                StartCoroutine(boss.Hurt(this.transform, thunderDamage));
            }
        }
    }

}
