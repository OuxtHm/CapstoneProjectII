using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyPillar : MonoBehaviour
{
    Enemy enemy;
    Boss boss;
    BoxCollider2D boxCollider2D;
    public int PillarDamage = 20;

    private void Awake()
    {
        boxCollider2D = this.GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        StartCoroutine(ChangeBoxOffsetSize());
    }

    private IEnumerator ChangeBoxOffsetSize()
    {
        Vector2[] offsets = new Vector2[] { new Vector2(0, -1.77f), new Vector2(0, -0.5f), new Vector2(0, -0.2f) };
        Vector2[] sizes = new Vector2[] { new Vector2(6, 3), new Vector2(3, 5), new Vector2(1.5f, 5.5f) };

        boxCollider2D.enabled = true;

        for (int i = 0; i < offsets.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            SetBoxColliderProperties(offsets[i], sizes[i]);

            // 적과 보스에게 데미지 주기
            if (boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
            {
                enemy = boxCollider2D.GetComponent<Enemy>();
                if (enemy != null)
                {
                    StartCoroutine(enemy.Hurt(this.transform, PillarDamage));
                }
            }

            if (boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Boss")))
            {
                boss = boxCollider2D.GetComponent<Boss>();
                if (boss != null)
                {
                    StartCoroutine(boss.Hurt(this.transform, PillarDamage));
                }
            }
        }

        boxCollider2D.enabled = false;

        yield return new WaitForSeconds(1.3f);
        Destroy(this.gameObject);
    }

    private void SetBoxColliderProperties(Vector2 offset, Vector2 size)
    {
        boxCollider2D.offset = offset;
        boxCollider2D.size = size;
    }
}
