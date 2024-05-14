using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPb : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Transform pos;
    public Transform playerpos;
    Vector3 moveDirection = Vector3.right;  //������ ����

    public float DelTime;   //���ŵǴ� �ð�
    public int Power;   // ����ü �����
    public int dir; // ���ư��� ���Ⱚ
    public int movecheck;   // �����̴� ����ü ����
    public int speed;   // ����ü �ӵ�

    

    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();

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
        DestoryObject();
    }
    public void Update()
    {
        if (movecheck == 1)
            pos.position += moveDirection * speed * Time.deltaTime;
        if(movecheck == 2)
        {
            transform.position = Vector3.Lerp(transform.position, playerpos.position, Time.deltaTime * speed);

            Vector3 direction = playerpos.position - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation.eulerAngles = new Vector3(0, 0, transform.rotation.eulerAngles.z);  // x, y�� ����, z�� ����
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.Playerhurt(Power);
        }
    }
    public void DestoryObject()
    {
        Destroy(gameObject, DelTime);
    }
}
