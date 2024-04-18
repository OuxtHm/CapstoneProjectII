using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyArrow : MonoBehaviour
{
    public Player player;
    private SpriteRenderer sprite;
    private CapsuleCollider2D capsuleCollider;
    public int arrowDamage = 10;
    public float speed = 20f; // 속도
    public float duration = 0.5f; // 이동 시간
    public int direction;      // 방향
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
            yield return null; 
        }

        Destroy(this.gameObject);
    }

}
