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

    public int Dir;
    public float DelTime;
    public int Power;
    public int speed = 15;
    public int Arrowpatten;

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
        if (collision.tag == "Player")
        {
            Player.instance.GetComponent<Player>().Playerhurt(Power);
        }

    }

    public void DestoryObject()
    {
        Destroy(gameObject, DelTime);
    }
}
