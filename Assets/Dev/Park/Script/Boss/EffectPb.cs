using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPb : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Transform pos;
    Animator anim;
    public Transform playerpos;
    Vector3 moveDirection = Vector3.right;  //������ ����
    Vector3 target;

    public float DelTime;   //���ŵǴ� �ð�
    public int Power;   // ����ü �����
    public int dir; // ���ư��� ���Ⱚ
    public int movecheck;   // �����̴� ����ü ����
    public int speed;   // ����ü �ӵ�
    float animTime = 2f;    //2�������� ���������� ����ü �ִϸ��̼� �ð�



    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (dir == 1)
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
        target = (playerpos.position - transform.position).normalized;  // ��ǥ ���� ���� ����
        DestoryObject();
    }
    public void Update()
    {
        if (movecheck == 1) // 1�̸� ������������ �̵�, 2�̸� Ÿ�� ��ġ�� �̵�
            pos.position += moveDirection * speed * Time.deltaTime;
        if(movecheck == 2)
        {
            transform.Translate(target * speed * Time.deltaTime);
        }

        animTime -= Time.deltaTime;
        if(this.gameObject.name == "FireBarrier(Clone)")
        {
            if (animTime < 0.5)
                anim.SetBool("Explosion", true);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.Playerhurt(Power, this.transform);
            if (movecheck == 2)
                Destroy(gameObject);
        }
    }
    public void DestoryObject()
    {
        Destroy(gameObject, DelTime);
    }
}
