using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Boss : MonoBehaviour
{
    public static Boss Instance;
    Player player;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;
    Transform PbSpawn;  //프리펩 생성 위치 오브젝트
    Transform AttackBox;    // 근접 공격 범위 오브젝트
    BoxCollider2D BoxCollider2DSize;    //Attackbox 오브젝트의 boxcollider2D

    bool ishurt = false;
    bool bossMoving = true;
    bool isdie = false;
    int DirX;   //몬스터가 바라보는 방향값
    public float playerLoc; // player의 X좌표
    public float bossLoc;  // boss의 X좌표
    public int atkPattern; //boss 공격 패턴
    int boss_OriginSpeed;   //몬스터의 이전 속도값 저장

    [Header("보스 몬스터 능력치")]
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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = Player.instance.GetComponent<Player>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        AttackBox = this.gameObject.transform.GetChild(0).GetComponent<Transform>();
        PbSpawn = this.gameObject.transform.GetChild(1).GetComponent<Transform>();
        rigid = this.GetComponent<Rigidbody2D>();
        randomAtk();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            StartCoroutine(Hurt(AttackBox));
    }

    public virtual void BossUpdate(Transform target)  // boss용 Update문
    {
        playerLoc = target.position.x;
        bossLoc = this.gameObject.transform.position.x;
        bossMove();
        bossAttack();
    }

    public void bossMove()  // boss의 움직이도록 하는 함수
    {
        if (bossMoving && !isdie)
        {
            gameObject.transform.Translate(new Vector2(DirX, 0) * Time.deltaTime * boss_Speed);
            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }
            
    }
    
    /*
    void bossRoll()
    {
        anim.SetTrigger("Roll");
        float moveDistance = DirX * boss_Speed * 5 * Time.deltaTime;
        gameObject.transform.Translate(new Vector2(moveDistance, 0));
        Debug.Log("굴렀음");
    }
    */
    public void bossAttack()
    {
        if(!isdie)
        {
            if (playerLoc < bossLoc && bossMoving)
            {
                spriteRenderer.flipX = true;
                DirX = -1;
                AttackBox.position = new Vector2(transform.position.x - 1.6f, transform.position.y - 3f);
            }
            else if (playerLoc > bossLoc && bossMoving)
            {
                spriteRenderer.flipX = false;
                DirX = 1;
                AttackBox.position = new Vector2(transform.position.x + 1.6f, transform.position.y - 3f);
            }

            switch (atkPattern)
            {
                case 1: //1은 근접공격으로 고정으로 가까울 때만 실행
                    bossMoving = false;
                    anim.SetTrigger("Attack");
                    anim.SetFloat("Attackpatten", 1);
                    Ranger_Normalattack();
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player")) //충돌 대미지 처리
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Playerhurt(boss_BumpPower);
            }
        }
    }
    public void randomAtk() // 공격 패턴 랜덤으로 정하기
    {
        atkPattern = Random.Range(2, 5);    // 2~4 사이의 숫자 랜덤값을 받음
        if (DirX == 1 && (playerLoc - bossLoc) <= 4f || (DirX == -1 && (playerLoc - bossLoc) >= -4f))    //보스와 플레이어의 거리가 4만큼 이내에 있으면 근접 공격 확정
            atkPattern = 1;
        if (!isdie && boss_CurHP > boss_MaxHP / 2)  // 1페이즈와 2페이즈의 패턴 실행 시간이 다름
            Invoke("randomAtk", 5f);
        else
            Invoke("randomAtk", 3.5f);
    }

    void Ranger_Normalattack()  //근접 공격
    {
        BoxCollider2DSize = this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(AttackBox.position, BoxCollider2DSize.size, 0);

        foreach (Collider2D collider in collider2D)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<Player>().Playerhurt(boss_OnePattenPower);
            }
        }
        Invoke("bossRoll", 0.5f);
        Invoke("MoveOn", 2f);
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
    }

    void Ranger_Arrowattack()   //활 쏘는 공격
    {
        ArrowPb APb = ArrowPb.GetComponent<ArrowPb>();
        APb.Power = boss_TwoPattenPower;
        APb.Dir = DirX;
        APb.DelTime = 3f;
        APb.Arrowpatten = 1;

        GameObject arrow = Instantiate(ArrowPb, PbSpawn.position, PbSpawn.rotation);

        Invoke("MoveOn", 2.5f);
    }

    IEnumerator Ranger_Arrowrain()  //화살비 공격
    {
        ArrowPb ArPb = ArrowrainPb.GetComponent<ArrowPb>();
        Vector2 newPosition = new Vector2(player.transform.position.x, PbSpawn.position.y + 1.1f);  //원래 있는 Pbspawn위치값을 수정해서 새로운 위치 선언
        ArPb.Power = boss_ThreePattenPower;
        ArPb.Dir = DirX;
        ArPb.DelTime = 1.5f;
        ArPb.Arrowpatten = 2;

        yield return new WaitForSeconds(1.5f);
        GameObject arrowrain = Instantiate(ArrowrainPb, newPosition, PbSpawn.rotation);
       
        Invoke("MoveOn", 2.5f);
    }

    IEnumerator Ranger_Laserattack()    //레이져 공격
    {
        Vector2 newPosition = new Vector2(PbSpawn.position.x + (DirX * 9.4f), PbSpawn.position.y - 0.07f);  //원래 있는 Pbspawn위치값을 수정해서 새로운 위치 선언
        ArrowPb LrPb = LaserPb.GetComponent<ArrowPb>();
        LrPb.Power = boss_FourPattenPower;
        LrPb.Dir = DirX;
        LrPb.DelTime = 0.3f;
        LrPb.Arrowpatten = 3;

        yield return new WaitForSeconds(0.9f);
        GameObject arrowrain = Instantiate(LaserPb, newPosition, PbSpawn.rotation);

        Invoke("MoveOn", 2.5f);
    }
    
    IEnumerator Hurt(Transform target)  //플레이어에게 피격 받았을 때 실행
    {
        if (boss_CurHP > 0 && !ishurt)
        {
            ishurt = true;
            boss_CurHP = boss_CurHP - 10;
            anim.SetBool("Move", false);
            anim.SetTrigger("Hurt");

            if (boss_Speed > 0)
                boss_OriginSpeed = boss_Speed;
            boss_Speed = 0;

            if (boss_CurHP <= 0)
            {
                isdie = true;
                StopAllCoroutines();
                StartCoroutine(Die());
                Debug.Log("죽었음");
            }
        }

        yield return new WaitForSeconds(0.3f);
        boss_Speed = boss_OriginSpeed;
        ishurt = false;
    }

    IEnumerator Die()  //몬스터가 죽었을 실행
    {
        DirX = 0;
        bossMoving = false;
        anim.SetBool("Move", false);
        anim.SetBool("Die", true);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    void MoveOn()
    {
        bossMoving = true;
    }

    public abstract void BossInitSetting(); // 적의 기본 정보를 설정하는 함수(추상)
}
