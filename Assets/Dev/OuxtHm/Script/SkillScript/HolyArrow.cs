using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyArrow : MonoBehaviour
{   
    Enemy enemy;
    Boss boss;
    public SkillScriptable skillInfo;
    public Player player;
    private SpriteRenderer sprite;
    private CapsuleCollider2D capsuleCollider;
    public float arrowDamage = 10;
    public float speed = 20f; // �ӵ�
    public float duration = 0.5f; // �̵� �ð�
    public int direction;      // ����
    private void Awake()
    {
        sprite = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        capsuleCollider = this.gameObject.GetComponent<CapsuleCollider2D>();
        direction = 0;
    }
    private void Start()
    {
        player = Player.instance;
        direction = player.sr.flipX ? -1 : 1;
        sprite.flipX = player.sr.flipX;
        StartCoroutine(MoveProjectile());
    }

    public IEnumerator MoveProjectile()
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            transform.Translate(new Vector2(direction, 0) * speed * Time.deltaTime);

            // ���� �������� ������ �ֱ�
            if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
            {
                enemy = capsuleCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    StartCoroutine(enemy.Hurt(this.transform, arrowDamage));
                }
            }

            if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Boss")))
            {
                boss = capsuleCollider.GetComponent<Boss>();
                if (boss != null)
                {
                    StartCoroutine(boss.Hurt(this.transform, arrowDamage));
                }
            }

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
