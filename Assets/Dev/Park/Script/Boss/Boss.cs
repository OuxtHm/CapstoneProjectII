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
    public GameObject ArrowPb; // 화살 프리펩
    public GameObject ArrowrainPb; // 화살비 프리펩

    bool bossMoving = true;
    int DirX;   //몬스터가 바라보는 방향값
    public float playerLoc; // player의 X좌표
    public float bossLoc;  // boss의 X좌표
    public int atkPattern; //boss 공격 패턴

    [Header("보스 몬스터 능력치")]
    public int boss_MaxHP; //보스 최대체력
    public int boss_CurHP; //보스 현재체력
    public int boss_Power; //보스 공격력
    public int boss_Speed; //보스 이동속도

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = GetComponent<Player>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        PbSpawn = this.gameObject.transform.GetChild(1).GetComponent<Transform>();
        AttackBox = this.gameObject.transform.GetChild(0).GetComponent<Transform>();
        rigid = this.GetComponent<Rigidbody2D>();
        randomAtk();
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
        if (bossMoving)
        {
            gameObject.transform.Translate(new Vector2(DirX, 0) * Time.deltaTime * boss_Speed);
            anim.SetBool("Move", true);
        }
        else
            anim.SetBool("Move", false);
    }

    public void bossAttack()
    {
        if (playerLoc < bossLoc)
        {
            spriteRenderer.flipX = true;
            DirX = -1;
            AttackBox.position = new Vector2(transform.position.x - 1.6f, -2);

        }
        else if(playerLoc > bossLoc)
        {
            spriteRenderer.flipX = false;
            DirX = 1;
            AttackBox.position = new Vector2(transform.position.x + 1.6f, -2);
        }

        switch (atkPattern)
        {
            case 1:
                anim.SetTrigger("Attack");
                anim.SetFloat("Attackpatten", 1);
                //Ranger_Arrowattack(); 애니메이션에서 실행되게 설정함
                atkPattern = 0;
                break;

            case 2:
                anim.SetTrigger("Attack");
                anim.SetFloat("Attackpatten", 2);
                Ranger_Normalattack();
                atkPattern = 0;
                break;

            case 3:
                bossMoving = false;
                anim.SetTrigger("Attack");
                anim.SetFloat("Attackpatten", 3);
                Ranger_Arrowrain();
                atkPattern = 0;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Playerhurt(boss_Power);
            }
            else
                Debug.Log("플레이어를 못 불러옴");
        }
    }
    public void randomAtk() // 공격 패턴 랜덤으로 정하기
    {
        Debug.Log("atkPattern");
        atkPattern = Random.Range(1, 4);
        Invoke("randomAtk", 4f);
    }

    void Ranger_Arrowattack()   //활 쏘는 공격
    {
        bossMoving = false;
        GameObject arrow = Instantiate(ArrowPb, PbSpawn.position, PbSpawn.rotation);
        ArrowPb APb = ArrowPb.GetComponent<ArrowPb>();

        APb.Power = boss_Power;
        APb.Dir = DirX;
        APb.DelTime = 3f;
        Invoke("MoveOn", 3f);
    }

    void Ranger_Normalattack()  //화살로 찌르는 근접 공격
    {
        bossMoving = false;
        BoxCollider2DSize = this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(AttackBox.position, BoxCollider2DSize.size, 0);

        foreach (Collider2D collider in collider2D)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<Player>().Playerhurt(boss_Power);
            }
        }
        Invoke("MoveOn", 3f);
        this.gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
    }

    void Ranger_Arrowrain()
    {
        bossMoving = false;

        Vector3 newPosition = new Vector3(PbSpawn.position.x + 6f, PbSpawn.position.y + 1.2f, PbSpawn.position.z);  //원래 있는 Pbspawn위치값을 수정해서 새로운 위치 선언
        GameObject arrowrain = Instantiate(ArrowrainPb, newPosition, PbSpawn.rotation);
        ArrowPb ArPb = arrowrain.GetComponent<ArrowPb>(); // 오류 수정: ArrowPb -> arrowrain

        ArPb.Power = boss_Power;
        ArPb.Dir = DirX;
        ArPb.DelTime = 10f;
        Invoke("MoveOn", 3f);
    }

    void MoveOn()
    {
        bossMoving = true;
    }

    public abstract void BossInitSetting(); // 적의 기본 정보를 설정하는 함수(추상)
}
