using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Enemy : MonoBehaviour
{
    public static Enemy Instance;
    Player player;
    EnemyHpBar enemyHpbar;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;
    Transform AttackBox;
    BoxCollider2D AttackBoxSize; //공격 범위 콜라이더 사이즈
    RaycastHit2D rayHit;
    RaycastHit2D rayHitAtk;

    public GameObject ExplosionPb; //개구리 몬스터 독안개 프리펩

    int DirX;   //몬스터가 바라보는 방향값
    public float detectionRange = 7f;  //몬스터의 타겟 인식 범위
    float distanceToTarget; // 몬스터와 타겟 사이의 거리
    bool istracking = false;    // 추적 가능 여부
    int enemy_OriginSpeed;  //몬스터의 원래 속도
    int atkPattern; //몬스터 공격 패턴
    bool isdie = false; //죽음 확인
    bool ishurt = false;    //피격 적용 확인
    bool isattack = false;  //공격 가능 확인
    bool giveDmg = false;   //개구리 투사체 대미지 확인

    [Header("일반 몬스터 능력치")]
    protected int enemy_Type; // 몬스터 종류에 따른 분류 번호 1: 일반 몬스터, 2: 공중 몬스터, 3: 충돌 몬스터
    public float enemy_MaxHP; //일반 몬스터 최대체력
    public float enemy_CurHP; //일반 몬스터 현재체력
    public int enemy_Power; //일반 몬스터 공격력
    public int enemy_Speed; //일반 몬스터 이동속도
    public float enemy_AttackSensor;  //일반 몬스터 플레이어 감지 범위

    private void Awake()
    {
        Instance = this;
        enemyHpbar = this.transform.GetChild(1).GetComponent<EnemyHpBar>();
    }

    private void Start()
    {
        player = GetComponent<Player>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        rigid = this.GetComponent<Rigidbody2D>();
        AttackBox = this.gameObject.transform.GetChild(0).GetComponent<Transform>();
        StartCoroutine(NextMove());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            StartCoroutine(Hurt(AttackBox));
    }

    public virtual void Short_Monster(Transform target)
    {
        TargetSensor(target);
        Sensor();
    }

    void TargetSensor(Transform target)  // 플레이어 추적
    {
        rigid = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        float distanceToTarget = Mathf.Abs(target.position.x - transform.position.x); // 몬스터와 타겟 사이의 x축 거리 계산
        Vector2 direction = (target.position - transform.position).normalized;

        //몬스터별 플레이어를 감지하여 공격을 실행하는 raycast 범위 설정
        float dir = DirX > 0 ? 1 : -1; // DirX가 양수이면 1, 음수이면 -1을 direction으로 설정
        Vector2 rayDirection = new Vector2(dir, 0);
        rayHitAtk = Physics2D.Raycast(rigid.position, rayDirection, enemy_AttackSensor, LayerMask.GetMask("Player"));
        Debug.DrawRay(rigid.position, rayDirection * enemy_AttackSensor, new Color(1, 0, 0));

        if (distanceToTarget <= detectionRange && !ishurt && !isdie && enemy_Type != 2) //공중 몬스터 이외의 몬스터가 타겟이 범위 안에 있을 때 수행
        {
            if (rayHit.collider != null && !istracking && !isattack)
            {
                direction.y = 0; // y값 위치 고정을 위해 추가
                direction.Normalize();
                if (direction.x >= 0)   // 타겟이 오른쪽에 있을 때
                {
                    DirX = 1;
                    spriteRenderer.flipX = false;
                    AttackBox.position = new Vector2(transform.position.x + 1, transform.position.y);
                }
                else
                {
                    DirX = -1;
                    spriteRenderer.flipX = true;
                    AttackBox.position = new Vector2(transform.position.x - 1, transform.position.y);
                }
                
                if((target.position.y - transform.position.y) <= 4f)   //타겟과 어느정도의 높이 차이가 있을경우 추적 멈춤
                {
                    anim.SetBool("Move", true);
                    transform.Translate(direction * Time.deltaTime * enemy_Speed);
                }
                else
                {
                    anim.SetBool("Move", false);
                }
                    

                if (rayHitAtk.collider != null && !isattack && !ishurt && enemy_Type == 1)   //일반 몬스터의 공격 실행
                {
                    StartCoroutine(Attack());
                }
            }
            else if(rayHit.collider == null)  //바닥이 없으면 추적 종료
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
            Vector2 targetPosition = new Vector2(target.position.x - 1, target.position.y); // 타겟의 정면으로 따라가기 위해 x-1, y-2를 해줬음
            Vector2 targetDirection = (targetPosition - (Vector2)transform.position).normalized;
            transform.Translate(targetDirection * Time.deltaTime * enemy_Speed);
        }
        else if(distanceToTarget >= detectionRange) // 타겟이 범위 밖에 있을 때 수행
        {
            istracking = false;
            Move();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision != null && collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Playerhurt(enemy_Power);
                if (enemy_Type == 3)
                {

                }
            }
            else
                Debug.Log("플레이어를 못 불러옴");
        }
    }

    
    private void OnCollisionStay2D(Collision2D collision)   //벽에 닿았을 시 반대로 돌아감
    {
        if (collision.gameObject.CompareTag("wall") && enemy_Type != 2)
        {
            Turn();
        }
    }
    
    public void Move()  //몬스터 기본 이동 동작
    {
        if (DirX != 0 && !isdie && !ishurt && !isattack)
        {
            anim.SetBool("Move", true);
            gameObject.transform.Translate(new Vector2(DirX, 0) * Time.deltaTime * enemy_Speed);

            if (DirX == -1)
            {
                spriteRenderer.flipX = true;
                AttackBox.position = new Vector2(transform.position.x - 1, transform.position.y);
            }
            else if (DirX == 1)
            {
                spriteRenderer.flipX = false;
                AttackBox.position = new Vector2(transform.position.x + 1, transform.position.y);
            }
        }
        else
        {
            anim.SetBool("Move", false);
        }
    }

    IEnumerator NextMove()  // 몬스터가 다음 실행할 이동 방향
    {
        if(!istracking)
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
        rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2.5f, LayerMask.GetMask("Ground"));   //레이어는 맵 레이어가 정해지면 수정해야함
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

    IEnumerator Attack()    //몬스터 공격 함수
    {
        atkPattern = Random.Range(1, 3);
        anim.SetTrigger("Attack");
        anim.SetFloat("Attackpatten", atkPattern);
        enemy_OriginSpeed = enemy_Speed;
        isattack = true;

        yield return new WaitForSeconds(0.7f);
        AttackBoxSize = this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(AttackBox.position, AttackBoxSize.size, 0);

        foreach (Collider2D collider in collider2D)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<Player>().Playerhurt(enemy_Power);
            }
        }
        yield return new WaitForSeconds(1.5f);
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
        isattack = false;
    }

    IEnumerator FrogExplosion() //개구리 몬스터 투사체 패턴 - 애니메이션에서 실행됨
    {
        int dirx = 1;
        GameObject Explosion = Instantiate(ExplosionPb, AttackBox.position, AttackBox.rotation);
        Animator anim = Explosion.GetComponent<Animator>();
        if (DirX != 0)
            dirx = DirX;
            Vector2 dir = new Vector2(dirx, 0);
        float DelTime = 2f;
        giveDmg = false;
        while (DelTime >= 0)
        {
            DelTime -= Time.deltaTime;
            if(!giveDmg)
                ExplosionGiveDamage(Explosion);
            Explosion.transform.Translate(dir * Time.deltaTime * 1); // 이동
            yield return new WaitForEndOfFrame();
        }
        anim.SetBool("Explosion",true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Explosion", false);
        Destroy(Explosion); 
    }

    void ExplosionGiveDamage(GameObject explosion)  //투사체 대미지 기능 동작
    {
        AttackBoxSize = ExplosionPb.gameObject.transform.GetComponent<BoxCollider2D>();
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(explosion.transform.position, AttackBoxSize.size, 0);
        foreach (Collider2D collider in collider2D)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<Player>().Playerhurt(enemy_Power);
                giveDmg = true; //한 번만 대미지를 주기 위해 사용
            }
        }
    }


    IEnumerator Hurt(Transform target)  //플레이어에게 피격 받았을 때 실행
    {
        if(enemy_CurHP > 0 && !isdie && !ishurt)
        {
            ishurt = true;
            enemy_CurHP = enemy_CurHP - 1;
            StartCoroutine(enemyHpbar.HpUpdate());      // 2024-03-30 유재현 추가
            anim.SetBool("Move", false);
            anim.SetTrigger("Hurt");
            if(enemy_Speed > 0)
                enemy_OriginSpeed = enemy_Speed;
            enemy_Speed = 0;

            StartCoroutine(Blink());
            StartCoroutine(Knockback(target));
            if (enemy_CurHP <= 0)
            {
                isdie = true;
                StopAllCoroutines();
                StartCoroutine(enemyHpbar.HpUpdate());      // 2024-03-30 유재현 추가
                StartCoroutine(Die());
                Debug.Log("죽었음");
            }
        }

        yield return new WaitForSeconds(0.3f);
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
    }

    IEnumerator Knockback(Transform target)
    {
        Vector3 knockbackDirection = transform.position - target.position;  // 피격된 위치를 저장
        knockbackDirection.Normalize();

        float maxKnockbackDistance = 3f;    //넉백 가능한 최대 거리
        float knockbackDistance = 2.0f;  // 넉백 거리를 나타내는 변수
        rigid.AddForce(knockbackDirection * knockbackDistance, ForceMode2D.Impulse);  // 피격된 위치 * 넉백 거리만큼의 힘을 넉백에 사용
        float distanceTravelled = 0f;  // 이미 이동한 거리를 나타내는 변수

        while (distanceTravelled < maxKnockbackDistance)
        {
            distanceTravelled += knockbackDistance * Time.fixedDeltaTime;  // 이동한 거리를 누적
            yield return new WaitForFixedUpdate();  // Fixed Update마다 체크하여 일정 거리까지만 이동하도록 함
        }
        yield return new WaitForSeconds(0.3f);
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
