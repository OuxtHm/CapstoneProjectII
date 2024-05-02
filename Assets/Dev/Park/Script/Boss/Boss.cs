using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Boss : MonoBehaviour
{
    public static Boss Instance;
    Player player;
    BossHpBar bossHpBar;        // 2024-04-10 유재현 추가
    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;
    Transform PbSpawn;  //프리펩 생성 위치 오브젝트
    Transform AttackBox;    // 근접 공격 범위 오브젝트
    BoxCollider2D BoxCollider2DSize;    //Attackbox 오브젝트의 boxcollider2D

    public bool ishurt = false;
    bool bossMoving = true;
    bool isdie = false;
    int DirX;   //몬스터가 바라보는 방향값
    public float playerLoc; // player의 X좌표
    public float bossLoc;  // boss의 X좌표
    public int atkPattern = 0; //boss 공격 패턴
    int boss_OriginSpeed;   //몬스터의 이전 속도값 저장
    float distanceToTarget; //플레이어와 보스 사이의 거리
    public int turnPoint = 1;    // 벽에 닿을 시 이동 방법 변경 조건
    int countRange; //패턴 값 범위 조절
    int totalDamage;    // 최종 대미지값

    [Header("보스 몬스터 능력치")]
    public int boss_stage;  //보스별 스테이지 구분
    public float boss_MaxHP; //보스 최대체력
    public float boss_CurHP; //보스 현재체력
    public int boss_Power; //보스 공격력
    public int boss_Speed; //보스 이동속도
    public int boss_BumpPower;    //충돌 대미지
    public int boss_OnePattenPower;   //첫번째 공격 패턴 대미지
    public int boss_TwoPattenPower;   //두번째 공격 패턴 대미지
    public int boss_ThreePattenPower;    //세번째 공격 패턴 대미지
    public int boss_FourPattenPower;   //네번째 공격 패턴 대미지

    [Header("1스테이지 보스 프리펩")]
    public GameObject ArrowPb; // 1스테이지 보스 화살 프리펩
    public GameObject ArrowrainPb; // 1스테이지 보스 화살비 프리펩
    public GameObject LaserPb; // 1스테이지 보스 화살비 프리펩
    public GameObject WarringPb;  //공격 전 위험표시 프리펩

    [Header("2스테이지 보스 프리펩")]
    public GameObject SwordEffectPb; // 2스테이지 보스 가로베기 프리펩
    public GameObject FireEffectPb; // 2스테이지 보스 파이어볼 프리펩

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = Player.instance.GetComponent<Player>();
        bossHpBar = BossHpBar.instance;     // 2024-04-10 유재현 추가
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        AttackBox = this.gameObject.transform.GetChild(0).GetComponent<Transform>();
        PbSpawn = this.gameObject.transform.GetChild(1).GetComponent<Transform>();
        rigid = this.GetComponent<Rigidbody2D>();
        randomAtk();
    }

    public virtual void BossUpdate(Transform target)  // boss용 Update문
    {
        playerLoc = target.position.x;
        bossLoc = this.gameObject.transform.position.x;
        distanceToTarget = Mathf.Abs(target.position.x - transform.position.x); // 거리의 차이를 절대값으로 저장
        bossMove();
        bossAttack();
    }

    public void bossMove()  // boss의 움직이도록 하는 함수
    {
        if(boss_stage == 1)   //벽에 닿을 시 플레이어쪽으로 이동
        {
            if (bossMoving && !isdie && !ishurt)
            {
                if(turnPoint == 1 && distanceToTarget <= 15f)
                {
                    gameObject.transform.Translate(new Vector2(-DirX, 0) * Time.deltaTime * boss_Speed);
                    if (DirX == 1)
                        spriteRenderer.flipX = true;
                    else
                        spriteRenderer.flipX = false;
                }
                else if(turnPoint == -1)
                    gameObject.transform.Translate(new Vector2(DirX, 0) * Time.deltaTime * boss_Speed);

                anim.SetBool("Move", true);
            }
            else
            {
                anim.SetBool("Move", false);
            }
        }
        else if(boss_stage == 2)
        {
            if(bossMoving)
            {
                if (playerLoc < bossLoc)
                {
                    spriteRenderer.flipX = true;
                    DirX = -1;
                }
                else
                {
                    spriteRenderer.flipX = false;
                    DirX = 1;
                }
            }
        }
    }
    public void bossAttack()
    {
        if(!isdie)
        {
            if (playerLoc < bossLoc && !bossMoving)
            {
                spriteRenderer.flipX = true;
                DirX = -1;
                if(boss_stage == 1)
                    AttackBox.position = new Vector2(transform.position.x - 1.6f, transform.position.y);
                else
                    AttackBox.position = new Vector2(transform.position.x - 1.5f, transform.position.y);
            }
            else if (playerLoc > bossLoc && !bossMoving)
            {
                spriteRenderer.flipX = false;
                DirX = 1;
                if (boss_stage == 1)
                    AttackBox.position = new Vector2(transform.position.x + 1.6f, transform.position.y);
                else
                    AttackBox.position = new Vector2(transform.position.x + 1.5f, transform.position.y);
            }
            if(boss_stage == 1)
            {
                if (atkPattern == 1)
                    atkPattern = Random.Range(2, countRange);
                switch (atkPattern)
                {
                    case -1: //1은 근접공격 고정으로 가까울 때만 실행
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 1);
                        totalDamage = boss_OnePattenPower;
                        atkPattern = 0;
                        break;

                    case 2:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 2);
                        //Ranger_Arrowattack(); 애니메이션에서 실행되게 설정함
                        atkPattern = 0;
                        break;

                    case 3:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 3);
                        StartCoroutine(Ranger_Arrowrain());
                        atkPattern = 0;
                        break;

                    case 4:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 4);
                        StartCoroutine(Ranger_Laserattack());
                        atkPattern = 0;
                        break;
                }
            }
            if (boss_stage == 2)
            {
                switch (atkPattern)
                {
                    case 1:
                        bossMoving = false;
                        anim.SetTrigger("Move");
                        Invoke("MoveOn", 3f);
                        atkPattern = 0;
                        break;
                    case 2:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 2);
                        atkPattern = 0;
                        break;

                    case 3:
                        bossMoving = false;
                        anim.SetTrigger("Attack");
                        anim.SetFloat("Attackpatten", 3);
                        atkPattern = 0;
                        break;

                    case 4:
                        //bossMoving = false;
                        Debug.Log(atkPattern);
                        atkPattern = 0;
                        break;
                }
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Player")) //충돌 대미지 처리
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player != null && boss_CurHP > 0)
            {
                player.Playerhurt(boss_BumpPower);
            }
        }

        if (collision != null && collision.gameObject.CompareTag("wall"))   //벽 충돌 처리
        {
            turnPoint *= -1;
            DirX *= -1;
        }
    }
    
    public void randomAtk() // 공격 패턴 랜덤으로 정하기
    {
        atkPattern = Random.Range(1, countRange);    // 2 ~ (countRange - 1) 사이의 숫자 랜덤값을 받음
        if (boss_stage == 1)    //보스와 플레이어의 거리가 4만큼 이내에 있으면 근접 공격 확정
            if(DirX == 1 && (playerLoc - bossLoc) <= 4f || (DirX == -1 && (playerLoc - bossLoc) >= -4f))
                atkPattern = -1;
        if (!isdie && boss_CurHP > boss_MaxHP / 2)  // 1페이즈와 2페이즈의 패턴 실행 시간이 다름
        {
            Invoke("randomAtk", 5f);
            countRange = 4;
        }  
        else
        {
            Invoke("randomAtk", 4f);
            countRange = 5;
        }
            
    }

    void Ranger_Arrowattack()   //1stage 활 쏘는 공격
    {
        EffectPb APb = ArrowPb.GetComponent<EffectPb>();
        APb.Power = boss_TwoPattenPower;
        APb.dir = DirX;
        APb.DelTime = 3f;
        APb.movecheck = 1;
        APb.speed = 20;

        GameObject arrow = Instantiate(ArrowPb, PbSpawn.position, PbSpawn.rotation);

        Invoke("MoveOn", 3f);
    }

    IEnumerator Ranger_Arrowrain()  //1stage 화살비 공격
    {
        EffectPb ArPb = ArrowrainPb.GetComponent<EffectPb>();
        Vector2 Targetpos = new Vector2(player.transform.position.x, PbSpawn.position.y + 1.1f);  //원래 있는 Pbspawn위치값을 수정해서 새로운 위치 선언
        Vector2 Warringpos = new Vector2(player.transform.position.x, PbSpawn.position.y - 2.1f);  //위험 표시 생성 위치
        ArPb.Power = boss_ThreePattenPower;
        ArPb.dir = DirX;
        ArPb.DelTime = 1.1f;
        ArPb.movecheck = 0;

        GameObject Warring = Instantiate(WarringPb, Warringpos, PbSpawn.rotation);  //위험 표시 생성
        yield return new WaitForSeconds(1.5f);
        GameObject arrowrain = Instantiate(ArrowrainPb, Targetpos, PbSpawn.rotation);//화살비 공격 생성

        Destroy(Warring);
        Invoke("MoveOn", 4.5f);
    }

    IEnumerator Ranger_Laserattack()    //1stage 레이져 공격
    {
        Vector2 newPosition = new Vector2(this.transform.position.x + (DirX * 9.8f), this.transform.position.y + 0.45f);  //원래 있는 Pbspawn위치값을 수정해서 새로운 위치 선언
        EffectPb LrPb = LaserPb.GetComponent<EffectPb>();
        LrPb.Power = boss_FourPattenPower;
        LrPb.dir = DirX;
        LrPb.DelTime = 0.3f;
        LrPb.movecheck = 0;

        yield return new WaitForSeconds(0.9f);
        GameObject arrowlaser = Instantiate(LaserPb, newPosition, PbSpawn.rotation);

        Invoke("MoveOn", 5f);
    }

    void giveDamage()   //애니메이션에서 실행되는 대미지값 넘겨주는 함수
    {
        BoxCollider2DSize = this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(AttackBox.position, BoxCollider2DSize.size, 0);

        foreach (Collider2D collider in collider2D)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                player = collider.GetComponent<Player>();
                player.Playerhurt(totalDamage);
            }
        }
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
    }

    IEnumerator Knight_Transparent()  //2stage 투명 공격 , SwordAttack 1
    {
        float fadeDuration = 0.8f;  // 변화에 걸리는 시간
        float elapsedTime = 0f;
        this.gameObject.layer = LayerMask.NameToLayer("DieEnemy");
        while (elapsedTime < fadeDuration)
        {
            spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(spriteRenderer.color.a, 0f, elapsedTime / fadeDuration));  // 알파값 서서히 변경
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.gameObject.transform.position = new Vector2(player.transform.position.x + (DirX > 0 ? -2 : 2), player.transform.position.y);
        yield return new WaitForSeconds(2f);
        this.gameObject.layer = LayerMask.NameToLayer("Boss");
        spriteRenderer.color = new Color(1, 1, 1, 1);
        anim.SetTrigger("Attack");
        anim.SetFloat("Attackpatten", 1);
        totalDamage = boss_OnePattenPower;
    }

    IEnumerator Knight_SwordAttack2()  //2stage 가로베기 공격, SwordAttack 2
    {
        Vector2 Spownpos = new Vector2(this.transform.position.x , this.PbSpawn.position.y);
        this.gameObject.transform.position = new Vector2(transform.position.x + (DirX > 0 ? 10 : -10), transform.position.y);

        yield return new WaitForSeconds(0.4f);
        EffectPb SEfPb = SwordEffectPb.GetComponent<EffectPb>();
        SEfPb.dir = DirX;
        SEfPb.Power = boss_TwoPattenPower;
        SEfPb.DelTime = 0.6f;
        SEfPb.movecheck = 0;

        GameObject effect = Instantiate(SwordEffectPb, Spownpos, PbSpawn.rotation);
        Invoke("MoveOn", 3.5f);
    }
    IEnumerator Knight_LightAttack()
    {
        Vector2 Spownpos = new Vector2(this.transform.position.x + DirX, this.PbSpawn.position.y);

        yield return new WaitForSeconds(0.1f);
        EffectPb FirePb = FireEffectPb.GetComponent<EffectPb>();
        FirePb.dir = DirX;
        FirePb.Power = boss_ThreePattenPower;
        FirePb.DelTime = 1f;
        FirePb.movecheck = 1;
        FirePb.speed = 15;

        GameObject effect = Instantiate(FireEffectPb, Spownpos, PbSpawn.rotation);
        Invoke("MoveOn", 3.5f);
    }
    
    public IEnumerator Hurt(Transform target, float Damage)  //플레이어에게 피격 받았을 때 실행
    {
        yield return new WaitForSeconds(0);
        if (boss_CurHP > 0 && !ishurt)
        {
            ishurt = true;
            boss_CurHP = boss_CurHP - Damage;
            anim.SetBool("Move", false);
            anim.SetTrigger("Hurt");
            //StartCoroutine(bossHpBar.FrontHpUpdate());      // 2024-04-10 유재현 추가
            //bossHpBar.anim.SetTrigger("Damage");
            StartCoroutine(Blink());

            if (boss_Speed > 0)
                boss_OriginSpeed = boss_Speed;
            boss_Speed = 0;

            if (boss_CurHP <= 0)
            {
                isdie = true;
                StopAllCoroutines();
                //StartCoroutine(bossHpBar.FrontHpUpdate());      // 2024-04-10 유재현 추가
                //bossHpBar.anim.SetTrigger("Remove");
                StartCoroutine(Die());
            }
            Invoke("OriginSpeed", 0.3f);
        }
        ishurt = false;
    }
    IEnumerator Blink() // 피격 효과
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    IEnumerator Die()  //보스가 죽었을 실행
    {
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        this.gameObject.layer = LayerMask.NameToLayer("DieEnemy");
        DirX = 0;
        bossMoving = false;
        anim.SetTrigger("Die");
        anim.SetBool("Move", false);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    void MoveOn()
    {
        if(!isdie)
            bossMoving = true;
    }
    void OriginSpeed()  //몬스터 원래 이동속도로 변경하는 함수
    {
        boss_Speed = boss_OriginSpeed;
    }



    public abstract void BossInitSetting(); // 적의 기본 정보를 설정하는 함수(추상)
}
