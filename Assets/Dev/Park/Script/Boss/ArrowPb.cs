using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArrowPb : MonoBehaviour
{
    Boss boss;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Transform pos;
    Vector3 moveDirection = Vector3.right; // 화살이 나가는 방향

    public int Dir; // 날아가는 방향값
    public float DelTime;   //제거되는 시간
    public int Power;   // 투사체 대미지
    public int speed = 20;  // 투사체 속도
    public int Arrowpatten; //공격 패턴

    void Start()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        boss = this.GetComponent<Boss>();

        if (Dir == 1)
        {
            spriteRenderer.flipX = false;
            moveDirection = Vector3.right;
        }
        else
        {
            spriteRenderer.flipX = true;
            moveDirection = Vector3.left;
        }
        pos = transform;
        DestoryObject();
    }

    private void Update()
    {
        if(Arrowpatten == 1)
            pos.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = collision.GetComponent<Player>();
            collision.GetComponent<Player>().StartCoroutine(player.Playerhurt(Power));
        }

    }

    public void DestoryObject()
    {
        Destroy(gameObject, DelTime);
    }
}
