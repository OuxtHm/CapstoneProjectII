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
    Transform PbSpawn;
    public GameObject ArrowPb; // 화살 프리펩

    bool bossMoving = true;
    int DirX;   //몬스터가 바라보는 방향값
    public float playerLoc; // player의 X좌표
    public float bossLoc;  // boss의 X좌표

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
        //rigid = this.GetComponent<Rigidbody2D>();
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
        {
            anim.SetBool("Move", false);
        }
    }

    public void bossAttack()
    {
        if (playerLoc < bossLoc)
        {
            spriteRenderer.flipX = true;
            DirX = -1;
        }
        else if(playerLoc > bossLoc)
        {
            spriteRenderer.flipX = false;
            DirX = 1;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            bossMoving = false;
            anim.SetTrigger("Attack");
            Ranger_Arrowattack();
        }
            

        if (playerLoc - bossLoc < 0.2f)
            bossMoving = false;
        else
            bossMoving = true;
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

    void Ranger_Arrowattack()
    {
        GameObject arrow = Instantiate(ArrowPb, PbSpawn.position, PbSpawn.rotation);
        ArrowPb APb = ArrowPb.GetComponent<ArrowPb>();

        APb.Power = boss_Power;
        APb.Dir = DirX;
        APb.DelTime = 3f;
    }

    public abstract void BossInitSetting(); // 적의 기본 정보를 설정하는 함수(추상)
}
