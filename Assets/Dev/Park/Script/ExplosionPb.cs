using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPb : MonoBehaviour
{
    Enemy enemy;
    Player player;
    Transform pos;
    public int Dir; // ���ư��� ���Ⱚ
    public float DelTime;   //���ŵǴ� �ð�
    public int Power;   // ����ü �����
    public int Speed;  // ����ü �ӵ�
    public float animTime = 2f;  // ���� �ִϸ��̼� ���۽ð�
    Vector3 MoveDirection = Vector3.right;

    Animator anim;
    void Start()
    {
        enemy = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        //player = Player.instance.GetComponent<Player>();

        if (Dir == 1)
        {
            MoveDirection = Vector3.right;
        }
        else
        {
            MoveDirection = Vector3.left;
        }
        pos = transform;
        DestoryObject();
    }

    private void Update()
    {
        pos.position += MoveDirection * Speed * Time.deltaTime;
        animTime -= Time.deltaTime;

        if(animTime < 0.5)
            anim.SetBool("Explosion", true);
        else
            anim.SetBool("Explosion", false);


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
