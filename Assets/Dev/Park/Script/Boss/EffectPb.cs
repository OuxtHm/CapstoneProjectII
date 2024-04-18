using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPb : MonoBehaviour
{
    public float DelTime;   //���ŵǴ� �ð�
    public int Power;   // ����ü �����
    public int dir;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        DestoryObject();
        
        if(dir > 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

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
