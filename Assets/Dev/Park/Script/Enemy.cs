using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Enemy : MonoBehaviour
{
    public static Enemy Instance;
    DataManager dm;
    Player player;
    Teleport teleport;
    EnemyHpBar enemyHpbar;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;
    Transform AttackBox;
    BoxCollider2D AttackBoxSize; //공격 범위 콜라이더 사이즈
    RaycastHit2D rayHit;    //바닥 감지 센서
    RaycastHit2D rayHitAtk; // 공격 범위 센서
    RaycastHit2D rayHitfront;   //벽 감지 센서

    public GameObject ExplosionPb; //2stage 개구리 몬스터 독안개 프리펩
    public GameObject RockPb;  //3stage 프리스트 공격 이펙트 프리펩
    public GameObject SeedPb;   //3stage 식물 씨앗공격 프리펩

    int DirX;   //몬스터가 바라보는 방향값
    int enemy_OriginSpeed;  //몬스터의 원래 속도
    int PBdir;  //투사체 방향값
    protected float detectionRange = 7f;  //몬스터의 타겟 인식 범위
    float distanceToTarget; // 몬스터와 타겟 사이의 거리
    public bool istracking = false;    // 추적 가능 여부
    bool isdie = false; //죽음 확인
    bool ishurt = false;    //피격 적용 확인
    public bool isattack = false;  //공격 가능 확인


    [Header("일반 몬스터 능력치")]
    protected int enemy_Type; // 몬스터 종류에 따른 분류 번호 1: 일반 몬스터, 2: 공중 몬스터, 3: 충돌 몬스터, 4: 고정 몬스터
    public float enemy_MaxHP; //일반 몬스터 최대체력
    public float enemy_CurHP; //일반 몬스터 현재체력
    protected int enemy_Power; //일반 몬스터 공격력
    protected int enemy_Speed; //일반 몬스터 이동속도
    protected float enemy_AttackSensor;  //일반 몬스터 플레이어 감지 범위
    protected float enemy_frontSensor; //일반 몬스터 전방 감지 범위
    public int enemyMoney;      // 몬스터 사망 시 플레이어에게 줄 돈 2024-05-15 유재현 추가

    private void Awake()
    {
        Instance = this;
        enemyHpbar = this.transform.GetChild(1).GetComponent<EnemyHpBar>();
        ExplosionPb = Resources.Load<GameObject>("Prefabs/Explosion");
        RockPb = Resources.Load<GameObject>("Prefabs/Rock");
        SeedPb = Resources.Load<GameObject>("Prefabs/Seed");
    }

    private void Start()
    {
        player = Player.instance.GetComponent<Player>();
        dm = DataManager.instance;
        teleport = Teleport.Instance.GetComponent<Teleport>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        rigid = this.GetComponent<Rigidbody2D>();
        AttackBox = this.gameObject.transform.GetChild(0).GetComponent<Transform>();
        StartCoroutine(NextMove());
    }

    public virtual void Short_Monster(Transform target)
    {
        TargetSensor(target);
        Sensor();
        if (Input.GetKeyDown(KeyCode.V))
            StartCoroutine(Hurt(this.transform, 10));
    }

    void TargetSensor(Transform target)  // 플레이어 추적
    {
        rigid = this.GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        float distanceToTarget = Mathf.Abs(target.position.x - transform.position.x); // 몬스터와 타겟 사이의 x축 거리 계산(절대값)
        Vector2 direction = (target.position - transform.position).normalized;

        //몬스터별 플레이어를 감지하여 공격을 실행하는 raycast 범위 설정
        float dir = DirX > 0 ? 1 : -1; // DirX가 양수이면 1, 음수이면 -1을 direction으로 설정
        Vector2 rayDirection = new Vector2(dir, 0);
        rayHitAtk = Physics2D.Raycast(rigid.position, rayDirection, enemy_AttackSensor, LayerMask.GetMask("Player"));
        Debug.DrawRay(rigid.position, rayDirection * enemy_AttackSensor, new Color(1, 0, 0));

        if (teleport.isTelepo == false) //플레이어가 포탈을 사용중일시 이동 제어
        {
            if (distanceToTarget <= detectionRange && !ishurt && !isdie && enemy_Type == 1 || enemy_Type == 3) //공중 몬스터 이외의 몬스터가 타겟이 범위 안에 있을 때 수행
            {
                if (!isattack)
                {
                    istracking = true;
                    direction.y = 0; // y값 위치 고정을 위해 추가
                    direction.Normalize();
                    if (direction.x >= 0)   // 타겟이 오른쪽에 있을 때
                    {
                        DirX = 1;
                        PBdir = 1;
                        spriteRenderer.flipX = false;
                        AttackBox.position = new Vector2(transform.position.x + 1, transform.position.y);
                    }
                    else
                    {
                        DirX = -1;
                        PBdir = -1;
                        spriteRenderer.flipX = true;
                        AttackBox.position = new Vector2(transform.position.x - 1, transform.position.y);
                    }

                    if (Mathf.Abs(target.position.y - transform.position.y) <= 4f)   //타겟과 어느정도의 높이 차이가 있을경우 추적 멈춤
                    {
                        anim.SetBool("Move", true);
                        Debug.Log("추적중 이동 실행");
                        Debug.Log(direction);
                        transform.Translate(direction * Time.deltaTime * enemy_Speed);
                    }
                    else
                    {
                        anim.SetBool("Move", false);
                        istracking = false;
                    }


                    if (rayHitAtk.collider != null && !isattack && !ishurt && enemy_Type == 1)   //일반 몬스터의 공격 실행
                    {
                        if (player.curHp > 0)
                            StartCoroutine(Attack());
                    }
                }
            }
            else if (distanceToTarget <= detectionRange && enemy_Type == 2 && !istracking)  // 공중 몬스터 일때
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
                Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
                Vector2 targetDirection = (targetPosition - (Vector2)transform.position).normalized;
                transform.Translate(targetDirection * Time.deltaTime * enemy_Speed);
            }
            else if (distanceToTarget >= detectionRange && enemy_Type != 2) // 타겟이 범위 밖에 있을 때 수행
            {
                istracking = false;
                Move();
            }
            else if(enemy_Type == 4)
            {
                if (direction.x >= 0)   // 타겟이 오른쪽에 있을 때
                {
                    DirX = 1;
                    spriteRenderer.flipX = false;
                    AttackBox.position = new Vector2(transform.position.x + 1.5f, transform.position.y);
                }
                else
                {
                    DirX = -1;
                    spriteRenderer.flipX = true;
                    AttackBox.position = new Vector2(transform.position.x - 1.5f, transform.position.y);
                }

                if (rayHitAtk.collider != null && !isattack)
                {
                    if (player.curHp > 0)
                        StartCoroutine(Attack());
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)  //충돌 대미지 주기
    {
        if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Playerhurt(enemy_Power, this.transform);
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
        if (teleport.isTelepo == false && enemy_Type != 4)
        {
            if (DirX != 0 && !isdie && !ishurt && !isattack && !istracking)
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
        Vector2 WallVec = new Vector2(rigid.position.x + DirX * enemy_frontSensor, rigid.position.y);

        // 레이저를 씬창에서 눈으로 확인할 수 있게 그려줌
        Debug.DrawRay(frontVec, Vector3.down * 2.5f, new Color(0, 1, 0));
        Debug.DrawRay(WallVec, Vector3.down * 0.3f, new Color(0, 0, 1));

        // 물리 기반으로 레이저를 아래로 쏘아서 실질적인 레이저 생성, LayMask.GetMask("")는 해당하는 레이어만 스캔함
        rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2.5f, LayerMask.GetMask("Ground"));
        rayHitfront = Physics2D.Raycast(WallVec, Vector3.down, 0.3f, LayerMask.GetMask("Ground","Wall"));
        if (rayHit.collider == null && enemy_CurHP >= 0 && enemy_Type != 2 && !istracking)
        {
            Turn();
        }
        if(rayHitfront.collider != null && enemy_CurHP >= 0 && enemy_Type != 2 && !istracking)
        {
            Turn();
        }
    }

    void Turn() // 이미지를 반대로 바꾸는 함수
    {
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        DirX *= -1;   // nextDirX에 -1을 곱해 방향 전환
        if (DirX == 1)  // distanceToTarget > detectionRange를 추가하지 않으면 플레이어가 사거리 내에 있고 rayHit=null이라면 제자리 돌기함
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
        anim.SetTrigger("Attack");
        enemy_OriginSpeed = enemy_Speed;
        isattack = true;

        yield return new WaitForSeconds(0.7f);
        AttackBoxSize = this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(AttackBox.position, AttackBoxSize.size, 0);

        foreach (Collider2D collider in collider2D)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                player = collider.GetComponent<Player>();
                player.Playerhurt(enemy_Power, this.transform);
            }
        }
        yield return new WaitForSeconds(1.5f);
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
        isattack = false;
    }
    void FrogExplosion() //개구리 몬스터 투사체 패턴
    {
        EffectPb ExPb = ExplosionPb.GetComponent<EffectPb>();
        ExPb.Power = 10;
        ExPb.speed = 4;
        ExPb.dir = PBdir;
        ExPb.DelTime = 2f;
        ExPb.movecheck = 1;
        ExPb.playerpos = player.transform;
        GameObject Explosion = Instantiate(ExplosionPb, AttackBox.position, AttackBox.rotation);
    }

    void PriestRock()  // 3stage 프리스트 공격 이펙트
    {
        EffectPb RkPb = RockPb.GetComponent<EffectPb>();
        RkPb.Power = 10;
        RkPb.dir = PBdir;
        RkPb.DelTime = 0.7f;
        RkPb.playerpos = player.transform;
        Vector2 Pbpos = new Vector2(AttackBox.position.x + (DirX * 2.5f), AttackBox.position.y - 1.4f);

        GameObject Rock = Instantiate(RockPb, Pbpos, AttackBox.rotation);
    }

    void PlantSeed()    //3stage 식물 씨앗 투사체 이펙트
    {
        EffectPb SEPb = SeedPb.GetComponent<EffectPb>();
        SEPb.movecheck = 1;
        SEPb.Power = 10;
        SEPb.speed = 7;
        SEPb.dir = DirX;
        SEPb.DelTime = 1f;
        SEPb.playerpos = player.transform;

        GameObject Seed = Instantiate(SeedPb, AttackBox.position, AttackBox.rotation);
    }

    public IEnumerator Hurt(Transform target, float damage)  //플레이어에게 피격 받았을 때 실행
    {
        if(enemy_CurHP > 0 && !isdie)
        {
            yield return new WaitForSeconds(0);
            ishurt = true;
            enemy_CurHP = enemy_CurHP - damage;
            StartCoroutine(enemyHpbar.HpUpdate());      // 2024-03-30 유재현 추가
            anim.SetBool("Move", false);
            anim.SetTrigger("Hurt");

            StartCoroutine(Blink());

            if (enemy_Type != 3) //충돌 몬스터는 넉백처리 안함
            {
                StartCoroutine(Knockback(target));
            }
            
            if (enemy_CurHP <= 0)
            {
                isdie = true;
                StopAllCoroutines();
                StartCoroutine(enemyHpbar.HpUpdate());      // 2024-03-30 유재현 추가
                StartCoroutine(Die());
            }
        }
        ishurt = false;
    }

    IEnumerator Die()  //몬스터가 죽었을 실행
    {
        istracking = false;
        DirX = 0;
        anim.SetBool("Move", false);
        anim.SetTrigger("Die");
        this.gameObject.layer = LayerMask.NameToLayer("DieEnemy");
        player.money += enemyMoney;
        dm.playerData.money += enemyMoney;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    IEnumerator Knockback(Transform target)
    {
        Vector2 knockbackDirection = (transform.position - target.position).normalized;  // 피격된 위치를 저장하고 방향을 정규화
        knockbackDirection = new Vector2(knockbackDirection.x, 0).normalized;
        float maxKnockbackDistance = 4f;    // 넉백 가능한 최대 거리
        float knockbackDistance = 4.0f;  // 넉백 거리를 나타내는 변수
        rigid.AddForce(knockbackDirection * knockbackDistance, ForceMode2D.Impulse);  // 피격된 위치 * 넉백 거리만큼의 힘을 넉백에 사용
        float distanceTravelled = 0f;  // 이미 이동한 거리를 나타내는 변수

        while (distanceTravelled < maxKnockbackDistance)
        {
            distanceTravelled += knockbackDistance * Time.fixedDeltaTime;  // 이동한 거리를 누적
            yield return new WaitForFixedUpdate();  // Fixed Update마다 체크하여 일정 거리까지만 이동하도록 함
        }
    }

    IEnumerator Blink() // 피격 효과
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1, 1, 1, 1f);
    }

    void OriginSpeed()  //몬스터 원래 이동속도로 변경하는 함수
    {
        enemy_Speed = enemy_OriginSpeed;
    }

    private void OnDrawGizmos()
    {
        // 감지 범위를 시각적으로 나타내기 위한 코드
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public abstract void InitSetting(); // 적의 기본 정보를 설정하는 함수(추상)
}
