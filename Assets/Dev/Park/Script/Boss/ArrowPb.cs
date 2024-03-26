using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArrowPb : MonoBehaviour
{
    Player player;
    Boss boss;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    public int Dir;
    public float DelTime;
    public int Power;
    void Start()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        DestoryObject();
    }

    private void Update()
    {
        if (Dir == 1)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
        transform.Translate(new Vector2(Dir, transform.position.y).normalized * Time.deltaTime);
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
