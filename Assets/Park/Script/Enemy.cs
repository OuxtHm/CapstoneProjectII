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
    //Player player;

    int DirX;   //몬스터가 바라보는 방향값
    public float detectionRange = 5f;  //몬스터의 타겟 인식 범위
    float distanceToTarget; // 몬스터와 타겟 사이의 거리
    bool isChasing = false; // 추적중인지 확인
    int originalSpeed;    // 원래 speed값 저장
    float target_X;
    float enemy_X;

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
        TargetSenser(target);
        Sensor();
    }

    void TargetSenser(Transform target)  // 플레이어 추적
    {
        rigid = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        target_X = target.position.x;
        distanceToTarget = Vector3.Distance(this.transform.position, target.position); // 몬스터와 타겟 사이의 거리 계산

        if (distanceToTarget <= detectionRange && !isChasing) // 타겟이 범위 안에 있을 때 수행
        {
            /*if (!isChasing)
            {
                originalSpeed = enemy_Speed; // 추적 시작 시 원래 속도 저장
                enemy_Speed += 2; // 추적 중인 경우 속도 증가
            }*/
            anim.SetBool("Move", true);
            Vector2 direction = (target.position - transform.position).normalized;
            if (target_X < 0)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
            direction.y = transform.position.y;
            direction.Normalize();
            transform.Translate(direction * Time.deltaTime * enemy_Speed);
            
            Debug.Log("타겟을 감지했습니다!");
        }
        else // 타겟이 범위 밖에 있을 때 수행
        {
            /*if (isChasing)
            {
                enemy_Speed = originalSpeed; // 추적 종료 시 원래 속도로 복원
                isChasing = false;
            }*/
            isChasing = false;
            Move();
            Debug.Log("타겟이 감지 범위 밖에 있습니다.");
        }
    }

    public void Move()
    {
        if(DirX != 0)
        {
            anim.SetBool("Move", true);
            transform.Translate(new Vector2(DirX, transform.position.y).normalized * Time.deltaTime * enemy_Speed);
        }
        else
        {
            anim.SetBool("Move", false);
        }
            

        if (DirX == -1)
        {
            spriteRenderer.flipX = true;
        }
        else if (DirX == 1)
        {
            spriteRenderer.flipX = false;
        }
    }

    IEnumerator NextMove()  // 몬스터가 다음 실행할 이동 방향
    {
        DirX = Random.Range(-1, 2);
        Debug.Log(DirX);
        float NextMoveTime = Random.Range(3f, 5f);
        yield return new WaitForSeconds(NextMoveTime);
        StartCoroutine(NextMove());
    }

    public void Sensor()    // 타일 감지 함수
    {
        rigid = this.GetComponent<Rigidbody2D>();

        // Enemy의 한 칸 앞의 값을 얻기 위해 자기 자신의 위치 값에 (x)에 + nextDirX값을 더하고 1.2f를 곱한다.
        Vector2 frontVec = new Vector2(rigid.position.x + DirX * 1.2f, rigid.position.y);

        Debug.DrawRay(frontVec, Vector3.down * 2.5f, new Color(0, 1, 0));

        // 레이저를 아래로 쏘아서 실질적인 레이저 생성(물리기반), LayMask.GetMask("")는 해당하는 레이어만 스캔함
        rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2.5f, LayerMask.GetMask("Tilemap", "UI"));
        if (rayHit.collider == null && enemy_CurHP >= 0)
        {
            Debug.Log("rayHit null임");
            isChasing = true;
            Turn();
        }

    }

    void Turn() // 이미지를 반대로 바꾸는 함수
    {
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        DirX *= -1;   // nextDirX에 -1을 곱해 방향전환
        if (DirX == 1 && distanceToTarget > detectionRange)  // distanceToTarget > detectionRange를 추가하지 않으면 플레이어가 사거리 내에 있고 rayHit=null이라면 제자리 돌기함
        {
            spriteRenderer.flipX = true; // DirX 값이 1이면 x축을 flip함
        }
    }

    private void OnDrawGizmos()
    {
        // 감지 범위를 시각적으로 나타내기 위한 코드
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public abstract void InitSetting(); // 적의 기본 정보를 설정하는 함수(추상)
}
