using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPb : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Transform pos;
    Animator anim;
    public Transform playerpos;
    Vector3 moveDirection = Vector3.right;  //나가는 방향
    Vector3 target;

    public float DelTime;   //제거되는 시간
    public int Power;   // 투사체 대미지
    public int dir; // 날아가는 방향값
    public int movecheck;   // 움직이는 투사체 구분
    public int speed;   // 투사체 속도
    float animTime = 2f;    //2스테이지 개구리몬스터 투사체 애니메이션 시간



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
        target = (playerpos.position - transform.position).normalized;  // 목표 지점 방향 벡터
        DestoryObject();
    }
    public void Update()
    {
        if (movecheck == 1) // 1이면 직선방향으로 이동, 2이면 타겟 위치로 이동
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
