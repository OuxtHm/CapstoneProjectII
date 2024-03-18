using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Enemy : MonoBehaviour
{
    public static Enemy Instance;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;
    Transform target;
    RaycastHit2D rayHit;

    int DirX;   //몬스터가 바라보는 방향값
    public float detectionRange = 4f;  //몬스터의 타겟 인식 범위
    float distanceToTarget; // 몬스터와 타겟 사이의 거리
    bool istracking = false;    // 추적 가능 여부
    int enemy_OriginSpeed;  //몬스터의 원래 속도
    protected int enemy_Type; // 몬스터 종류에 따른 분류 번호 1: 일반 지상 몬스터, 2: 일반 공중 몬스터
    bool isdie = false;
    bool ishurt = false;
    bool isattack = false;

    [Header("일반 몬스터 능력치")]
    public int enemy_MaxHP; //일반 몬스터 최대체력
    public int enemy_CurHP; //일반 몬스터 현재체력
    public int enemy_Power; //일반 몬스터 공격력
    public int enemy_Speed; //일반 몬스터 이동속도
    //public int ActionPattern; //일반 몬스터 공격 패턴

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        rigid = this.GetComponent<Rigidbody2D>();
        StartCoroutine(NextMove());
    }

    public virtual void Short_Monster(Transform target)
    {
        TargetSensor(target, rayHit);
        Sensor();
    }

    void TargetSensor(Transform target, RaycastHit2D rayHit)  // 플레이어 추적
    {
        rigid = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        distanceToTarget = Vector3.Distance(this.transform.position, target.position); // 몬스터와 타겟 사이의 거리 계산
        Vector2 direction = (target.position - transform.position).normalized;
        
            
        if (distanceToTarget <= detectionRange && !ishurt && !isdie && !isattack && enemy_Type != 2) // 타겟이 범위 안에 있을 때 수행
        {
            if(rayHit.collider != null && !istracking && enemy_Type == 1) // 지상 몬스터 일때
            {
                direction.y = transform.position.y; // y값 위치 고정을 위해 추가
                direction.Normalize();

                if (direction.x >= 0)   // 타겟이 오른쪽에 있을 때
                {
                    DirX = 1;
                    spriteRenderer.flipX = false;
                }
                else
                {
                    DirX = -1;
                    spriteRenderer.flipX = true;
                }
                anim.SetBool("Move", true);
                transform.Translate(direction * Time.deltaTime * enemy_Speed);

                if(distanceToTarget <= 2.5f && !isattack && !ishurt)
                {
                    StartCoroutine(Attack());
                }
                //Debug.Log(distanceToTarget);
                //Debug.Log("지상 추적중");

            }
            else if(rayHit.collider == null)
            {
                istracking = true;
                anim.SetBool("Move", false);
            }
            else //추적중에 바닥이 없으면 타겟 인식 범위까지 반대방향으로 이동함
            {
                Move();
            }

            
        }
        else if (enemy_Type == 2)  // 공중 몬스터 일때
        {
            if (direction.x >= 0)   // 타겟이 오른쪽에 있을 때
            {
                DirX = 1;
                spriteRenderer.flipX = false;
            }
            else
            {
                DirX = -1;
                spriteRenderer.flipX = true;
            }
            anim.SetBool("Move", true);
            Vector2 targetPosition = new Vector2(target.position.x - 1, target.position.y - 2);
            Vector2 targetDirection = (targetPosition - (Vector2)transform.position).normalized;
            transform.Translate(targetDirection * Time.deltaTime * enemy_Speed);
            Debug.Log("공중 추적중");
        }
        else if(distanceToTarget >= detectionRange) // 타겟이 범위 밖에 있을 때 수행
        {
            istracking = false;
            Move();
        }
    }


    public void Move()
    {
        if (DirX != 0 && !isdie && !ishurt)
        {
            anim.SetBool("Move", true);
            transform.Translate(new Vector2(DirX, transform.position.y).normalized * Time.deltaTime * enemy_Speed);

            if (DirX == -1)
            {
                spriteRenderer.flipX = true;
            }
            else if (DirX == 1)
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            anim.SetBool("Move", false);
        }
    }

    IEnumerator NextMove()  // 몬스터가 다음 실행할 이동 방향
    {
        DirX = Random.Range(-1, 2);
        float NextMoveTime = Random.Range(3f, 5f);
        yield return new WaitForSeconds(NextMoveTime);
        StartCoroutine(NextMove());
    }

    public void Sensor()    // 타일 감지 함수
    {
        rigid = this.GetComponent<Rigidbody2D>();

        // Enemy의 한 칸 앞의 값을 얻기 위해 자기 자신의 위치 값에 (x)에 + DirX값을 더하고 1.2f를 곱한다.
        Vector2 frontVec = new Vector2(rigid.position.x + DirX * 1.2f, rigid.position.y);

        // 레이저를 씬창에서 눈으로 확인할 수 있게 그려줌
        Debug.DrawRay(frontVec, Vector3.down * 2.5f, new Color(0, 1, 0));
        
        // 물리 기반으로 레이저를 아래로 쏘아서 실질적인 레이저 생성, LayMask.GetMask("")는 해당하는 레이어만 스캔함
        rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2.5f, LayerMask.GetMask("Tilemap", "UI"));   //레이어는 맵 레이어가 정해지면 수정해야함
        if (rayHit.collider == null && enemy_CurHP >= 0 && enemy_Type != 2)
        {
            Turn();
        }
    }

    void Turn() // 이미지를 반대로 바꾸는 함수
    {
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        DirX *= -1;   // nextDirX에 -1을 곱해 방향 전환
        if (DirX == 1 && distanceToTarget > detectionRange)  // distanceToTarget > detectionRange를 추가하지 않으면 플레이어가 사거리 내에 있고 rayHit=null이라면 제자리 돌기함
        {
            spriteRenderer.flipX = false; // DirX 값이 1이면 x축을 flip함
        }
        else
        {
            spriteRenderer.flipX = true; // DirX 값이 1이 아니면 x축 flip을 해제함
        }
    }

    IEnumerator Attack()
    {
        enemy_OriginSpeed = enemy_Speed;
        enemy_Speed = 0;
        isattack = true;
        anim.SetBool("Move", false);
        anim.SetTrigger("Attack");
        Debug.Log("공격 실행");
        yield return new WaitForSeconds(2f);
        isattack = false;
        enemy_Speed = enemy_OriginSpeed;
    }

   

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && !isdie && !ishurt)
        {
            StartCoroutine(Hurt(collision.transform));
        }
    }

    IEnumerator Hurt(Transform target)  //플레이어에게 피격 받았을 때 실행
    {
        if(enemy_CurHP > 0 && !isdie)
        {
            ishurt = true;
            Debug.Log(istracking);
            enemy_CurHP = enemy_CurHP - 1;
            anim.SetBool("Move", false);
            anim.SetTrigger("Hurt");
            enemy_OriginSpeed = enemy_Speed;
            enemy_Speed = 0;

            StartCoroutine(Blink());
            StartCoroutine(Knockback(target));

            if (enemy_CurHP <= 0)
            {
                isdie = true;
                StopAllCoroutines();
                StartCoroutine(Die());
                Debug.Log("죽었음");
            }
        }

        

        yield return new WaitForSeconds(0.5f);
        enemy_Speed = enemy_OriginSpeed;
        ishurt = false;
    }

    IEnumerator Die()  //몬스터가 죽었을 실행
    {
        istracking = true;
        DirX = 0;
        anim.SetBool("Move", false);
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        //StopAllCoroutines();
        //gameObject.SetActive(false);
        //Destroy(gameObject); 삭제할거면 이 방법을 사용
    }

    IEnumerator Knockback(Transform target)
    {
        Vector3 knockbackDirection = transform.position - target.position;  //피격된 위치를 저장
        knockbackDirection.Normalize();
        rigid.AddForce(knockbackDirection * 5f, ForceMode2D.Impulse); // 피격된 위치 * 원하는 힘의 크기만큼 넉백. ForceMode2D.Impulse를 사용하면 순간적인 강한 힘을 줄 수 있음 
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator Blink() // 피격 효과
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = originalColor;
    }

    private void OnDrawGizmos()
    {
        // 감지 범위를 시각적으로 나타내기 위한 코드
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public abstract void InitSetting(); // 적의 기본 정보를 설정하는 함수(추상)
}
