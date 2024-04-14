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
    Vector3 moveDirection = Vector3.right; // ȭ���� ������ ����

    public int Dir; // ���ư��� ���Ⱚ
    public float DelTime;   //���ŵǴ� �ð�
    public int Power;   // ����ü �����
    public int speed = 20;  // ����ü �ӵ�
    public int Arrowpatten; //���� ����

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
